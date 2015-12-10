using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOPATHManage
{
    public static class Constants
    {
        public const string GOPATH_CONFIG_ELEMENT = "GoPaths";
        public const string GOPATH_PATH_ELEMENT = "Path";
        public const string GOPATH_CONFIG_FILE = "GoPaths.config";
        public const string GOPATH_SYSTEM_VARIABLE = "GOPATH";
        public const string GOROOT_SYSTEM_VARIABLE = "GOROOT";
        public const string GO_REGISTRY_LOCATION = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment\";
        public const string PATH_NOT_FOUND_ERROR = "NOT_FOUND";
        public const string ROOT_NOT_FOUND_ERROR = "ROOT_NOT_FOUND";

        public enum KeyMethod
        {
            ADD = 1,
            UPDATE = 2,
            REMOVE = 3
        }
    }
}
