using LuaInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    public class LUAPlugin : IPlugin
    {
        // For sandboxing see: 
        //   http://stackoverflow.com/questions/6171381/luainterface-how-to-restrict-access-to-net-classes
        // For calls the are safe/unsafe see:
        //   http://lua-users.org/wiki/SandBoxes

        protected Dictionary<String, Boolean> LuaCommands = new Dictionary<String, Boolean>()
        {
            #region "LUA Command Black/whitelist"
            {"assert", true},
            {"collectgarbage", false},
            {"dofile", false},
            {"error", true},
            {"_G", false},
            {"getfenv", false},
            {"getmetatable", false},
            {"ipairs", true},
            {"load", false},
            {"loadfile", false},
            {"loadstring", false},
            {"next", true},
            {"pairs", true},
            {"pcall", true},
            {"print", true},
            {"rawequal", false},
            {"rawget", false},
            {"rawset", false},
            {"select", true},
            {"setfenv", false},
            {"setmetatable", false},
            {"tonumber", true},
            {"tostring", true},
            {"type", true},
            {"unpack", true},
            {"_VERSION", true},
            {"xpcall", true},
            {"coroutine", false},
            {"coroutine.create", true},
            {"coroutine.resume", true},
            {"coroutine.running", true},
            {"coroutine.status", true},
            {"coroutine.wrap", true},
            {"coroutine.yield", true},
            {"module", false},
            {"require", false},
            {"package", false},
            {"package.*", false},
            {"package.loaded", false},
            {"package.loaders", false},
            {"package.loadlib", false},
            {"package.path/package.cpath", false},
            {"package.preload", false},
            {"package.seeall", false},
            {"string", false},
            {"string.byte", true},
            {"string.char", true},
            {"string.dump", false},
            {"string.find", true},
            {"string.format", true},
            {"string.gmatch", true},
            {"string.gsub", true},
            {"string.len", true},
            {"string.lower", true},
            {"string.match", true},
            {"string.rep", true},
            {"string.reverse", true},
            {"string.sub", true},
            {"string.upper", true},
            {"table", false},
            {"table.insert", true},
            {"table.maxn", true},
            {"table.remove", true},
            {"table.sort", true},
            {"math", false},
            {"math.abs", true},
            {"math.acos", true},
            {"math.asin", true},
            {"math.atan", true},
            {"math.atan2", true},
            {"math.ceil", true},
            {"math.cos", true},
            {"math.cosh", true},
            {"math.deg", true},
            {"math.exp", true},
            {"math.floor", true},
            {"math.fmod", true},
            {"math.frexp", true},
            {"math.huge", true},
            {"math.ldexp", true},
            {"math.log", true},
            {"math.log10", true},
            {"math.max", true},
            {"math.min", true},
            {"math.modf", true},
            {"math.pi", true},
            {"math.pow", true},
            {"math.rad", true},
            {"math.random", true},
            {"math.randomseed", false},
            {"math.sin", true},
            {"math.sinh", true},
            {"math.sqrt", true},
            {"math.tan", true},
            {"math.tanh", true},
            {"io", false},
            {"io.*", false},
            {"io.read", true},
            {"io.write", true},
            {"io.flush", true},
            {"io.type", true},
            {"os", false},
            {"os.clock", true},
            {"os.date", false},
            {"os.difftime", true},
            {"os.execute", false},
            {"os.exit", false},
            {"os.getenv", false},
            {"os.remove", false},
            {"os.rename", false},
            {"os.setlocale", false},
            {"os.time", true},
            {"os.tmpname", false},
            {"debug", false},
            {"debug.*", false},
            {"newproxy", false}
            #endregion
        };

        /// <summary>
        /// Engine that is used to run the script
        /// </summary>
        protected Lua LuaEngine;

        /// <summary>
        /// File location of the script to run
        /// </summary>
        protected String ScriptFileLoc;

        /// <summary>
        /// Sandbox code that is run before any script is run for security reasons
        /// </summary>
        protected readonly String SandboxLuaCode;

        public PluginDAO PluginDAO
        {
            get;
            set;
        }

        /// <summary>
        /// Constructs a basic Lua plugin given the PluginDAO.
        /// Finds the file of the plugin script by:
        /// <code>LUAPlugin.getLuaScriptLocation(plugin.Name)</code>
        /// </summary>
        /// <param name="plugin">Plugin object to construct this object against</param>
        public LUAPlugin(PluginDAO plugin) 
            : this(plugin, LUAPlugin.getLuaScriptLocation(plugin.Name))
        {
            // Pass to more defined constructor
        }

        /// <summary>
        /// Constructs a basic Lua plugin given the PluginDAO.
        /// </summary>
        /// <param name="plugin">Plugin object to construct this object against</param>
        /// <param name="script">Script that the plugin will run (If it does not have the 
        /// <code>LUAPlugin.LuaExtension</code> at the end of the string, it will be appended.</param>
        public LUAPlugin(PluginDAO plugin, String script)
        {
            if (plugin == null || script == null)
            {
                throw new ArgumentNullException();
            }
            // Make sure we are using the file extension
            script = script.Trim();
            if (!script.EndsWith(LUADefinitions.LuaExtension))
            {
                script += LUADefinitions.LuaExtension;
            }
            // See if it's there
            if (!File.Exists(script))
            {
                throw new ArgumentException("Cannot find file " + script);
            }
            
            this.PluginDAO      = plugin;
            this.ScriptFileLoc  = getLuaScriptLocation(script);
            // Store sandbox code in a variable so we only call it once
            this.SandboxLuaCode = GetSandBoxCode();
        }

        /// <summary>
        /// gets the default location of where a Lua Script will be loaded from
        /// </summary>
        /// <param name="name">Name of script</param>
        /// <returns></returns>
        public static String getLuaScriptLocation(String name)
        {
            // TODO - Make this more elegant and use a File object
            String basePath = LUADefinitions.LuaScriptLocation.Trim().TrimEnd('\\');
            if (name == null)
                return basePath;
            name = name.Trim();
            if (name.Equals(String.Empty))
                return basePath;
            else if (name.StartsWith(@"/") || name.StartsWith(@"\"))
                return basePath + name;
            else
                return basePath + @"\" + name;
        }

        ~LUAPlugin()
        {
            if (this.LuaEngine != null)
            {
                this.LuaEngine.Close();
                this.LuaEngine.Dispose();
            }
        }

        /// <summary>
        /// Clears the previous Lua state and re-creates the engine
        /// </summary>
        private void loadNewLuaEngine()
        {
            // Kill our previous object, if any
            if (this.LuaEngine != null)
            {
                this.LuaEngine.Close();
                this.LuaEngine.Dispose();
            }
            this.LuaEngine                = new Lua();
            this.LuaEngine.HookException += LuaEngine_HookException;
            this.LuaEngine.DebugHook     += LuaEngine_DebugHook;
            // Sandbox us so we can't do anything lethal
            this.LuaEngine.DoString(this.SandboxLuaCode);
        }

        public void Run(ParsedMessage message, AWatcherService service, t2sDbLibrary.IDBController controller)
        {
            // Register our plugin so we can call C# methods
            String engineHash = LuaScriptingEngine.registerPlugin(this, service, controller, this.LuaEngine);

            try
            {
                // Get to a clean state
                loadNewLuaEngine();

                // Run the script
                this.LuaEngine.DoFile(this.ScriptFileLoc);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                LuaScriptingEngine.unregisterPlugin(engineHash);
            }

        }

        /// <summary>
        /// Does simple syntax checking of a LUA plugin.  NOTE: Doe not find runtime errors!
        /// </summary>
        /// <param name="code">Code to compile</param>
        /// <param name="surpressExceptions">Set to false if you would like to see 
        /// the exception thrown, good for debugging</param>
        /// <returns>True if compiling to bytecode is successful</returns>
        public static Boolean canCompile(String code, Boolean surpressExceptions = true)
        {
            Lua tempLua = null;
            try
            {
                tempLua = new Lua();
                tempLua.LoadString(code, "CompileCheck");
            }
            catch (Exception ex)
            {
                if (surpressExceptions)
                    return false;
                else
                    throw ex;
            }
            finally
            {
                if (tempLua != null)
                    tempLua.Close();
            }

            return true;
        }

        /// <summary>
        /// Retrieves
        /// </summary>
        /// <returns></returns>
        protected String GetSandBoxCode()
        {
            // Get a good estimate of the size we'll need
            StringBuilder sandboxing = new StringBuilder(this.LuaCommands.Count * 10 + 30);

            //make a table for all the classes you want to expose
            sandboxing.Append("safeCalls = {}\n");
            //store all the ones you want
            //sandboxing.Append("safeCalls.Form = luanet.System.Windows.Forms.Form\n");

            //remove access to LuaInterface
            sandboxing.Append("luanet = nil\n");
            sandboxing.Append("package.loaded.luanet = nil\n");
            //prevent future packages from being loaded
            foreach (KeyValuePair<String, Boolean> keyValue in this.LuaCommands)
            {
                // If we are disabling it, 'nil' it from the sandbox
                if (keyValue.Value == false)
                {
                    sandboxing.Append(keyValue.Key);
                    sandboxing.Append(" = nil\n");
                }
            }

            return sandboxing.ToString();
        }

        protected static void LuaEngine_HookException(object sender, LuaInterface.HookExceptionEventArgs e)
        {
            Console.WriteLine("EXCEPTION: " + e.Exception.ToString());
        }

        protected static void LuaEngine_DebugHook(object sender, LuaInterface.DebugHookEventArgs e)
        {
            Console.WriteLine("DEBUG: " + e.LuaDebug.ToString());
        }
    }
}
