using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class LUAPlugin : IPlugin
    {
        // For sandboxing see: http://stackoverflow.com/questions/6171381/luainterface-how-to-restrict-access-to-net-classes

        //public static void Main()
        //{
        //    String sandboxing = "";
        //    sandboxing += "--make a table for all the classes you want to expose\n";
        //    sandboxing += "safeClasses = {}\n";
        //    sandboxing += "--store all the ones you want\n";
        //    sandboxing += "safeClasses.Form = luanet.System.Windows.Forms.Form\n";
        //    sandboxing += "--etc...\n";

        //    sandboxing += "--remove access to LuaInterface\n";
        //    sandboxing += "luanet = nil\n";
        //    sandboxing += "package.loaded.luanet = nil\n";
        //    sandboxing += "--prevent future packages from being loaded\n";
        //    sandboxing += "require = nil\n";
        //    sandboxing += "package.loadlib = nil\n";

        //    LuaInterface.Lua lua = new LuaInterface.Lua();
        //    lua.DebugHook += lua_DebugHook;
        //    lua.HookException += lua_HookException;
        //    lua.DoString(sandboxing);
        //    Console.WriteLine("Executing lua script...");
        //    try
        //    {
        //        lua.DoFile("test.lua");
        //    }
        //    catch (LuaInterface.LuaScriptException e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //    Console.WriteLine("DONE!");
        //    Console.WriteLine("Listing variables:");
        //    foreach (string global in lua.Globals)
        //    {
        //        Console.WriteLine(global);
        //    }
        //    Console.WriteLine(lua["width"]);
        //    Console.WriteLine("DONE!");
        //    lua.Close();
        //    Console.ReadLine();
        //}

        public void Run(ParsedMessage message, AWatcherService service)
        {
            throw new NotImplementedException();
        }

        public PluginDAO PluginDAO
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected static void lua_HookException(object sender, LuaInterface.HookExceptionEventArgs e)
        {
            Console.WriteLine("EXCEPTION: " + e.Exception.ToString());
        }

        protected static void lua_DebugHook(object sender, LuaInterface.DebugHookEventArgs e)
        {
            Console.WriteLine("DEBUG: " + e.LuaDebug.ToString());
        }
    }
}
