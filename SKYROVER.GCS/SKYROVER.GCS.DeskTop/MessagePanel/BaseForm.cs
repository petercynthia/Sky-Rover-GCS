using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKYROVER.GCS.DeskTop.MessagePanel
{
    public partial class BaseForm : Form
    {

        /// <summary>
        /// 窗体动画函数    注意：要引用System.Runtime.InteropServices;
        /// </summary>
        /// <param name="hwnd">指定产生动画的窗口的句柄</param>
        /// <param name="dwTime">指定动画持续的时间</param>
        /// <param name="dwFlags">指定动画类型，可以是一个或多个标志的组合。</param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        //下面是可用的常量，根据不同的动画效果声明自己需要的
        private const int AW_HOR_POSITIVE = 0x0001;//自左向右显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        private const int AW_HOR_NEGATIVE = 0x0002;//自右向左显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        private const int AW_VER_POSITIVE = 0x0004;//自顶向下显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        private const int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志该标志
        private const int AW_CENTER = 0x0010;//若使用了AW_HIDE标志，则使窗口向内重叠；否则向外扩展
        private const int AW_HIDE = 0x10000;//隐藏窗口
        private const int AW_ACTIVE = 0x20000;//激活窗口，在使用了AW_HIDE标志后不要使用这个标志
        private const int AW_SLIDE = 0x40000;//使用滑动类型动画效果，默认为滚动动画类型，当使用AW_CENTER标志时，这个标志就被忽略
        private const int AW_BLEND = 0x80000;//使用淡入淡出效果

        public bool isShown = false;

        public BaseForm()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            if (isShown) AnimateWindow(this.Handle, 500, AW_SLIDE | AW_HIDE | AW_HOR_POSITIVE);
            else AnimateWindow(this.Handle, 500, AW_SLIDE | AW_ACTIVE | AW_HOR_NEGATIVE);
            isShown = !isShown;
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
