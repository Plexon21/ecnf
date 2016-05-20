using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        private static TraceSource Log = new TraceSource(nameof(Cities));
        private List<City> cities;
        public int Count
        {
            get
            {
                return cities.Count();
            }
        }


        public Cities()
        {
            cities = new List<City>();
        }

        public City this[int i]
        {
            get
            {
                if (i < 0 || i >= this.Count)
                {
                    throw new IndexOutOfRangeException("This message is not meaningdful");
                }
                return cities[i];
            }
            set { cities[i] = value; }
        }

        public City this[string cityName]
        {
            get
            {
                var city = cities.Find(c => c.Name.Equals(cityName, StringComparison.InvariantCultureIgnoreCase));
                if (city != null)
                {
                    return city;
                }
                else
                    throw new KeyNotFoundException();

            }
            set
            {
                var city = cities.Find(c => c.Name.Equals(cityName, StringComparison.InvariantCultureIgnoreCase));
                if (city != null)
                {
                    city = value;
                }
                else
                    throw new KeyNotFoundException();
            }
        }

        public int ReadCities(string filename)
        {
            try
            {
                using (TextReader reader = new StreamReader(filename))
                {
                    Log.TraceEvent(TraceEventType.Information, 3, "ReadCities started");
                    IEnumerable<string[]> citiesAsStrings = reader.GetSplittedLines('\t');

                    var countBefore = cities.Count();

                    cities.AddRange(citiesAsStrings.Select(line => new City(
                        name: line[0].Trim(),
                        country: line[1].Trim(),
                        population: int.Parse(line[2]),
                        latitude: double.Parse(line[3], CultureInfo.InvariantCulture),
                        longitude: double.Parse(line[4], CultureInfo.InvariantCulture)
                        )));
                    Log.TraceEvent(TraceEventType.Information, 4, "ReadCities ended");
                    return cities.Count() - countBefore;
                }
            }
            catch (FileNotFoundException e)
            {
                Log.TraceEvent(TraceEventType.Critical, 5, e.StackTrace);
                throw e;
            }

        }


        public IEnumerable<City> FindNeighbours(WayPoint location, double distance)
            => cities
               .Where(c => c.Location.Distance(location) <= distance)
               .OrderBy(c => c.Location.Distance(location));



    }
}
