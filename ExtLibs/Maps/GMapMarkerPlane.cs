using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using MissionPlanner.Utilities;

namespace MissionPlanner.Maps
{

    [Serializable]
    public class GMapMarkerPlane : GMapMarker,IPayload
    {

        #region 喊话器
        /// <summary>
        /// 定时器，设置透明度变化的频率
        /// </summary>
        Timer timer;
        /// <summary>
        /// 透明度
        /// </summary>
        private static int Alpha = 0;       
        /// <summary>
        /// specifies how the outline is painted
        /// </summary>
        [NonSerialized]
        public Pen pen = new Pen(Color.FromArgb(Alpha, Color.Red));

        public PathGradientBrush brush;
             

        int mRadius;
        /// <summary>
        /// 绘制半径长度
        /// </summary>
        public int MRadius { get => mRadius; set => mRadius = value; }
        /// <summary>
        /// 激活喊话器
        /// </summary>
        /// <param name="active"></param>
        public void activeMegaPhone(bool active)
        {

            mMegaPhoneStatus = active;


          // Console.WriteLine("OnActiveMegaPhone: mMegaPhoneStatus={0},mRadius={1}", mMegaPhoneStatus.ToString(), mRadius.ToString());

            if (mMegaPhoneStatus)
            {
                if (timer == null)
                {
                    timer = new Timer();
                    timer.Interval = 200;
                    timer.Enabled = true;
                    timer.Tick += Timer_Tick;
                    timer.Start();

                }
            }
            else
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Enabled = false;
                    timer = null;
                    Overlay.Control.Refresh();
                }

            }

        }

        bool mMegaPhoneStatus=false;
        /// <summary>
        /// 标识喊话器的状态
        /// </summary>
        public bool MMegaPhoneStatus { get => mMegaPhoneStatus; set => mMegaPhoneStatus = value; }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Alpha > 140) Alpha = 20;
            else Alpha += 20;
           // Console.WriteLine("OnTick: mMegaPhoneStatus={0},mRadius={1}", mMegaPhoneStatus.ToString(), mRadius.ToString());
            Overlay.Control.Refresh();
            
        }

       
        /// <summary>
        /// 绘制范围线
        /// </summary>
        /// <param name="g"></param>
        private void DrawCircle(Graphics g)
        {

            int tempR = (int)((mRadius) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat)) * 2;

            if (mMegaPhoneStatus)
            {
                GraphicsPath graphicsPath = new GraphicsPath();
                graphicsPath.AddEllipse(new System.Drawing.Rectangle(-tempR / 2, -tempR / 2, tempR, tempR));
                brush = new PathGradientBrush(graphicsPath);
                brush.CenterColor = Color.FromArgb(Alpha, Color.Red);
                brush.SurroundColors = new Color[] {Color.FromArgb(Alpha/2, Color.White)};
                g.FillPath(brush, graphicsPath);               
                g.DrawPath(pen, graphicsPath);
               
            }

        }

        #endregion

        #region 吊舱
        /// <summary>
        /// 
        /// </summary>
        public bool PodActived { get; set; }
        /// <summary>
        /// 吊舱相机焦距
        /// </summary>
        public float PodFocus { get; set; }
        /// <summary>
        /// 吊舱相机对角线长度
        /// </summary>
        public float CCDLength { get; set; }
        /// <summary>
        /// 相机CCD宽度
        /// </summary>
        private float mCCDWidth = 4.57143f;
        /// <summary>
        /// 相机CCD高度
        /// </summary>
        private float mCCDHight = 3.42857f;
        /// <summary>
        /// 吊舱中心位置
        /// </summary>
        public PointLatLng centerPos { get; set; }
        /// <summary>
        /// 吊舱朝向
        /// </summary>
        public float podHeading { get; set; }
        /// <summary>
        /// 吊舱俯仰角
        /// </summary>
        public float podPitch { get; set; }


              
        /// <summary>
        /// 吊舱横滚角
        /// </summary>
        public float podRoll { get; set; }
        /// <summary>
        /// 无人机飞行高度（相对地面）
        /// </summary>
        public float UAVHight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float UAVDist { get; set; }

     
        public float CCDWidth { get=>mCCDWidth; set => mCCDWidth = value; }
        public float CCDHight { get => mCCDHight; set => mCCDHight = value; }


        private double alpha = 0;

        private double beta = 0;

        /// <summary>
        /// 根据CCD尺寸及焦距计算角度
        /// </summary>
        /// <param name="CCDWidth"></param>
        /// <param name="CCDHight"></param>
        /// <param name="PodFocus"></param>
        private void CalcuatePodKeyAngle(float CCDWidth, float CCDHight, float PodFocus)
        {

            alpha = Math.Atan(CCDHight / (2 * PodFocus));
            beta = Math.Atan(CCDWidth / Math.Sqrt(CCDHight * CCDHight + 4 * PodFocus * PodFocus));
        }

        GMapPolygon gMapPolygon;


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
        public void DrawRegion(PointLatLng point, float UAVHight, float podPitch, float podHeading, float podRoll, float CCDWidth, float CCDHight, float PodFocus)
        {
            #region 吊舱参数赋值


            //如果俯仰角小于0,无人机相对高度不为正，无法结算，直接返回
            if (podPitch < 0 || UAVHight <= 0) return;

            this.centerPos = point;
            this.UAVHight = UAVHight;
            this.podPitch = podPitch;
            //吊舱俯仰角转换
            this.podPitch = 90 - podPitch;

            //吊舱自身方位角，如果小于0，那么需要加360.
            this.podHeading = (podHeading < 0 ? 360 + podHeading : podHeading);
            //吊舱方位角加飞机方位角
            this.podHeading += heading;
            //如果最终角度大于360度，则减去360度
            if (this.podHeading > 360) this.podHeading -= 360;
            this.podRoll = podRoll;
            this.CCDHight = CCDHight;
            this.CCDWidth = CCDWidth;
            this.PodFocus = PodFocus;
            #endregion

            #region 投影参数计算

            //根据CCD、焦距计算角度
            CalcuatePodKeyAngle(this.CCDWidth, this.CCDHight, this.PodFocus);

            this.podPitch = (float)(this.podPitch / 180 * Math.PI);
            this.podHeading = (float)((this.podHeading) / 180 * Math.PI);
            this.podRoll = (float)((this.podRoll) / 180 * Math.PI);
            //地理坐标转换为投影坐标          
            double[] xy = CoordConvertHelper.ConvertLngLat2WebMercator((double)centerPos.Lng, (double)centerPos.Lat);

            double deltX1 = UAVHight * Math.Atan(beta) / Math.Cos(this.podPitch - alpha);
            double deltX2 = UAVHight * Math.Atan(beta) / Math.Cos(this.podPitch + alpha);

            double deltY1 = UAVHight * Math.Tan(this.podPitch - alpha);
            double deltY2 = UAVHight * Math.Tan(this.podPitch + alpha);

            double x = xy[0];
            double y = xy[1];

            double px1 = x + deltX1 * Math.Cos(this.podHeading) + deltY1 * Math.Sin(this.podHeading);
            double py1 = y + deltY1 * Math.Cos(this.podHeading) - deltX1 * Math.Sin(this.podHeading);

            double px2 = x + deltX2 * Math.Cos(this.podHeading) + deltY2 * Math.Sin(this.podHeading);
            double py2 = y + deltY2 * Math.Cos(this.podHeading) - deltX2 * Math.Sin(this.podHeading);

            double px3 = x - deltX2 * Math.Cos(this.podHeading) + deltY2 * Math.Sin(this.podHeading);
            double py3 = y + deltY2 * Math.Cos(this.podHeading) + deltX2 * Math.Sin(this.podHeading);

            double px4 = x - deltX1 * Math.Cos(this.podHeading) + deltY1 * Math.Sin(this.podHeading);
            double py4 = y + deltY1 * Math.Cos(this.podHeading) + deltX1 * Math.Sin(this.podHeading);

            double centerX = x + UAVHight * Math.Tan(this.podPitch) * Math.Sin(this.podHeading);
            double centerY = y + UAVHight * Math.Tan(this.podPitch) * Math.Cos(this.podHeading);

            px1 = (px1 - centerX) * Math.Cos(this.podRoll) + (py1 - centerY) * Math.Sin(this.podRoll) + centerX;
            py1 = (py1 - centerY) * Math.Cos(this.podRoll) - (px1 - centerX) * Math.Sin(this.podRoll) + centerY;

            px2 = (px2 - centerX) * Math.Cos(this.podRoll) + (py2 - centerY) * Math.Sin(this.podRoll) + centerX;
            py2 = (py2 - centerY) * Math.Cos(this.podRoll) - (px2 - centerX) * Math.Sin(this.podRoll) + centerY;

            px3 = (px3 - centerX) * Math.Cos(this.podRoll) + (py3 - centerY) * Math.Sin(this.podRoll) + centerX;
            py3 = (py3 - centerY) * Math.Cos(this.podRoll) - (px3 - centerX) * Math.Sin(this.podRoll) + centerY;

            px4 = (px4 - centerX) * Math.Cos(this.podRoll) + (py4 - centerY) * Math.Sin(this.podRoll) + centerX;
            py4 = (py4 - centerY) * Math.Cos(this.podRoll) - (px4 - centerX) * Math.Sin(this.podRoll) + centerY;

            //投影坐标转换为地理坐标
            double[] point1 = CoordConvertHelper.ConvertWebMercator2LngLat(px1, py1);// CoordConvertHelper.ConvertXYZ2BLH(px1, py1, xyz[2]-UAVHight);
            PointLatLng p1 = new PointLatLng(point1[1], point1[0]);
            double[] point2 = CoordConvertHelper.ConvertWebMercator2LngLat(px2, py2);// CoordConvertHelper.ConvertXYZ2BLH(px2, py2, xyz[2] - UAVHight);
            PointLatLng p2 = new PointLatLng(point2[1], point2[0]);
            double[] point3 = CoordConvertHelper.ConvertWebMercator2LngLat(px3, py3);// CoordConvertHelper.ConvertXYZ2BLH(px3, py3, xyz[2] - UAVHight);
            PointLatLng p3 = new PointLatLng(point3[1], point3[0]);
            double[] point4 = CoordConvertHelper.ConvertWebMercator2LngLat(px4, py4);// CoordConvertHelper.ConvertXYZ2BLH(px4, py4, xyz[2] - UAVHight);
            PointLatLng p4 = new PointLatLng(point4[1], point4[0]);

            #endregion

            #region 图形更新渲染

            if (gMapPolygon == null)
            {
                gMapPolygon = new GMapPolygon(new System.Collections.Generic.List<PointLatLng>(), "SinglePodFOVPolyon");
                gMapPolygon.Points.Add(p1);
                gMapPolygon.Points.Add(p2);
                gMapPolygon.Points.Add(p3);
                gMapPolygon.Points.Add(p4);
                gMapPolygon.Stroke = new Pen(Color.Red, 2);
                gMapPolygon.Fill = Brushes.Transparent;

            }
            else
            {
                gMapPolygon.Points[0] = p1;
                gMapPolygon.Points[1] = p2;
                gMapPolygon.Points[2] = p3;
                gMapPolygon.Points[3] = p4;
            }

            if (!Overlay.Polygons.Contains(gMapPolygon)) Overlay.Polygons.Add(gMapPolygon);

            Overlay.Control.UpdatePolygonLocalPosition(gMapPolygon);
            Overlay.Control.Refresh();

            #endregion

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        public void ActiveSinglePod(bool active)
        {

            PodActived = active;
            if (PodActived)
            {
                DrawRegion(this.Position, UAVHight, podPitch, podHeading, podRoll, CCDWidth, CCDHight, PodFocus);

            }
            else
            {

                if (gMapPolygon != null && Overlay.Polygons.Contains(gMapPolygon))
                {

                    Overlay.Polygons.Remove(gMapPolygon);
                }
            }
        }

        #endregion



        private readonly Bitmap icon = global::MissionPlanner.Maps.Resources.planeicon;



        float heading = 0;
        float cog = -1;
        float target = -1;
        float nav_bearing = -1;
        float radius = -1;

        public GMapMarkerPlane(PointLatLng p, float heading, float cog, float nav_bearing, float target, float radius)
            : base(p)
        {
            this.heading = heading;
            this.cog = cog;
            this.target = target;
            this.nav_bearing = nav_bearing;
            this.radius = radius;
            Size = icon.Size;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="heading"></param>
        /// <param name="cog"></param>
        /// <param name="target"></param>
        /// <param name="sysid"></param>
        public void updateMarkerValue(GMapMarkerPlane gMapMarkerPlane)
        {
            this.Position = gMapMarkerPlane.Position;
            this.heading = gMapMarkerPlane.heading;
            this.cog = gMapMarkerPlane.cog;
            this.target = gMapMarkerPlane.target;
            this.nav_bearing = gMapMarkerPlane.nav_bearing;
            this.radius = gMapMarkerPlane.radius;
            Size = icon.Size;

        }


        public override void OnRender(Graphics g)
        {
            var temp = g.Transform;
            g.TranslateTransform(LocalPosition.X, LocalPosition.Y);

            g.RotateTransform(-Overlay.Control.Bearing);

            int length = 500;
            // anti NaN
            try
            {
                g.DrawLine(new Pen(Color.Red, 2), 0.0f, 0.0f, (float)Math.Cos((heading - 90) * MathHelper.deg2rad) * length,
                    (float)Math.Sin((heading - 90) * MathHelper.deg2rad) * length);
            }
            catch
            {
            }
            //draw megaphone effect area
            if (mMegaPhoneStatus && mRadius > 0)
            {

                DrawCircle(g);
            }

            if (PodActived) DrawRegion(this.Position, this.UAVHight, this.podPitch, this.podHeading, this.podRoll, this.mCCDWidth, this.mCCDHight, this.PodFocus);

            g.DrawLine(new Pen(Color.Green, 2), 0.0f, 0.0f, (float)Math.Cos((nav_bearing - 90) * MathHelper.deg2rad) * length,
                (float)Math.Sin((nav_bearing - 90) * MathHelper.deg2rad) * length);
            g.DrawLine(new Pen(Color.Black, 2), 0.0f, 0.0f, (float)Math.Cos((cog - 90) * MathHelper.deg2rad) * length,
                (float)Math.Sin((cog - 90) * MathHelper.deg2rad) * length);
            g.DrawLine(new Pen(Color.Orange, 2), 0.0f, 0.0f, (float)Math.Cos((target - 90) * MathHelper.deg2rad) * length,
                (float)Math.Sin((target - 90) * MathHelper.deg2rad) * length);
            // anti NaN
            try
            {
                float desired_lead_dist = 100;

                double width =
                (Overlay.Control.MapProvider.Projection.GetDistance(Overlay.Control.FromLocalToLatLng(0, 0),
                     Overlay.Control.FromLocalToLatLng(Overlay.Control.Width, 0)) * 1000.0);
                double m2pixelwidth = Overlay.Control.Width / width;

                float alpha = (float)(((desired_lead_dist * (float)m2pixelwidth) / radius) * MathHelper.rad2deg);

                var scaledradius = radius * (float)m2pixelwidth;

                if (radius < -1 && alpha < -1)
                {
                    // fixme 

                    float p1 = (float)Math.Cos((cog) * MathHelper.deg2rad) * scaledradius + scaledradius;

                    float p2 = (float)Math.Sin((cog) * MathHelper.deg2rad) * scaledradius + scaledradius;

                    g.DrawArc(new Pen(Color.HotPink, 2), p1, p2, Math.Abs(scaledradius) * 2, Math.Abs(scaledradius) * 2, cog, alpha);
                }

                else if (radius > 1 && alpha > 1)
                {
                    // correct

                    float p1 = (float)Math.Cos((cog - 180) * MathHelper.deg2rad) * scaledradius + scaledradius;

                    float p2 = (float)Math.Sin((cog - 180) * MathHelper.deg2rad) * scaledradius + scaledradius;

                    g.DrawArc(new Pen(Color.HotPink, 2), -p1, -p2, scaledradius * 2, scaledradius * 2, cog - 180, alpha);
                }
            }
            catch
            {
            }

            try
            {
                g.RotateTransform(heading);
            }
            catch
            {
            }
            g.DrawImageUnscaled(icon, icon.Width / -2, icon.Height / -2);

            g.Transform = temp;
        }

        public bool activeMegaPhone()
        {
            throw new NotImplementedException();
        }

        public void drawCircle(Graphics g)
        {
            throw new NotImplementedException();
        }
    }

    /*
    [Serializable]
    public class GMapMarkerPlane : GMapMarker
    {
        private readonly Bitmap icon = global::MissionPlanner.Maps.Resources.planeicon;

        float heading = 0;
        float cog = -1;
        float target = -1;
        float nav_bearing = -1;
        float radius = -1;

        public GMapMarkerPlane(PointLatLng p, float heading, float cog, float nav_bearing, float target, float radius)
            : base(p)
        {
            this.heading = heading;
            this.cog = cog;
            this.target = target;
            this.nav_bearing = nav_bearing;
            this.radius = radius;
            Size = icon.Size;
        }

        public override void OnRender(Graphics g)
        {
            var temp = g.Transform;
            g.TranslateTransform(LocalPosition.X, LocalPosition.Y);

            g.RotateTransform(-Overlay.Control.Bearing);

            int length = 500;
            // anti NaN
            try
            {
                g.DrawLine(new Pen(Color.Red, 2), 0.0f, 0.0f, (float) Math.Cos((heading - 90)*MathHelper.deg2rad)*length,
                    (float) Math.Sin((heading - 90)*MathHelper.deg2rad)*length);
            }
            catch
            {
            }
            g.DrawLine(new Pen(Color.Green, 2), 0.0f, 0.0f, (float) Math.Cos((nav_bearing - 90)*MathHelper.deg2rad)*length,
                (float) Math.Sin((nav_bearing - 90)*MathHelper.deg2rad)*length);
            g.DrawLine(new Pen(Color.Black, 2), 0.0f, 0.0f, (float) Math.Cos((cog - 90)*MathHelper.deg2rad)*length,
                (float) Math.Sin((cog - 90)*MathHelper.deg2rad)*length);
            g.DrawLine(new Pen(Color.Orange, 2), 0.0f, 0.0f, (float) Math.Cos((target - 90)*MathHelper.deg2rad)*length,
                (float) Math.Sin((target - 90)*MathHelper.deg2rad)*length);
            // anti NaN
            try
            {
                float desired_lead_dist = 100;

                double width =
                (Overlay.Control.MapProvider.Projection.GetDistance(Overlay.Control.FromLocalToLatLng(0, 0),
                     Overlay.Control.FromLocalToLatLng(Overlay.Control.Width, 0))*1000.0);
                double m2pixelwidth = Overlay.Control.Width/width;

                float alpha = (float)(((desired_lead_dist * (float)m2pixelwidth) / radius) * MathHelper.rad2deg);

                var scaledradius = radius * (float)m2pixelwidth;

                if (radius < -1 && alpha < -1)
                {
                    // fixme 

                    float p1 = (float)Math.Cos((cog) * MathHelper.deg2rad) * scaledradius + scaledradius;

                    float p2 = (float)Math.Sin((cog) * MathHelper.deg2rad) * scaledradius + scaledradius;

                    g.DrawArc(new Pen(Color.HotPink, 2), p1, p2, Math.Abs(scaledradius) * 2, Math.Abs(scaledradius) * 2, cog, alpha);
                }

                else if (radius > 1 && alpha > 1)
                {
                    // correct

                    float p1 = (float)Math.Cos((cog - 180) * MathHelper.deg2rad) * scaledradius + scaledradius;

                    float p2 = (float)Math.Sin((cog - 180) * MathHelper.deg2rad) * scaledradius + scaledradius;

                    g.DrawArc(new Pen(Color.HotPink, 2), -p1, -p2, scaledradius * 2, scaledradius * 2, cog - 180, alpha);
                }
            }
            catch
            {
            }

            try
            {
                g.RotateTransform(heading);
            }
            catch
            {
            }
            g.DrawImageUnscaled(icon, icon.Width/-2, icon.Height/-2);

            g.Transform = temp;
        }
    }
     
    */

}