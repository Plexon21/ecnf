using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class City
    {
        public string Country { get;  set; }
        public WayPoint Location { get;  set; }
        public string Name { get;  set; }
        public int Population { get;  set; }

        public City()
        {
        }

        public City(string name, string country, int population, double latitude, double longitude)
        {
            Name = name;
            Country = country;
            Population = population;
            Location = new WayPoint(name, latitude, longitude);
        }

        public override bool Equals(object obj)
        {
            var c = (City) obj;
            return (c.Name.Equals(this.Name, StringComparison.InvariantCultureIgnoreCase) &&
                    c.Country.Equals(this.Country, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
