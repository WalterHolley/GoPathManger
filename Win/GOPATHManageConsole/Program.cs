using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GOPATHLib;



/*commands
*  c - change path 
*  a - add path
*  d - delete path
*  u - update path
*  l - list paths
* 
*/



namespace GOPATHManageConsole
{
    class Program
    {
        static void Main(string[] args)
        {   
            var performTask = Init();
            
            if(!performTask && args[0] == "a" || performTask)
                ParseCommands(args);
           
        }


        private static bool Init()
        {
            PathConfigManager pcm = new PathConfigManager();

            var paths = pcm.GetPaths();

            if(paths.Count == 0)
            {
                GoPathCommands cmds = new GoPathCommands();
                var gPath = cmds.GetCurrentGoPath();

                switch(gPath)
                {
                    case Constants.PATH_NOT_FOUND_ERROR:
                        Console.WriteLine("No Go Path is currently set");
                        return false;
                    case Constants.ROOT_NOT_FOUND_ERROR:
                        Console.WriteLine("Go does not appear to be installed");
                        return false;
                    default:
                        pcm.AddGoPath("default", gPath);
                        Console.WriteLine("Current path discovered.  Added as 'default'");
                        ListPaths();
                        break;

                }
            }
            return true;
        }


        private static void ParseCommands(string[] args)
        {
            GoPathCommands cmds = new GoPathCommands();
            PathConfigManager pcm = new PathConfigManager();
            string[] parameters = new string[args.Length - 1];
            
            Array.Copy(args, 1, parameters, 0, args.Length - 1);

            if(parameters.Length == 0 && args[0] != "l")
            {
                Console.WriteLine("No arguments");
                return;
            }


            switch(args[0])
            {
                case "a":
                    AddPath(parameters);
                    break;
                case "c":
                    ChangePath(parameters);
                    break;
                case "l":
                    ListPaths();
                    break;
                case "d":
                    DeletePath(parameters);
                    break;
                case "u":
                    UpdatePath(parameters);
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Adds a go path to the path configuration list
        /// </summary>
        /// <param name="args">array of add path arguments from the console</param>
        private static void AddPath(string[] args)
        {
            PathConfigManager pcm = new PathConfigManager();
            try 
            {
                pcm.AddGoPath(args[0], args[1]);
                Console.WriteLine("Path successfully added.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Path add failed: " + ex.Message);
            }
            
        }

        //TODO: Revise to take in full array of arguments.  Perform update options according to user input
        /// <summary>
        /// Updates path values specified by the user
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void UpdatePath(string[] args)
        {
            PathConfigManager pcm = new PathConfigManager();
            if(args.Length > 2)
            {
                Console.WriteLine("Invalid number of arguments");
                return;
            }
            else
            {
                if (!Path.IsPathRooted(args[1]))
                {
                    Console.WriteLine(String.Format("Changing key from {0} to {1}", args[0], args[1]));
                   if(pcm.UpdateGoPathKey(args[0], args[1]))
                       Console.WriteLine("Key updated");
                   else
                       Console.WriteLine("Key update failed");

                    
                }
                else
                {
                    Console.WriteLine("Updating path for key " + args[0]);
                    pcm.UpdateGoPath(args[0], args[1]);
                }

                
            }

        }

        /// <summary>
        /// Removes path from stored path configuration
        /// </summary>
        /// <param name="key"></param>
        private static void DeletePath(string[] args)
        {
            PathConfigManager pcm = new PathConfigManager();
            GoPathCommands cmds = new GoPathCommands();

            var paths = pcm.GetPaths();
            

            foreach(KeyValuePair<string, string> kvp in paths)
            {
                if(args[0] == kvp.Key || args[0] == kvp.Value)
                {
                    pcm.RemoveGoPath(kvp.Key);
                    Console.WriteLine("Path Removed");
                    break;
                }
            }
        }
        /// <summary>
        /// Writes a list of paths to the console
        /// </summary>
        private static void ListPaths()
        {
            PathConfigManager pcm = new PathConfigManager();
            var paths = pcm.GetPaths();

            if(paths.Count == 0)
            {
                Console.WriteLine("No Paths currently set");
                return;
            }

            Console.WriteLine("KEY \t\t DIRECTORY");
            foreach(KeyValuePair<string, string> kvp in paths)
            {
                Console.WriteLine(string.Format("{0} \t {1}", kvp.Key, kvp.Value));
            }

        }


        /// <summary>
        /// Change the current Go path on the system
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool ChangePath(string[] args)
        {
            PathConfigManager pcm = new PathConfigManager();
            GoPathCommands cmds = new GoPathCommands();
            var paths = pcm.GetPaths();
            if(!Path.IsPathRooted(args[0]))
            {
                foreach(KeyValuePair<string, string> kvp in paths)
                {
                    if (kvp.Key.Equals(args[0]))
                    {
                        cmds.ChangeGoPath(kvp.Value);
                        return true;
                    }
                }
            }
            else
            {
                if (!Directory.Exists(args[0]))
                    Directory.CreateDirectory(args[0]);

                cmds.ChangeGoPath(args[0]);
                return true;
            }

            return false;
            
        }
    }
}
