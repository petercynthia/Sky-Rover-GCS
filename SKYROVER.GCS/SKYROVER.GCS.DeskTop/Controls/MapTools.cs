using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MissionPlanner.Controls;

namespace SKYROVER.GCS.DeskTop.Controls
{
    public partial class MapTools : UserControl
    {
        /// <summary>
        /// 地图控件
        /// </summary>
        private myGMAP mapControl;

        public MapTools()
        {
            InitializeComponent();
        }

        public myGMAP MapControl { get => mapControl; set => mapControl = value; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setHomeTool_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomInTool_Click(object sender, EventArgs e)
        {
            if ((mapControl.Zoom + 1) < mapControl.MaxZoom)
                mapControl.Zoom += 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomOutTool_Click(object sender, EventArgs e)
        {
            if ((mapControl.Zoom - 1) > mapControl.MinZoom)
                mapControl.Zoom -= 1;

        }
    }
}
