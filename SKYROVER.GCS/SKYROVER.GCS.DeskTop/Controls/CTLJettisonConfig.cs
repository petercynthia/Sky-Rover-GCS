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
    public delegate void SaveJettisonConfig(string txtServoChanell, string txtSC1PWMOn, string txtSC1PWMOff,
                                            string txtServoChanel2, string txtSC2PWMOn, string txtSC2PWMOff);
    public partial class CTLJettisonConfig : UserControl
    {
        public event SaveJettisonConfig SaveJettisonConfigEvent;

        public CTLJettisonConfig()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        public void InitControl(string txtServoChanel1, string txtSC1PWMOn, string txtSC1PWMOff,
                                            string txtServoChanel2, string txtSC2PWMOn, string txtSC2PWMOff) {

            this.txtServoChanel1.Text = txtServoChanel1;
            this.txtServoChanel2.Text = txtServoChanel2;
            this.txtSC1PWMOff.Text = txtSC1PWMOff;
            this.txtSC1PWMOn.Text = txtSC1PWMOn;
            this.txtSC2PWMOff.Text = txtSC2PWMOff;
            this.txtSC2PWMOn.Text = txtSC2PWMOn;

            this.SetControlEnable(false);

        }


        private void btnModify_Click(object sender, EventArgs e)
        {
            SetControlEnable(true);
            this.btnConfirm.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        private void SetControlEnable(bool enable) {
            this.txtServoChanel1.Enabled = enable;
            this.txtSC1PWMOn.Enabled = enable;
            this.txtSC1PWMOff.Enabled = enable;

            this.txtServoChanel2.Enabled = enable;
            this.txtSC2PWMOn.Enabled = enable;
            this.txtSC2PWMOff.Enabled = enable;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (SaveJettisonConfigEvent != null) {
                SaveJettisonConfigEvent(this.txtServoChanel1.Text,this.txtSC1PWMOn.Text,this.txtSC1PWMOff.Text,
                                        this.txtServoChanel2.Text,this.txtSC2PWMOn.Text,this.txtSC2PWMOff.Text);
            }
            SetControlEnable(false);
            this.btnConfirm.Enabled = false;
        }
    }
}
