using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectReader
    {
        private TextReader reader;

        public SimpleObjectReader(TextReader reader)
        {
            this.reader = reader;
        }

        public Object Next()
        {
            if (reader != null)
            {
                var line = reader.ReadLine();
                if (line != null && line.Contains("Instance of "))
                {
                    var parts = line.Split(' ');
                    var type = Type.GetType(parts[2]);
                    if (type == null)
                    {
                        var a = AppDomain.CurrentDomain.GetAssemblies();

                        foreach (var assembly in a)
                        {
                            try
                            {
                                var assemType = assembly.GetType(parts[2]);
                                if (assemType != null)
                                {
                                    {
                                        type = assemType;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                return null;
                            }
                        }
                    }

                    var obj = Activator.CreateInstance(type);
                    while (line != null && obj != null && !line.Contains("End of instance"))
                    {
                        PropertyInfo pInfo;
                        if (line.Contains("is a nested object"))
                        {
                            parts = line.Split(' ');
                            pInfo = obj.GetType().GetProperty(parts[0]);
                            pInfo.SetValue(obj, this.Next());
                        }
                        else
                        {
                            parts = line.Split('=');
                            pInfo = obj.GetType().GetProperty(parts[0]);
                            if (pInfo != null)
                            {
                                if (pInfo.PropertyType == typeof(string))
                                {
                                    pInfo.SetValue(obj, parts[1].Trim('\"'));
                                }
                                else if (pInfo.PropertyType == typeof(double))
                                {
                                    pInfo.SetValue(obj, Double.Parse(parts[1], CultureInfo.InvariantCulture));
                                }
                                else if (pInfo.PropertyType == typeof(Int32))
                                {
                                    pInfo.SetValue(obj, Int32.Parse(parts[1], CultureInfo.InvariantCulture));
                                }

                            }
                        }
                        line = reader.ReadLine();
                    }
                    return obj;
                }
            }
            return null;
        }
    }
}
