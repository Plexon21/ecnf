using System;
using System.Collections.Concurrent;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    ///	<summary>
    ///	Manages	a routes from a	city to	another	city.
    ///	</summary>
    public class Routes : IRoutes
    {
        private static TraceSource Log = new TraceSource(nameof(Routes));
        private List<Link> routes = new List<Link>();
        private Cities cities;
        public delegate void RouteRequestHandler(object sender, RouteRequestEventArgs e);
        public event RouteRequestHandler RouteRequested;

        public int Count
        {
            get
            {
                return routes.Count;
            }
        }

        ///	<summary>
        ///	Initializes	the	Routes with	the	cities.
        ///	</summary>
        ///	<param name="cities"></param>
        public Routes(Cities _cities)
        {
            cities = _cities;
        }

        public Routes()
        {
        }


        ///	<summary>
        ///	Reads a	list of	links from the given file.
        ///	Reads only links where the cities exist.
        ///	</summary>
        ///	<param name="filename">name	of links file</param>
        ///	<returns>number	of read	route</returns>
        public int ReadRoutes(string _filename)
        {
            Log.TraceEvent(TraceEventType.Information, 1, "ReadRoutes started");
            var previousCount = Count;
            using (var reader = new StreamReader(_filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var tokens = line.Split('\t');
                    try
                    {
                        var city1 = cities[tokens[0]];
                        var city2 = cities[tokens[1]];

                        if (city1 != null && city2 != null)
                            routes.Add(new Link(city1, city2, city1.Location.Distance(city2.Location),
                                TransportMode.Rail));
                    }
                    catch (KeyNotFoundException e)
                    {
                    }
                }
            }
            Log.TraceEvent(TraceEventType.Information, 2, "ReadRoutes ended");

            return Count - previousCount;
        }

        public City[] FindCities(TransportMode transportMode)
        {
            return routes.Where(r => r.TransportMode == transportMode)
                    .Select(r => r.FromCity)
                    .Concat(routes.Where(r => r.TransportMode == transportMode).Select(r => r.ToCity))
                    .Distinct().ToArray();
        }


        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            return FindShortestRouteBetween(fromCity, toCity, mode, null);
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode, IProgress<string> reportProgress)
        {
            RouteRequested?.Invoke(this, new RouteRequestEventArgs(cities[fromCity], cities[toCity], mode));

            //use dijkstra's algorithm to look for all single-source shortest paths
            var visited = new Dictionary<City, DijkstraNode>();
            var pending = new SortedSet<DijkstraNode>(new DijkstraNode[]
            {
                new DijkstraNode()
                {
                    VisitingCity = cities[fromCity],
                    Distance = 0
                }
            });
            reportProgress?.Report("Initialization done");

            while (pending.Any())
            {
                var cur = pending.Last();
                pending.Remove(cur);

                if (!visited.ContainsKey(cur.VisitingCity))
                {
                    visited[cur.VisitingCity] = cur;

                    foreach (var link in GetListOfAllOutgoingRoutes(cur.VisitingCity, mode))
                        pending.Add(new DijkstraNode()
                        {
                            VisitingCity = (link.FromCity == cur.VisitingCity) ? link.ToCity : link.FromCity,
                            Distance = cur.Distance + link.Distance,
                            PreviousCity = cur.VisitingCity
                        });
                }
            }
            reportProgress?.Report("Route calculation done");

            //did we find any route?
            if (!visited.ContainsKey(cities[toCity]))
                return null;
            reportProgress?.Report("Check if route found done");

            //create a list of cities that we passed along the way
            var citiesEnRoute = new List<City>();
            for (var c = cities[toCity]; c != null; c = visited[c].PreviousCity)
                citiesEnRoute.Add(c);
            reportProgress?.Report("All cities passed added to list done");
            citiesEnRoute.Reverse();
            reportProgress?.Report("List reverse done");

            //convert that city-list into a list of links
            IEnumerable<Link> paths = ConvertListOfCitiesToListOfLinks(citiesEnRoute, mode);
            reportProgress?.Report("Convert cities to links done");
            return paths.ToList();
        }

        public List<List<Link>> FindAllShortestRoutes()
        {
            var routes = new List<List<Link>>();
            for (int i = 0; i < cities.Count; i++)
            {
                for (int j = 0; j < cities.Count; j++)
                {
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Ship));
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Rail));
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Flight));
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Car));
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Bus));
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Tram));
                }
            }
            return routes;
        }
        public List<List<Link>> FindAllShortestRoutesParallel()
        {
            var routes = new ConcurrentBag<List<Link>>();
            Parallel.For(0, cities.Count, i =>
            {
                for (int j = 0; j < cities.Count; j++)
                {
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Ship));
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Rail));
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Flight));
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Car));
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Bus));
                    routes.Add(FindShortestRouteBetween(cities[i].Name, cities[j].Name, TransportMode.Tram));
                }

            });
            return routes.ToList();
        }

        public async Task<List<Link>> FindShortestRouteBetweenAsync(string fromCity, string toCity, TransportMode mode)
        {
            return await Task.Run(() => FindShortestRouteBetween(fromCity, toCity, mode));
        }
        public async Task<List<Link>> FindShortestRouteBetweenAsync(string fromCity, string toCity, TransportMode mode, IProgress<string> reportProgress)
        {
            return await Task.Run(() => FindShortestRouteBetween(fromCity, toCity, mode, reportProgress));
        }

        public IEnumerable<Link> ConvertListOfCitiesToListOfLinks(List<City> cities, TransportMode m)
        {
            //List<Link> result = new List<Link>();
            for (int i = 0; i < cities.Count - 1; i++)
            {
                City from = cities[i];
                City to = cities[i + 1];
                yield return GetRoute(from, to, m);
            }


            //return result;
        }

        public Link GetRoute(City from, City to, TransportMode m)
        {
            Link res = null;
            foreach (var a in routes)
            {
                if (((a.FromCity == from && a.ToCity == to) ||
                    a.FromCity == to && a.ToCity == from) && a.TransportMode == m)
                    return a;
            }
            return null;
        }





        private IEnumerable<Link> GetListOfAllOutgoingRoutes(City visitingCity, TransportMode mode)
        {
            return routes.Where(l => ((l.FromCity.Equals(visitingCity) || l.ToCity.Equals(visitingCity)) && l.TransportMode == mode));
        }

        private class DijkstraNode : IComparable<DijkstraNode>
        {
            public City VisitingCity;
            public double Distance;
            public City PreviousCity;

            public int CompareTo(DijkstraNode other)
            {
                return other.Distance.CompareTo(Distance);
            }
        }


    }
}
