using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYROVER.GCS.DeskTop.MapTools
{
   public class SetHome
    {
        public static void setHome()
        {

            if (MainUI.comPort == null) return;

            if (MainUI.comPort.MAV.cs.lat != 0 && MainUI.comPort.MAV.cs.lng != 0)
            {

                MainUI.comPort.MAV.cs.HomeLocation = MainUI.comPort.MAV.cs.Location;
                MainUI.MainUIInstance.getGMAP.Position = new GMap.NET.PointLatLng(MainUI.comPort.MAV.cs.Location.Lat, MainUI.comPort.MAV.cs.Location.Lng);
            }
        }
    }
}
