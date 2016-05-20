using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlannerGarbage
{
    class Program
    {

        /* Resultate:
        * Ohne anpassungen:                     00:00:00.0420933
        * 
        * Änderungen in App.config:
        * GC server enabled:                    00:00:00.0277348
        * concurrent enabled:                   00:00:00.0436908
        * cpu gruppen enabled:                  00:00:00.0420473
        * grosse objekte erlaut:                00:00:00.0422410
        * Alle configsettings eingeschaltet:    00:00:00.0752415
        * 
        * GCSettings.LatencyMode anpassungen:
        * GCLatencyMode.Interactive:            00:00:00.0485255
        * GCLatencyMode.SustainedLowLatency:    00:00:00.0443946
        * GCLatencyMode.Batch:                  00:00:00.0435164
        * GCLatencyMode.LowLatency:             00:00:00.0423008
        */
        static void Main(string[] args)
        {
            GarbageTest tc = new GarbageTest(1000000);
            //GCSettings.LatencyMode = GCLatencyMode.Interactive;
            //GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            //GCSettings.LatencyMode = GCLatencyMode.Batch;
            //GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            tc.RunTest();
            Console.ReadLine();
        }
    }

    public class GarbageTest
    {
        private readonly Stopwatch watch = new Stopwatch();
        private readonly int size;

        public GarbageTest(int size)
        {
            this.size = size;
        }


        public void RunTest()
        {
            List<TextWriter> l = new List<TextWriter>();
            for (int i = 0; i < size; i++)
            {
                TextWriter r = new StringWriter(new StringBuilder(5));
                l.Add(r);
            }
            Console.WriteLine(GC.GetTotalMemory(true));
            l = null;
            watch.Start();
            GC.Collect();
            GC.Collect(0, GCCollectionMode.Optimized, false);
            GC.Collect(0, GCCollectionMode.Forced, true);
            watch.Stop();
            Console.WriteLine("Gesamtzeit {0}", watch.Elapsed);
        }
    }
}
