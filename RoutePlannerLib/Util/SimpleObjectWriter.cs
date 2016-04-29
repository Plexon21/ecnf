using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
   public class SimpleObjectWriter
    {
        private TextWriter sw1;

        public SimpleObjectWriter(TextWriter sw1)
        {
            this.sw1 = sw1;
        }

        public void Next(Object o)
        {
            throw new NotImplementedException();
        }

    }
}
