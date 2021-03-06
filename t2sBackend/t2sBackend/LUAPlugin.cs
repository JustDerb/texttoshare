﻿using LuaInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            {"coroutine.create", true},
            {"coroutine.resume", true},
            {"coroutine.running", true},
            {"coroutine.status", true},
            {"coroutine.wrap", true},
            {"coroutine.yield", true},
            {"coroutine", false},
            {"module", false},
            {"require", false},
            //{"package.*", false},
            {"package", false},
            //{"package.loaded", false},
            //{"package.loaders", false},
            //{"package.loadlib", false},
            //{"package.path/package.cpath", false},
            //{"package.preload", false},
            //{"package.seeall", false},
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
            {"string", false},
            {"table.insert", true},
            {"table.maxn", true},
            {"table.remove", true},
            {"table.sort", true},
            {"table", false},
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
            {"math", false},
            //{"io.*", false},
            //{"io.read", true},
            //{"io.write", true},
            //{"io.flush", true},
            //{"io.type", true},
            {"io", false},
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
            {"os", false},
            //{"debug.*", false},
            {"debug", false},
            {"newproxy", false}
            #endregion
        };

        /// <summary>
        /// Engine that is used to run the script
        /// </summary>
        public Lua LuaEngine
        {
            get;
            protected set;
        }

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
            : this(plugin, plugin.Name)
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

            this.ScriptFileLoc = LUADefinitions.getLuaScriptLocation(script);

            // See if it's there
            if (!File.Exists(this.ScriptFileLoc))
            {
                throw new ArgumentException("Cannot find file " + this.ScriptFileLoc);
            }

            this.PluginDAO = plugin;

            // Store sandbox code in a variable so we only call it once
            this.SandboxLuaCode = GetSandBoxCode();
        }

        ~LUAPlugin()
        {
            if (this.LuaEngine != null)
            {
                this.LuaEngine.Close();
                //this.LuaEngine.Dispose();
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
            this.LuaEngine = new Lua();
            this.LuaEngine.HookException += LuaEngine_HookException;
            this.LuaEngine.DebugHook += LuaEngine_DebugHook;
            // Sandbox us so we can't do anything lethal
            this.LuaEngine.DoString(this.SandboxLuaCode);
        }

        private void RunLuaCode(Lua engine, String location)
        {
            engine.DoFile(location);
        }

        private void registerFailedRun(ParsedMessage message, AWatcherService service, t2sDbLibrary.IDBController controller, Exception ex)
        {
            // Increment the error counter
            controller.IncrementPluginFailedAttemptCount(this.PluginDAO.PluginID);

            // Disable if above threshold
            int count = controller.GetPluginFailedAttemptCount(this.PluginDAO.PluginID);
            Logger.LogMessage(this.PluginDAO.Name + @": " + ex.Message, LoggerLevel.WARNING);
            if (count > LUADefinitions.DisablePluginAboveErrorCount)
            {
                controller.DisableGlobalPlugin(this.PluginDAO.PluginID);
                Logger.LogMessage(this.PluginDAO.Name + @": Plugin has been disabled", LoggerLevel.WARNING);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("LUAPlugin.cs: ");
            sb.Append(this.PluginDAO.Name);
            sb.Append(" failed (count=");
            sb.Append(count);
            sb.Append("/");
            sb.Append(LUADefinitions.DisablePluginAboveErrorCount);
            sb.Append("): ");
            sb.Append(ex.Message);
            Logger.LogMessage(sb.ToString(), LoggerLevel.WARNING);

            // Message the user saying it failed
            Message failedMsg = new Message(new string[1] { message.Sender.PhoneEmail }, "");
            failedMsg.FullMessage = "Plugin " + this.PluginDAO.Name + " has failed to run. Please try again later.";
            service.SendMessage(failedMsg);
        }

        public void Run(ParsedMessage message, AWatcherService service, t2sDbLibrary.IDBController controller)
        {
            String engineHash = null;

            Task LuaCodeTask = null;
            try
            {
                // Get to a clean state
                loadNewLuaEngine();

                // Register our plugin so we can call C# methods
                engineHash = LuaScriptingEngine.registerPlugin(this, message, service, controller, this.LuaEngine);

                Thread LuacodeThread = null;
                // Run the script
                LuaCodeTask = new Task(() =>
                {
                    LuacodeThread = Thread.CurrentThread;
                    RunLuaCode(this.LuaEngine, this.ScriptFileLoc);
                });
                LuaCodeTask.Start();

                bool finished = true;

                try
                {
                    if (!LuaCodeTask.Wait(LUADefinitions.MaxRunTime))
                    {
                        finished = false;

                        // Sorry Lua Code, gotta die now
                        LuacodeThread.Abort();
                    }
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerExceptions.Count > 0)
                        throw ex.InnerExceptions[0];
                }

                if (!finished)
                    registerFailedRun(message, service, controller, 
                        new LuaException("Lua script ran too long. Max runtime is: " + LUADefinitions.MaxRunTime.ToString()));
            }
            catch (LuaException ex)
            {
                registerFailedRun(message, service, controller, ex);
            }
            finally
            {
                if (engineHash != null)
                {
                    if (!LuaScriptingEngine.unregisterPlugin(engineHash))
                        Logger.LogMessage("LUAPlugin.Run: Couldn't unregister plugin! (" + this.PluginDAO.Name + ")", LoggerLevel.SEVERE);
                }
            }

        }

        /// <summary>
        /// Does simple syntax checking of a LUA plugin.  NOTE: Doe not find runtime errors!
        /// </summary>
        /// <param name="code">Code to compile</param>
        /// <param name="suppressExceptions">Set to false if you would like to see 
        /// the exception thrown, good for debugging</param>
        /// <returns>True if compiling to bytecode is successful</returns>
        public static Boolean canCompile(String code, Boolean suppressExceptions = true)
        {
            Lua tempLua = null;
            try
            {
                tempLua = new Lua();
                tempLua.LoadString(code, "CompileCheck");
            }
            catch (Exception ex)
            {
                if (suppressExceptions)
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
