using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathReplyCodeChallenge2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            Map map = parser.GetParsedMap("1_victoria_lake.txt");
            PathFinder finder = new PathFinder();

            var rr = finder.FindRoutes(map, map.Customers);
            Console.WriteLine(rr.Count);
            rr.OrderByDescending(o => o.Reward).ToList();

            Console.WriteLine("Completed! Press any key");
            Console.ReadLine();
        }

    }
}
