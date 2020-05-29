using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_GetOptimalScheduleGain
{
    class Program
    {
        public class Event
        {
            public int Price;
            public int StartTime, FinishTime;
        }
        public static int GetOptimalScheduleGain(params Event[] events)
        {
            // добавление fakeBorderEvent позволяет не обрабатывать некоторые граничные случаи
            var fakeBorderEvent = new Event { StartTime = int.MinValue, FinishTime = int.MinValue, Price = 0 };
            events = events.Concat(new[] { fakeBorderEvent }).OrderBy(e => e.FinishTime).ToArray();

            // OPT(k) = Max(OPT(k-1), w(k) + OPT(p(k))
            var opt = new int[events.Length];
            opt[0] = 0; // нулевым всегда будет fakeBorderEvent
            for (var k = 1; k < events.Length; k++)
            {
                var p = 0;
                for (int e = 0; e < k; e++)
                    if (events[e].FinishTime < events[k].StartTime)
                        p = e;
                opt[k] = Math.Max(opt[k - 1], events[k].Price + opt[p]);
            }
            return opt.Last();
        }
        static void Main(string[] args)
        {
            Assert.AreEqual(0, GetOptimalScheduleGain(new Event[0]));
            Assert.AreEqual(50, GetOptimalScheduleGain(
                new Event { StartTime = 1, FinishTime = 11, Price = 50 }));
            Assert.AreEqual(280, GetOptimalScheduleGain(
                new Event { StartTime = 9, FinishTime = 11, Price = 50 },
                new Event { StartTime = 10, FinishTime = 13, Price = 190 },
                new Event { StartTime = 14, FinishTime = 16, Price = 90 },
                new Event { StartTime = 12, FinishTime = 15, Price = 200 }));
            SecretTest();
            Console.WriteLine("OK");
        }
    }
}
