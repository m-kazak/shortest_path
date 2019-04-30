using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathReplyCodeChallenge2019
{
    class Map
    {
        public int N { get; }
        public int M { get; }
        public char[,] CharMap { get; }
        public char[,] CharCustomerMap { get; }
        public int[,] IntMap { get; }
        public List<Customer> Customers { get; }

        public int CustomerCount { get; }
        public int ReplyCount { get; }

        public Map(int x, int y, int c, int r, List<Customer> cus)
        {
            N = x;
            M = y;
            CustomerCount = c;
            ReplyCount = r;
            CharMap = new char[x, y];
            IntMap = new int[x, y];
            Customers = cus;
        }

        public void DrawCharMap()
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(CharMap[j, i]);
                }
                Console.WriteLine();
            }
        }

        public void DrawIntMap()
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(IntMap[j, i]);
                }
                Console.WriteLine();
            }
        }

        public List<Coordinate> AvailableCoordinates()
        {
            List<Coordinate> coo = new List<Coordinate>();

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (CharMap[j, i] != '#' && CharMap[j, i] != 'C')
                        coo.Add(new Coordinate(j, i));
                }
            }
            return coo;
        }

    }
}
