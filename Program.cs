using System.Threading.Tasks;

namespace HelloUdf
{
    class Program
    {
        // static void Main(string[] args)
        // {
        //     // HelloUdfExample.Run();
        //     // CountAndAggregateMnM.Run();
        // }

        static async Task Main(string[] args)
        {
            await Yahoo.Tick();
        }
    }
}