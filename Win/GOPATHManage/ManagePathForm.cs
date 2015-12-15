using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GOPATHLib;

namespace GOPATHManage
{
    public partial class ManagePathForm : Form
    {
        private Constants.KeyMethod _formMethod;
        public ManagePathForm(Constants.KeyMethod formMethod)
        {
            InitializeComponent();

            _formMethod = formMethod;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();

            if(result == System.Windows.Forms.DialogResult.OK)
                txtGoPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PathConfigManager pm = new PathConfigManager();
            switch(_formMethod)
            {
                case Constants.KeyMethod.ADD:
                    pm.AddGoPath(txtPathAlias.Text, txtGoPath.Text);
                    break;
                case Constants.KeyMethod.UPDATE:
                    pm.UpdateGoPath(txtPathAlias.Text, txtGoPath.Text);
                    break;
                default:
                    break;

            }

            this.Close();
        }

        private void ManagePathForm_Load(object sender, EventArgs e)
        {

        }
    }
}
