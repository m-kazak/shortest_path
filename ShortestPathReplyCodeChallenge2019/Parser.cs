using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathReplyCodeChallenge2019
{
    class Parser
    {
        //Get parsed Map
        public Map GetParsedMap(string filePath)
        {
            int n, m, c, r;
            string init_line, customer_line, map_line;
            Map map;

            List<Customer> customers = new List<Customer>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                init_line = sr.ReadLine();
                String[] init_arr = init_line.Split(' ');
                n = Convert.ToInt32(init_arr[0]);
                m = Convert.ToInt32(init_arr[1]);
                c = Convert.ToInt32(init_arr[2]);
                r = Convert.ToInt32(init_arr[3]);

                //parse customers
                for (int i = 1; i <= c; i++)
                {
                    customer_line = sr.ReadLine();
                    String[] customer_arr = customer_line.Split(' ');
                    customers.Add(new Customer(i, new Coordinate(Convert.ToInt32(customer_arr[0]), Convert.ToInt32(customer_arr[1])), Convert.ToInt32(customer_arr[2])));
                }

                customers.OrderByDescending(o => o.Reward).ToList();

                //draw char, value and cell map
                map = new Map(n, m, c, r, customers);

                for (int i = 0; i < m; i++)
                {
                    map_line = sr.ReadLine();
                    Char[] map_arr = map_line.ToCharArray();
                    for (int j = 0; j < n; j++)
                    {
                        map.CharMap[j, i] = map_arr[j];
                        map.IntMap[j, i] = GetCost(map_arr[j]);
                    }
                }
            }

            //locate offices on the map
            foreach (var cus in customers)
            {
                map.CharMap[cus.Coo.X, cus.Coo.Y] = 'C';
            }

            return map;
        }

        //Get cost of moving via terrain
        private int GetCost(char terrain)
        {
            switch (terrain)
            {
                case '#':
                    return -1;
                case '~':
                    return 800;
                case '*':
                    return 200;
                case '+':
                    return 150;
                case 'X':
                    return 120;
                case '_':
                    return 100;
                case 'H':
                    return 70;
                case 'T':
                    return 50;
                default:
                    throw new ArgumentException($"Terrain '{terrain}' unknown");
            }
        }
    }
}
