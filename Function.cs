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

        public ProjectionInfo proj4326 = ProjectionInfo.FromEpsgCode(4326);
        public ProjectionInfo proj2446 = ProjectionInfo.FromEpsgCode(2446);

        public double[] ReprojectPoints(string x,string y)
        {
            var lat = double.Parse((y.ToString()));
            var lon = double.Parse(x.ToString());
            Console.WriteLine(string.Format("deg lon={0}, lat={1}", lon, lat));

            var xy = ConvertLonLatToXY(lon, lat);
            Console.WriteLine(string.Format("x={0}, y={1}", xy[0], xy[1]));
            return xy;
        }

        // 緯度経度→平面直角座標系
        public double[] ConvertLonLatToXY(double lon, double lat)
        {
            var lonlat = new[] { lon, lat };
            Reproject.ReprojectPoints(lonlat, null, proj4326, proj2446, 0, 1);
            var ret = new[] { lonlat[1], lonlat[0] };
            return ret;
        }
    }
}
