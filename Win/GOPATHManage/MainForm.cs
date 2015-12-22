using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using GOPATHLib;

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

            if (result == System.Windows.Forms.DialogResult.OK)
                UpdatePathList();
        }

        private void btnChangePath_Click(object sender, EventArgs e)
        {
            string selectedPath = ((KeyValuePair<string, string>)lbPaths.SelectedItem).Value;
            GoPathCommands cmd = new GoPathCommands();
            cmd.ChangeGoPath(selectedPath);
            UpdateCurrentPath();
        }

        private void lbPaths_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSelectedPathKey.Text = ((KeyValuePair<string, string>)lbPaths.SelectedItem).Key;
            lblSelectedPath.Text = lbPaths.SelectedValue.ToString();
        }

        private void UpdatePathList()
        {
            var paths = new PathConfigManager().GetPaths();

            lbPaths.DisplayMember = "Key";
            lbPaths.ValueMember = "Value";
            lbPaths.DataSource = paths;
            lbPaths.Refresh();

            
        }

        private void UpdateCurrentPath()
        {
            lblCurrentPath.Text = new GoPathCommands().GetCurrentGoPath();
            lblCurrentPath.Refresh();
        }

        private void FormInit()
        {
            PathConfigManager pm = new PathConfigManager();
            var paths = pm.GetPaths();

            if(paths.Count == 0)
            {
                GoPathCommands cmd = new GoPathCommands();

                string goPath = cmd.GetCurrentGoPath();
                switch(goPath)
                {
                    case GOPATHLib.Constants.PATH_NOT_FOUND_ERROR:
                         var result = MessageBox.Show("Go is available, but a path is not set.  Would you like to set a path?", "Set Go Path", MessageBoxButtons.YesNo);
                        if(result == System.Windows.Forms.DialogResult.Yes)
                        {
                            var addForm = new ManagePathForm(Constants.KeyMethod.ADD);
                            var formResult = addForm.ShowDialog(this);

                            if (formResult == System.Windows.Forms.DialogResult.Cancel)
                                Application.Exit();
                        }
                        break;
                    case GOPATHLib.Constants.ROOT_NOT_FOUND_ERROR:
                        MessageBox.Show("An installation of Go was not found on this machine.", "Go Not Found", MessageBoxButtons.OK);
                        Application.Exit();
                        break;
                    default:
                        pm.AddGoPath("Default", goPath);
                        break;
                }
                

                
                
            }
          

            UpdatePathList();
            UpdateCurrentPath();
            
        }

        
    }
}
