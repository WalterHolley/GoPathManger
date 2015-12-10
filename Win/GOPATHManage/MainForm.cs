using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace GOPATHManage
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
           
            
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            FormInit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ManagePathForm mpForm = new ManagePathForm(Constants.KeyMethod.ADD);
            DialogResult result = mpForm.ShowDialog(this);
        }

        private void btnChangePath_Click(object sender, EventArgs e)
        {
            string selectedPath = ((KeyValuePair<string, string>)lbPaths.SelectedItem).Value;
            GoPathCommands cmd = new GoPathCommands();
            cmd.ChangeGoPath(selectedPath);
        }

        private void UpdatePathList()
        {
            var paths = new PathConfigManager().GetGoPaths();

            lbPaths.DisplayMember = "Key";
            lbPaths.ValueMember = "Value";
            lbPaths.DataSource = ConvertPaths(paths);
            

            

            lbPaths.Refresh();

        }

        private void FormInit()
        {
            PathConfigManager pm = new PathConfigManager();
            var paths = pm.GetGoPaths();

            if(paths.Count == 0)
            {
                GoPathCommands cmd = new GoPathCommands();

                string goPath = cmd.GetCurrentGoPath();
                switch(goPath)
                {
                    case Constants.PATH_NOT_FOUND_ERROR:
                         var result = MessageBox.Show("Go is available, but a path is not set.  Would you like to set a path?", "Set Go Path", MessageBoxButtons.YesNo);
                        if(result == System.Windows.Forms.DialogResult.Yes)
                        {
                            var addForm = new ManagePathForm(Constants.KeyMethod.ADD);
                            var formResult = addForm.ShowDialog(this);

                            if (formResult == System.Windows.Forms.DialogResult.Cancel)
                                Application.Exit();
                        }
                        break;
                    case Constants.ROOT_NOT_FOUND_ERROR:
                        MessageBox.Show("An installation of Go was not found on this machine.", "Go Not Found", MessageBoxButtons.OK);
                        Application.Exit();
                        break;
                    default:
                        pm.AddGoPath("Default", goPath);
                        break;
                }
                

                
                
            }
          

            UpdatePathList();
            
        }

        /// <summary>
        /// Converts the xml node list to a key/value dictionary for binding
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> ConvertPaths(System.Xml.XmlNodeList list)
        {
            List<KeyValuePair<string, string>> paths = new List<KeyValuePair<string, string>>();
            
            foreach(XmlNode n in list)
            {
                var path = new KeyValuePair<string,string>(n.Attributes["key"].Value, n.Attributes["value"].Value);
                paths.Add(path);
            }
            return paths;
        }

        
    }
}
