using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using GOPATHLib;


namespace GoPathManage.Tests

{
    [TestClass]
    public class GoPathLibTest
    {
        [TestMethod]
        public void GetCurrentPathTest()
        {
            //setup
            GoPathCommands cmds = new GoPathCommands();
            string expectedResult = GetPathFromRegistry();
                        

            //execute
            string result = cmds.GetCurrentGoPath();

            //assert
            Assert.AreEqual(expectedResult, result, true);

        }

        [TestMethod]
        public void ChangeGoPathTest()
        {
            //setup
            GoPathCommands cmd = new GoPathCommands();
            string currentPath = cmd.GetCurrentGoPath();
            string testGoPath = "C:\\GoPath";
            string expectedResult = null;

            if (currentPath == Constants.ROOT_NOT_FOUND_ERROR)
                expectedResult = Constants.ROOT_NOT_FOUND_ERROR;
            else
                expectedResult = testGoPath;
            //execute
            cmd.ChangeGoPath(testGoPath);

            //assert
            Assert.AreEqual(cmd.GetCurrentGoPath(), expectedResult);

            //cleanup
            if(!currentPath.Equals(Constants.ROOT_NOT_FOUND_ERROR))
                cmd.ChangeGoPath(currentPath);
        }

        private string GetPathFromRegistry()
        {
            //check for root path first
            try
            {
                var rootKey = Registry.LocalMachine.OpenSubKey(Constants.GO_REGISTRY_LOCATION, false);
                var result = rootKey.GetValue(Constants.GOROOT_SYSTEM_VARIABLE).ToString();
            }
            catch
            {
                return Constants.ROOT_NOT_FOUND_ERROR;
            }


            try
            {
                var mainKey = Registry.LocalMachine.OpenSubKey(Constants.GO_REGISTRY_LOCATION, false);
                return mainKey.GetValue(Constants.GOPATH_SYSTEM_VARIABLE).ToString();
            }
            catch
            {
                return Constants.PATH_NOT_FOUND_ERROR;
            }
            
        }
    }
}
