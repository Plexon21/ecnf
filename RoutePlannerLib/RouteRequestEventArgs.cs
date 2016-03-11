using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestEventArgs : EventArgs
    {
        public TransportMode TransportMode { get; private set; }
        public City FromCity { get; private set; }
        public City ToCity { get; private set; }


        public RouteRequestEventArgs(City fromCity, City toCity, TransportMode transportMode)
        {
            FromCity = fromCity;
            ToCity = toCity;
            TransportMode = transportMode;
        }
    }
}
