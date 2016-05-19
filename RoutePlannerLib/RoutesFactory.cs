using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Properties;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RoutesFactory
    {
        static public IRoutes Create(Cities cities)
        {
            return Create(cities, Settings.Default.RouteAlgorithm);
        }

        static public IRoutes Create(Cities cities, string algorithmClassName)
        {
            var a = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in a)
            {
                try
                {
                    var assemType = assembly.GetType(algorithmClassName);
                    if (assemType != null)
                    {
                        {
                            var constructor = assemType.GetConstructor(new[] {typeof (Cities)});
                            var instance = constructor.Invoke(new object[] {cities});
                            return (IRoutes) instance;
                        }
                    }
                }
                catch (Exception)
                {
                    throw new NotSupportedException();
                }
            }
            throw new NotSupportedException();
        }
    }
}
