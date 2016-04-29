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
            try
            {
                Assembly[] a = AppDomain.CurrentDomain.GetAssemblies();
                Type assemType = null;
                foreach (var assembly in a)
                {
                    if (assemType == null)
                    {
                        assemType = assembly.GetType(algorithmClassName);
                    }
                }

                if (assemType != null)
                {
                    var type = Type.GetType(algorithmClassName);
                    var constructor = type.GetConstructor(new[] { typeof(Cities) });
                    var instance = constructor.Invoke(new object[] { cities });
                    return (IRoutes)instance;
                }
                else throw new NotSupportedException();
            }
            catch (Exception) { throw new NotSupportedException(); }

            return null;
        }
    }
}
