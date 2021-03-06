﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestWatcher
    {
        private Dictionary<City, int> requests = new Dictionary<City, int>();
        public void LogRouteRequests(object sender, RouteRequestEventArgs e)
        {
            if (!requests.ContainsKey(e.ToCity))
            {
                requests.Add(e.ToCity, 1);
            }
            else
            {
                requests[e.ToCity]++;
            }
            Console.WriteLine("Current Request State");
            Console.WriteLine("---------------------");
            foreach (KeyValuePair<City, int> c in requests)
            {
                Console.WriteLine($"ToCity: {c.Key.Name} has been requested {c.Value} times.");
            }
        }

        public int GetCityRequests(City city)
        {
            if (requests.ContainsKey(city))
                return requests?[city] ?? 0;
            else
                return 0;
        }
    }
}
