using GMap.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Maps
{
    /// <summary>
    /// 定义挂载的
    /// </summary>
  public  interface IPayload
    {
        #region 扩音器
        /// <summary>
        /// 绘制半径
        /// </summary>
        int MRadius { get; set; }
        /// <summary>
        /// 激活状态
        /// </summary>
        bool MMegaPhoneStatus { get; set; }
        /// <summary>
        /// 激活
        /// </summary>
        /// <returns></returns>
        void activeMegaPhone(bool active);
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="g"></param>
        void drawCircle(Graphics g);


        #endregion

        #region 吊仓
        /// <summary>
        /// 
        /// </summary>
        bool PodActived { get; set; }
        /// <summary>
        /// 吊舱相机焦距
        /// </summary>
        float PodFocus { get; set; }
        /// <summary>
        /// 吊舱相机对角线长度
        /// </summary>
        float CCDLength { get; set; }
        /// <summary>
        /// 相机CCD宽度
        /// </summary>
         float CCDWidth { get; set; }
        /// <summary>
        /// 相机CCD高度
        /// </summary>
        float CCDHight { get; set; }
        /// <summary>
        /// 吊舱中心位置
        /// </summary>
        PointLatLng centerPos { get; set; }
        /// <summary>
        /// 吊舱朝向
        /// </summary>
        float podHeading { get; set; }
        /// <summary>
        /// 吊舱俯仰角
        /// </summary>
        float podPitch { get; set; }
        /// <summary>
        /// 吊舱横滚角
        /// </summary>
        float podRoll { get; set; }
        /// <summary>
        /// 无人机飞行高度（相对地面）
        /// </summary>
        float UAVHight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        float UAVDist { get; set; }
       

        /// <summary>
        /// 绘制吊舱覆盖区域
        /// </summary>
        /// <param name="point"></param>
        /// <param name="UAVHight"></param>
        /// <param name="podPitch"></param>
        /// <param name="podHeading"></param>
        /// <param name="podRoll"></param>
        /// <param name="CCDWidth"></param>
        /// <param name="CCDHight"></param>
        /// <param name="PodFocus"></param>
        void DrawRegion(PointLatLng point, float UAVHight, float podPitch, float podHeading, float podRoll, float CCDWidth, float CCDHight, float PodFocus);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        void ActiveSinglePod(bool active);

        #endregion

        #region 投掷器

        #endregion

        

    }
}
