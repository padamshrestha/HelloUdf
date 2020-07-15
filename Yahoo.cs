using System;
using System.Threading.Tasks;
using Fynance;

namespace HelloUdf
{
    public class Yahoo
    {
        public static async Task Tick()
        {
            var result = await Ticker.Build()
                         .SetSymbol("MSFT")
                         .SetPeriod(Period.OneDay)
                         .SetInterval(Interval.OneHour)
                         .GetAsync();

            Console.WriteLine(result.RegularMarketPrice);
            var i = 1;
            foreach(var item in result.Quotes){
                Console.WriteLine($"Value of i: {i} - Volume is {item.Volume} - High is {item.High} and Low {item.Low}");
                i++;
            }            
        }

    }
}