using Serilog;
using System;
using System.IO;

namespace JurassicDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Init();

            Log.Debug("Application args: {Args}", args);

            if (args.Length != 1)
            {
                Log.Information("Usage: {Application} path_to_file", Path.GetFileName(Environment.GetCommandLineArgs()[0]));
            }
            else
            {
                Exec(args[0]);
            }

            // ==== debug stuff
            Console.WriteLine("=> Program finished. Press any key to exit");
            // pause at exit
            Console.ReadKey();
        }

        static void Init()
        {
            Log.Logger = new LoggerConfiguration()
                //.WriteTo.Console()
                //.WriteTo.File("logs\\myapp-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
                .ReadFrom.AppSettings()
                .CreateLogger();
        }

        static void Exec(string filepath)
        {
            if (!File.Exists(filepath))
                Log.Error("File not exists: {Filepath}", filepath);

            try
            {
                var engine = new Jurassic.ScriptEngine();
                //engine.EnableDebugging = true;
                engine.SetGlobalValue("console",
                    new Jurassic.Library.FirebugConsole(engine)
                    {
                        Output = new SerilogConsoleOutput()
                    });
                engine.SetGlobalFunction("CreateService",
                    new Func<string, string, string, bool>((srvName, exePath, displayName) =>
                    {
                        try
                        {
                            ServiceTools.ServiceInstaller.CreateService(srvName, exePath, displayName);
                            return true;
                        } catch(Exception ex)
                        {
                            Log.Error(ex, "CreateService exception:");
                            return false;
                        }
                    }));

                engine.SetGlobalFunction("DeleteService",
                    new Func<string, bool>((srvName) =>
                    {
                        try
                        {
                            ServiceTools.ServiceInstaller.Uninstall(srvName);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, "DeleteService exception:");
                            return false;
                        }
                    }));
                engine.ExecuteFile(filepath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exec exception:");
            }
        }
    }
}
