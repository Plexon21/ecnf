using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.FormattableString;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
   public class SimpleObjectWriter
    {
        private TextWriter writer;

        public SimpleObjectWriter(TextWriter sw1)
        {
            this.writer = sw1;
        }

        public void Next(Object o)
        {
            if (writer != null && o != null)
            {
                writer.WriteLine("Instance of {0}", o.GetType().FullName);
                foreach (var property in o.GetType().GetProperties().OrderBy(ob => ob.Name))
                {
                    var propType = property.GetValue(o);
                    if (!property.CustomAttributes.Select(x => x.AttributeType == typeof (XmlIgnoreAttribute)).Any())
                    {
                        if (propType is string)
                        {
                            writer.WriteLine("{0}=\"{1}\"",property.Name, property.GetValue(o, System.Reflection.BindingFlags.GetProperty, null, null, CultureInfo.InvariantCulture));
                        }
                        else if (propType is ValueType)
                        {
                            writer.WriteLine(Invariant($"{property.Name}={property.GetValue(o, System.Reflection.BindingFlags.GetProperty, null, null, CultureInfo.InvariantCulture)}"));
                        }
                        else
                        {
                            writer.WriteLine("{0} is a nested object...", property.Name);
                            this.Next(property.GetValue(o, System.Reflection.BindingFlags.GetProperty, null, null, CultureInfo.InvariantCulture));
                        }
                    }
                }
                writer.WriteLine("End of instance");
            }
        }

    }
}
