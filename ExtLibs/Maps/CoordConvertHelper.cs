using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Maps
{
    public class CoordConvertHelper
    {
        static readonly double a = 6378137;
        static readonly double f = 1 / 298.257223563;
        static readonly double b = 6356752.3142;
        static readonly double e = Math.Sqrt(a * a - b * b) / a;
        static readonly double ee = 0.0066943800042608;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="B"></param>
        /// <param name="L"></param>
        /// <param name="H"></param>
        /// <returns></returns>
        public static double[] ConvertBLH2XYZ(double B, double L, double H)
        {

            double[] result = new double[3];

            double N = a / Math.Sqrt(1 - e * e * Math.Sin(B * Math.PI / 180) * Math.Sin(B * Math.PI / 180));

            result[0] = (N + H) * Math.Cos(B * Math.PI / 180) * Math.Cos(L * Math.PI / 180);
            result[1] = (N + H) * Math.Cos(B * Math.PI / 180) * Math.Sin(L * Math.PI / 180);
            result[2] = (N * (1 - (e * e)) + H) * Math.Sin(B * Math.PI / 180);

            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static double[] ConvertXYZ2BLH(double X, double Y, double Z)
        {

            double[] result = new double[3];

            double ee1 = (a * a - b * b) / b * b;
            double t = Math.Atan(Z * a / (Math.Sqrt(X * X + Y * Y) * b));

            result[0] = Math.Atan(Y / X) * 180 / Math.PI + 180;

            bool flag = true;
            double B0 = 0, N = 0;
            double e2 = 2 * f - f * f;
            double d = Math.Sqrt(X * X + Y * Y);
            while (flag)
            {
                N = a / Math.Sqrt(1 - e2 * Math.Sin(B0 * Math.Sin(B0)));
                result[1] = Math.Atan((Z + N * e2 * Math.Sin(B0)) / d);
                if (Math.Abs(B0 - result[1]) < 1e-10) flag = false;
                B0 = result[1];

            }


            result[2] = Math.Sqrt(X * X + Y * Y) / Math.Cos(result[1]) - N;

            result[1] = result[1] * 180 / Math.PI;


            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static double[] ConvertLngLat2WebMercator(double longitude, double latitude)
        {
            double[] result = new double[2];
            if (Math.Abs(longitude) <= 180 && Math.Abs(latitude) <= 90)
            {

                result[0] = longitude * 20037508.34 / 180;
                result[1] = Math.Log(Math.Tan((90 + latitude) * Math.PI / 360)) / (Math.PI / 180) * 20037508.34 / 180;
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double[] ConvertWebMercator2LngLat(double x, double y)
        {
            double[] result = new double[2];
            result[0] = x / 20037508.34 * 180;
            result[1] = y / 20037508.34 * 180;
            result[1] = 180 / Math.PI * (2 * Math.Atan(Math.Exp(result[1] * Math.PI / 180)) - Math.PI / 2);
            return result;
        }

    }

}
