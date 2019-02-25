using MissionPlanner.Utilities;
using SKYROVER.GCS.DeskTop.Controls;
using SKYROVER.GCS.DeskTop.MessagePanel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKYROVER.GCS.DeskTop.MenuItems
{
    public delegate void MAVCommandsParameterChange(List<Locationwp> ctlHoverMAVs);
    /// <summary>
    /// 航点命令
    /// </summary>
    public partial class frmWayPointCommands : BaseForm
    {
        public event MAVCommandsParameterChange MAVCommandsParameterChangeEvent;
        
        List<Locationwp> mCtlHoverMAVs;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctlHoverMAVs"></param>
        public void Init(List<Locationwp> ctlHoverMAVs)
        {
            //解析航点上的命令
            foreach (Locationwp cmd in ctlHoverMAVs)
            {
                switch (cmd.id)
                {
                    //悬停
                    case 19:
                        CtlHoverMAV ctlHoverMAV = new CtlHoverMAV() { Width = this.commandContainer.Width - 6 };
                        ctlHoverMAV.SetDefaultValue(600,1,(int)cmd.p1);
                        ctlHoverMAV.AfterDeleteMAVEvent += Ctl_AfterDeleteMAVEvent;
                        this.commandContainer.Controls.Add(ctlHoverMAV);
                        break;
                    //投放
                    case 183:
                        CtlDoSetServoMAV ctlDoSetServoMAV = new CtlDoSetServoMAV() { Width = this.commandContainer.Width - 6 };
                        ctlDoSetServoMAV.SetParameters((int)cmd.p1, (int)cmd.p2);
                        ctlDoSetServoMAV.AfterDeleteMAVEvent += CtlDoSetServoMAV_AfterDeleteMAVEvent;
                        this.commandContainer.Controls.Add(ctlDoSetServoMAV);
                        break;
                    //拍照
                    case 203:
                        CtlDigitalCamMAV ctlDigitalCamMAV = new CtlDigitalCamMAV() { Width = this.commandContainer.Width - 6 };
                        ctlDigitalCamMAV.AfterDeleteMAVEvent += CtlDigitalCamMAV_AfterDeleteMAVEvent;
                        this.commandContainer.Controls.Add(ctlDigitalCamMAV);
                        break;
                    default:
                        break;

                }
                
            }



        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Locationwp> GetCtlHoverMAVs()
        {

            mCtlHoverMAVs = new List<Locationwp>();

            foreach (Control control in this.commandContainer.Controls)
            {
              mCtlHoverMAVs.Add((control as ILocationwp).Locationwp);
            }
            return mCtlHoverMAVs;
        }

        public frmWayPointCommands()
        {
            InitializeComponent();
            this.Load += FrmWayPointCommands_Load;
        }

        private void FrmWayPointCommands_Load(object sender, EventArgs e)
        {
            btnHoverMAV.SetLabelText("悬停");
            btnHoverMAV.OnClickEvent += BtnHoverMAV_OnClickEvent;           
            btnTriggerCAM.SetLabelText("拍照");
            btnTriggerCAM.OnClickEvent += BtnTriggerCAM_OnClickEvent;
            btnThrowMAV.SetLabelText("投掷");
            btnThrowMAV.OnClickEvent += BtnThrowMAV_OnClickEvent;
        }

       

        private void BtnThrowMAV_OnClickEvent(string man)
        {
            if (Settings.config["txtServoChanel1"] == null || Settings.config["txtSC1PWMOn"] == null)
            {
                MessageBox.Show("请检查配置信息！","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            CtlDoSetServoMAV ctlDoSetServoMAV = new CtlDoSetServoMAV() { Width = this.commandContainer.Width - 6 };
            

            ctlDoSetServoMAV.SetParameters(int.Parse(Settings.config["txtServoChanel1"]), int.Parse(Settings.config["txtSC1PWMOn"]));

            ctlDoSetServoMAV.AfterDeleteMAVEvent += CtlDoSetServoMAV_AfterDeleteMAVEvent;
            this.commandContainer.Controls.Add(ctlDoSetServoMAV);
        }

        private void CtlDoSetServoMAV_AfterDeleteMAVEvent(Control ctlDoSetServoMAV)
        {
            this.commandContainer.Controls.Remove(ctlDoSetServoMAV);
        }

        private void BtnTriggerCAM_OnClickEvent(string man)
        {
            CtlDigitalCamMAV ctlDigitalCamMAV = new CtlDigitalCamMAV() {Width=this.commandContainer.Width-6};
            ctlDigitalCamMAV.AfterDeleteMAVEvent += CtlDigitalCamMAV_AfterDeleteMAVEvent;
            this.commandContainer.Controls.Add(ctlDigitalCamMAV);
        }

        private void CtlDigitalCamMAV_AfterDeleteMAVEvent(Control ctlDigitalCamMAV)
        {
            this.commandContainer.Controls.Remove(ctlDigitalCamMAV);
        }

        private void BtnHoverMAV_OnClickEvent(string man)
        {
            CtlHoverMAV ctl = new CtlHoverMAV() { Width = this.commandContainer.Width - 6 };
            ctl.SetDefaultValue(600,1,3);
            ctl.AfterDeleteMAVEvent += Ctl_AfterDeleteMAVEvent;            
            this.commandContainer.Controls.Add(ctl);

        }

        private void Ctl_AfterDeleteMAVEvent(Control ctlHoverMAV)
        {
            this.commandContainer.Controls.Remove(ctlHoverMAV);
        }
               
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            List<Locationwp> cmds = GetCtlHoverMAVs();
            if (MAVCommandsParameterChangeEvent != null) MAVCommandsParameterChangeEvent(cmds);
            this.Close();
        }
    }

}
