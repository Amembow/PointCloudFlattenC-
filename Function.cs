using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Projections;

namespace 練習
{
    class Function
    {

        public ProjectionInfo proj4326 = ProjectionInfo.FromEpsgCode(4326);　//WGS84

        public double[] ReprojectPoints(string x,string y,int sys)
        {
            var lat = double.Parse((x.ToString()));
            var lon = double.Parse(y.ToString());
            //Console.WriteLine(string.Format("deg lon={0}, lat={1}", lon, lat));

            var xy = ConvertLonLatToXY(lon, lat,sys);
            Console.WriteLine(string.Format("x={0}, y={1}", xy[0], xy[1]));
            return xy;
        }

        // 緯度経度→平面直角座標系
        public double[] ConvertLonLatToXY(double lon, double lat,int sys)
        {
            DotSpatial.Projections.ProjectionInfo proj = ProjectionInfo.FromEpsgCode(2443);
            switch (sys) 
            {
                case 1:
                    proj = ProjectionInfo.FromEpsgCode(2443); //1系
                    break;
                case 2:
                    proj = ProjectionInfo.FromEpsgCode(2444); //2系
                    break;
                case 3:
                    proj = ProjectionInfo.FromEpsgCode(2445); //3系
                    break;
                case 4:
                    proj = ProjectionInfo.FromEpsgCode(2446); //4系
                    break;
                case 5:
                    proj = ProjectionInfo.FromEpsgCode(2447); //5系
                    break;
                case 6:
                    proj = ProjectionInfo.FromEpsgCode(2448); //6系
                    break;
                case 7:
                    proj = ProjectionInfo.FromEpsgCode(2449); //7系
                    break;
                case 8:
                    proj = ProjectionInfo.FromEpsgCode(2450); //8系
                    break;
                case 9:
                    proj = ProjectionInfo.FromEpsgCode(2451); //9系
                    break;
                case 10:
                    proj = ProjectionInfo.FromEpsgCode(2452); //10系
                    break;
                case 11:
                    proj = ProjectionInfo.FromEpsgCode(2453); //11系
                    break;
                case 12:
                    proj = ProjectionInfo.FromEpsgCode(2454); //12系
                    break;
                case 13:
                    proj = ProjectionInfo.FromEpsgCode(2455); //13系
                    break;
                case 14:
                    proj = ProjectionInfo.FromEpsgCode(2456); //14系
                    break;
                case 15:
                    proj = ProjectionInfo.FromEpsgCode(2457); //15系
                    break;
                case 16:
                    proj = ProjectionInfo.FromEpsgCode(2458); //16系
                    break;
                case 17:
                    proj = ProjectionInfo.FromEpsgCode(2459); //17系
                    break;
                case 18:
                    proj = ProjectionInfo.FromEpsgCode(2460); //18系
                    break;
                case 19:
                    proj = ProjectionInfo.FromEpsgCode(2461); //19系
                    break;
            }


            var lonlat = new[] { lon, lat };
            Reproject.ReprojectPoints(lonlat, null, proj4326, proj, 0, 1);
            var ret = new[] { lonlat[1], lonlat[0] };
            return ret;
        }

        public double AffineY(double x1,double y1, double p) 
        {
            double angle = Math.PI * p / 180;
            var y = x1 * Math.Sin(angle) + y1 * Math.Cos(angle);

            return y;
        }
    }
}
