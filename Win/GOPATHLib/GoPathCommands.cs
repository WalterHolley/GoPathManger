﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;

namespace GOPATHLib
{
    public class GoPathCommands
    {
        
        public GoPathCommands()
        {

        }

        public string GetCurrentGoPath()
        {

            //Check for root path to ensure Go is setup
            var rootPath = GetGoRoot();

            if (rootPath == Constants.ROOT_NOT_FOUND_ERROR)
                return Constants.ROOT_NOT_FOUND_ERROR;

            //Check Environment variable first
            string envPath = Environment.GetEnvironmentVariable("GOPATH");
            

            //Check Registry 
            if (string.IsNullOrEmpty(envPath))
            {
                var path = GetPathFromRegistry();

                if(path == Constants.PATH_NOT_FOUND_ERROR)
                    return Constants.PATH_NOT_FOUND_ERROR;
                
            }

            return envPath;
        }

        public void ChangeGoPath(string path)
        {
            //Make sure Go is available before changing path
            if(!GetGoRoot().Equals(Constants.ROOT_NOT_FOUND_ERROR))
            {
                var mainKey = Registry.LocalMachine.OpenSubKey(Constants.GO_REGISTRY_LOCATION, true);

                if (mainKey != null)
                    mainKey.SetValue(Constants.GOPATH_SYSTEM_VARIABLE, path);
            }
            
        }

        private string GetPathFromRegistry()
        {
            var mainKey = Registry.LocalMachine.OpenSubKey(Constants.GO_REGISTRY_LOCATION, false);

            if(mainKey != null)
                return mainKey.GetValue(Constants.GOPATH_SYSTEM_VARIABLE).ToString();

            return Constants.PATH_NOT_FOUND_ERROR;
        }

        private string GetGoRoot()
        {
            try
            {
                var mainKey = Registry.LocalMachine.OpenSubKey(Constants.GO_REGISTRY_LOCATION, false);
                return mainKey.GetValue(Constants.GOROOT_SYSTEM_VARIABLE).ToString();

                
            }
            catch(Exception ex)
            {
                return Constants.ROOT_NOT_FOUND_ERROR;
            }
            

            
        }

    }
}
