using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MissionPlanner.Utilities;
using MissionPlanner.Controls;
using SKYROVER.GCS.DeskTop.Utilities;
using MissionPlanner;

namespace SKYROVER.GCS.DeskTop.MessagePanel
{
    public partial class ImportantMessagePanel : UserControl
    {

        public ImportantMessagePanel()
        {
            InitializeComponent();
            SetStyle(
                     ControlStyles.OptimizedDoubleBuffer
                    
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.SupportsTransparentBackColor
                     | ControlStyles.Opaque,
                     true);
        }

        // <summary>
        /// 自定义绘制窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //// 定义颜色的透明度
            Color drawColor = Color.FromArgb(30, this.BackColor);
            //// 定义画笔
            Pen labelBorderPen = new Pen(drawColor, 0);
            SolidBrush labelBackColorBrush = new SolidBrush(drawColor);
            //// 绘制背景色
            e.Graphics.DrawRectangle(labelBorderPen, 0, 0, Size.Width, Size.Height);
            e.Graphics.FillRectangle(labelBackColorBrush, 0, 0, Size.Width, Size.Height);

            base.OnPaint(e);
        }

        protected override CreateParams CreateParams
        {
            get {
                //var parms = base.CreateParams;
                //parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN  
                //return parms;
                CreateParams cp = base.CreateParams;
                // 开启 WS_EX_TRANSPARENT,使控件支持透明
                cp.ExStyle = 0x20 ;
                return cp;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingSourceQuickTab"></param>
        public void BindingDataSource(BindingSource bindingSourceQuickTab)
        {
            quickView1.DataBindings.Add(new System.Windows.Forms.Binding("number", bindingSourceQuickTab, "alt", true));
            quickView2.DataBindings.Add(new System.Windows.Forms.Binding("number", bindingSourceQuickTab, "groundspeed", true));
            quickView3.DataBindings.Add(new System.Windows.Forms.Binding("number", bindingSourceQuickTab, "wp_dist", true));
            quickView4.DataBindings.Add(new System.Windows.Forms.Binding("number", bindingSourceQuickTab, "yaw", true));
            quickView5.DataBindings.Add(new System.Windows.Forms.Binding("number", bindingSourceQuickTab, "verticalspeed", true));
            quickView6.DataBindings.Add(new System.Windows.Forms.Binding("number", bindingSourceQuickTab, "DistToHome", true));

        }
       
        #region 移動窗體


        private bool m_isDown = false;
        private System.Drawing.Point m_lastMousePosition;

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            m_isDown = true;
            m_lastMousePosition = new System.Drawing.Point(e.X, e.Y);

        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isDown)
            {

                int x = e.X - m_lastMousePosition.X;
                int y = e.Y - m_lastMousePosition.Y;

                this.Location = new System.Drawing.Point(this.Location.X + x, this.Location.Y + y);
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            m_isDown = false;
        }

        #endregion

    }
}
