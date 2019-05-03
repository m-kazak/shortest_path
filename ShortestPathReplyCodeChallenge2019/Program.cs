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
            RunProgram("test");
            //RunProgram("1_victoria_lake");
            //RunProgram("2_himalayas");
            //RunProgram("3_budapest");
            //RunProgram("4_manhattan");
            //RunProgram("5_oceania");

            /*
            List<Coordinate> ll = new List<Coordinate>();
            ll.Add(new Coordinate(1,1));

            Path pp = new Path(ll, new Customer(1, new Coordinate(1, 1), 1), 1);

            List<Coordinate> lc = new List<Coordinate>();
            lc.Add(pp.Way.First());

            pp = null;
            ll.Clear();

            Console.WriteLine(lc[0].X);*/

            Console.WriteLine("Completed! Press any key");
            Console.ReadLine();
        }


        static public void RunProgram(String fileName)
        {
            Parser parser = new Parser();
            Map map = parser.GetParsedMap(fileName + ".txt");
            RouteFinder finder = new RouteFinder();

            var rr = finder.FindRoutes(map, map.Customers);
            var tree = finder.GetOptimalTree(rr);
            var split_tree = finder.SplitTree(tree);

            foreach (var r in rr)
            {
                foreach (var p in r.Paths)
                {
                    Console.WriteLine("{0} {1} {2} {3}", r.Office.X, r.Office.Y, p.GetDirection(),p.Cost);
                }
            }

            foreach (var t in tree)
            {
                Console.WriteLine("A:{0} {1} B:{2} {3} Cost:{4}", t.A.X, t.A.Y, t.B.X, t.B.Y, t.Cost);
            }
            Console.WriteLine("************");
            foreach (var st in split_tree)
            {
                Console.WriteLine("-------------");
                foreach (var t in st)
                {
                    Console.WriteLine("A:{0} {1} B:{2} {3} Cost:{4}", t.A.X, t.A.Y, t.B.X, t.B.Y, t.Cost);
                }
            }
        }

        //2nd score. Locate near rewardest and try reach others
        static public void RunProgram1(String fileName)
        {
            Parser parser = new Parser();
            Map map = parser.GetParsedMap(fileName + ".txt");
            RouteFinder finder = new RouteFinder();

            var r = finder.FindRoutes(map);

            Output.OutputFile(r, fileName + "_answer.txt");
        }

    }
}
