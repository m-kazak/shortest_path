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
            RunProgram("1_victoria_lake");
            RunProgram("2_himalayas");
            RunProgram("3_budapest");
            RunProgram("4_manhattan");
            RunProgram("5_oceania");

            Console.WriteLine("Completed! Press any key");
            Console.ReadLine();
        }

        static public void RunProgram(String fileName)
        {
            Parser parser = new Parser();
            Map map = parser.GetParsedMap(fileName + ".txt");
            RouteFinder finder = new RouteFinder();

            //map.DrawCharMap();

            var r = finder.FindRoutes(map);

            Output.OutputFile(r, fileName + "_answer.txt");
        }

    }
}
