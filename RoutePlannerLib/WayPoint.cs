using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.FormattableString;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class WayPoint
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public WayPoint(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            return  Invariant($"WayPoint: {(Name!=null?Name+" ":"")}{Latitude:0.00}/{Longitude:0.00}");
        }

        public double Distance(WayPoint target)
        {
            double myLat, myLong, tarLat, tarLong;
            var r = 6371;
            myLat = Latitude / 180 * Math.PI;
            myLong = Longitude / 180 * Math.PI;
            tarLat = target.Latitude / 180 * Math.PI;
            tarLong = target.Longitude / 180 * Math.PI;
            return (r*
                    Math.Acos(Math.Sin(myLat)*Math.Sin(tarLat) +
                              Math.Cos(myLat)*Math.Cos(tarLat)*Math.Cos(myLong - tarLong)));
        }
    }
}
