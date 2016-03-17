using System.Collections.Generic;
using System.IO;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public static class Extensions
    {
        public static IEnumerable<string[]> GetSplittedLines(this TextReader reader, char splitChar)
        {
            string line = reader.ReadLine();
            while (line != null)
            {
                yield return line.Split(splitChar);
                line = reader.ReadLine();
            }
        }
    }
}
