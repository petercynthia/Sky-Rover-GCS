using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKYROVER.GCS.DeskTop.Controls
{
    public delegate void OnClick(string man);
    public partial class BtnCommand : UserControl
    {
        public event OnClick OnClickEvent;

        public BtnCommand()
        {
            InitializeComponent();
        }

        public void SetLabelText(string lblText)
        {

            this.lblCommand.Text = lblText;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (OnClickEvent!=null) { OnClickEvent(this.lblCommand.Text); }
        }
    }
}
