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
            ParseCommands(args);
           
        }

        private static void ParseCommands(string[] args)
        {
            GoPathCommands cmds = new GoPathCommands();
            PathConfigManager pcm = new PathConfigManager();

            switch(args[0])
            {
                case "a":
                    pcm.AddGoPath(args[1], args[2]);
                    break;
                case "c":
                    if(!ChangePath(args[1]))
                    {
                        Console.WriteLine("Key not found, or Unable to create path");
                    }
                    break;
                case "l":
                    ListPaths();
                    break;
                case "d":
                    DeletePath(args[1]);
                    break;
                case "u":
                    UpdatePath(args[1], args[2]);
                    break;
                default:
                    break;
            }
        }

        //TODO: Revise to take in full array of arguments.  Perform update options according to user input
        /// <summary>
        /// Updates path values specified by the user
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void UpdatePath(string key, string value)
        {
            PathConfigManager pcm = new PathConfigManager();

            if (!Path.IsPathRooted(value))
                pcm.UpdateGoPathKey(key, value);
            else
                pcm.UpdateGoPath(key, value);
        }

        /// <summary>
        /// Removes path from stored path configuration
        /// </summary>
        /// <param name="key"></param>
        private static void DeletePath(string key)
        {
            PathConfigManager pcm = new PathConfigManager();
            GoPathCommands cmds = new GoPathCommands();

            var paths = pcm.GetPaths();
            

            foreach(KeyValuePair<string, string> kvp in paths)
            {
                if(key == kvp.Key || key == kvp.Value)
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

            Console.WriteLine("KEY \t DIRECTORY");
            foreach(KeyValuePair<string, string> kvp in paths)
            {
                Console.WriteLine(string.Format("{0} \t {1}", kvp.Key, kvp.Value));
            }

        }


        private static bool ChangePath(string key)
        {
            PathConfigManager pcm = new PathConfigManager();
            GoPathCommands cmds = new GoPathCommands();
            var paths = pcm.GetPaths();
            if(!Path.IsPathRooted(key))
            {
                foreach(KeyValuePair<string, string> kvp in paths)
                {
                    if(kvp.Key.Equals(key))
                    {
                        cmds.ChangeGoPath(kvp.Value);
                        return true;
                    }
                }
            }
            else
            {
                if (!Directory.Exists(key))
                    Directory.CreateDirectory(key);

                cmds.ChangeGoPath(key);
                return true;
            }

            return false;
            
        }
    }
}
