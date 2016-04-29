using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
   public  class SimpleObjectReader
    {
        private TextReader stringReader;

        public SimpleObjectReader(TextReader stringReader)
        {
            this.stringReader = stringReader;
        }

        public Object Next()
        {
            throw new NotImplementedException();
        }
    }
}
