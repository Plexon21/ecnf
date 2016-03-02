using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    class RoutePlannerConsoleApp
    {
        static void Main(string[] args)
        {
            var version = AssemblyName.GetAssemblyName("RoutePlannerConsole.exe").Version;
            Console.WriteLine("Welcome to Routeplanner (Version " + version + ")");
            var bern = new WayPoint("Bern", 46.95, 7.44);
            var tripolis = new WayPoint("Tripolis", 32.876174, 13.187507);
            Console.WriteLine(bern.Distance(tripolis));
        }
    }
}
