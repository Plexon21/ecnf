using System.Collections.Concurrent;
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
        public static void AddIfNotNull<U>(this List<U> list, U value)
        where U : class
        {
            if (value != null) { list.Add(value); }
        }
        public static void AddIfNotNull<U>(this ConcurrentBag<U> list, U value)
       where U : class
        {
            if (value != null) { list.Add(value); }
        }
    }
}
