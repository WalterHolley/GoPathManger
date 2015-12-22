using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;


namespace GOPATHLib
{
    public class PathConfigManager
    {
        
        private string configFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Constants.GOPATH_CONFIG_FILE;

        public PathConfigManager()
        {
            InitConfig();
        }

        
        /// <summary>
        /// Inserts a new Go Path into the config
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="directory"></param>
        public void AddGoPath(string alias, string directory)
        {
            XmlDocument doc = GetPathConfig();
            XmlNode node = doc.SelectSingleNode("//" + Constants.GOPATH_CONFIG_ELEMENT);

            XmlNodeList pathNodes = node.SelectNodes("//" + Constants.GOPATH_PATH_ELEMENT);

            //check to see if node with key currently exists
            if (pathNodes != null)
            {
                foreach (XmlNode n in pathNodes)
                {
                    if (n.Attributes["key"].Value == alias)
                    {
                        throw new Exception("A path with this alias already exists.");

                    }
                }
            }
          
            XmlElement newPath = doc.CreateElement(Constants.GOPATH_PATH_ELEMENT);
            newPath.SetAttribute("key", alias);
            newPath.SetAttribute("value", directory);

            node.AppendChild(newPath);
            doc.Save(configFilePath);

        }

        /// <summary>
        /// Removes Path from configuration file
        /// </summary>
        /// <param name="alias">key for path to be removed.</param>
        public void RemoveGoPath(string alias)
        {
            XmlDocument doc = GetPathConfig();
            XmlNode node = doc.SelectSingleNode("//" + Constants.GOPATH_CONFIG_ELEMENT);

            XmlNodeList pathNodes = node.SelectNodes("//" + Constants.GOPATH_PATH_ELEMENT);

            if(pathNodes != null)
            {
                foreach(XmlNode n in pathNodes)
                {
                    if(n.Attributes["key"].Value == alias)
                    {
                        doc.RemoveChild(n);
                        doc.Save(configFilePath);
                        break;
                    }
                }
            }
        }

        public void UpdateGoPath(string alias, string newDirectory)
        {
            XmlDocument doc = GetPathConfig();
            XmlNode node = doc.SelectSingleNode("//" + Constants.GOPATH_CONFIG_ELEMENT);
            XmlNodeList pathNodes = node.SelectNodes("//" + Constants.GOPATH_PATH_ELEMENT);

            if (pathNodes != null)
            {
                foreach (XmlNode n in pathNodes)
                {
                    if (n.Attributes["key"].Value == alias)
                    {
                        n.Attributes["value"].Value = newDirectory;
                        doc.Save(configFilePath);
                        break;
                    }
                }
            }
        }

        
        /// <summary>
        /// Updates the Key value for a path configuration
        /// </summary>
        /// <param name="currentKey">existing path key</param>
        /// <param name="newKey">new path key</param>
        public void UpdateGoPathKey(string currentKey, string newKey)
        {
            XmlDocument doc = GetPathConfig();
            XmlNode node = doc.SelectSingleNode("//" + Constants.GOPATH_CONFIG_ELEMENT);
            XmlNodeList pathNodes = node.SelectNodes("//" + Constants.GOPATH_PATH_ELEMENT);

            if(pathNodes != null)
            {
                foreach( XmlNode n in pathNodes)
                {
                    if (n.Attributes["key"].Value == currentKey)
                    {
                        n.Attributes["key"].Value = newKey;
                        doc.Save(configFilePath);
                        break;
                    }
                }
            }
        }

        private XmlNodeList GetGoPaths()
        {
            XmlDocument doc = GetPathConfig();
            var node = doc.SelectSingleNode("//" + Constants.GOPATH_CONFIG_ELEMENT);
            return node.SelectNodes("//" + Constants.GOPATH_PATH_ELEMENT);
        }

        /// <summary>
        /// Converts the xml node list to key/value pairs
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetPaths()
        {
            List<KeyValuePair<string, string>> paths = new List<KeyValuePair<string, string>>();
            var list = GetGoPaths();

            foreach (XmlNode n in list)
            {
                var path = new KeyValuePair<string, string>(n.Attributes["key"].Value, n.Attributes["value"].Value);
                paths.Add(path);
            }
            return paths;
        }

        private XmlDocument GetPathConfig()
        {
               
            XmlDocument doc = new XmlDocument();
            doc.Load(configFilePath);
            return doc;
            
        }

        private void InitConfig()
        {
            if (!File.Exists(configFilePath))
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(declaration, root);

                var goSection = doc.CreateElement(Constants.GOPATH_CONFIG_ELEMENT);
                doc.AppendChild(goSection);
                doc.Save(configFilePath);

            }
        }
    }
}
