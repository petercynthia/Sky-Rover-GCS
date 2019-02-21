using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKYROVER.GCS.DeskTop.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        #region 移动窗体
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
