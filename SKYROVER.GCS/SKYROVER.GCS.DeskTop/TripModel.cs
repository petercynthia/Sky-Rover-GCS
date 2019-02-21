using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYROVER.GCS.DeskTop
{
  public  class TripModel
    {
        //航线类型
        TripType tripType;
        cameraInfo camera;
        double altitude;
        double resoluton;
        double speed;

        double overlap;
        double sidelap;
        int heading;

        string startFrom;

        public double Altitude { get => altitude; set => altitude = value; }
        public cameraInfo Camera { get => camera; set => camera = value; }
        public double Resoluton { get => resoluton; set => resoluton = value; }
        public double Speed { get => speed; set => speed = value; }
        public double Overlap { get => overlap; set => overlap = value; }
        public double Sidelap { get => sidelap; set => sidelap = value; }
        public int Heading { get => heading; set => heading = value; }
        public string StartFrom { get => startFrom; set => startFrom = value; }
        internal TripType TripType { get => tripType; set => tripType = value; }
    }
  public class cameraInfo
    {

        public string name;
        public float focallen;
        public float sensorwidth;
        public float sensorheight;
        public int imagewidth;
        public int imageheight;
    }
  public class DisplayInfos
    {

       public bool boundary;

       public  bool markers;

      public  bool grid;

      public  bool Internals;

      public  bool footPrints;


    }
    /// <summary>
    /// 航带信息
    /// </summary>
  public class StatsInfos {

        public bool boundary;

        public bool markers;

        public bool grid;

        public bool Internals;

        public bool footPrints;
    }

    enum TripType {

        Normal,
        Cross,
        Corridor
    }
    /// <summary>
    /// 
    /// </summary>
    public class TripResult
    {

        public string area;
        public string flyTime;
        public string pictureCount;
        public string trigerCamTime;
        public string tripCount;
        public string tripLength;
    }

}
