using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        private List<City> cities;
        public int Count { get; private set; }

        public Cities()
        {
            cities = new List<City>();
            Count = 0;
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
            using (TextReader reader = new StreamReader(filename))
            {
                var readCounter = 0;
                var line = reader.ReadLine();
                while (line != null)
                {
                    String[] values = line.Split('\t');
                    cities.Add(new City(values[0].Trim(), values[1].Trim(), int.Parse(values[2].Trim()), double.Parse(values[3].Trim()),
                        double.Parse(values[4].Trim())));
                    readCounter++;
                    line = reader.ReadLine();

                }
                Count += readCounter;
                return readCounter;
            }

        }


        public IEnumerable<City> FindNeighbours(WayPoint location, double distance)
        {
            return cities.Where(c => c.Location.Distance(location) <= distance).OrderBy(c => c.Location.Distance(location));
        }


    }
}
