﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathReplyCodeChallenge2019
{
    class PathFinder
    {
        //Draw cost to each cell from 1 coordinate
        public Cell[,] DrawCellMap(Map map, Coordinate coo)
        {
            Cell[,] cells = new Cell[map.N, map.M];
            List<Coordinate> queue = new List<Coordinate>();
            List<Coordinate> temp_queue = new List<Coordinate>();
            int temp_cost = 0;

            for (int i = 0; i < map.M; i++)
            {
                for (int j = 0; j < map.N; j++)
                {
                    cells[j, i] = new Cell(new Coordinate(j, i));
                }
            }

            //Initialize starting cell
            queue.Add(coo);
            cells[coo.X, coo.Y].Cost = 0;
            cells[coo.X, coo.Y].PrevCoo = new Coordinate(-1, -1);

            //Main loop
            while (true)
            {
                //Process queue
                foreach (var q in queue)
                {
                    cells[q.X, q.Y].Checked = true;
                    var coos = PossibleWays(map, q);

                    //Process nearest cells
                    foreach (var c in coos)
                    {
                        if (cells[c.X, c.Y].Checked)
                            continue;
                        temp_cost = cells[q.X, q.Y].Cost + map.IntMap[c.X, c.Y];
                        if (temp_cost < cells[c.X, c.Y].Cost)
                        {
                            cells[c.X, c.Y].Cost = temp_cost;
                            cells[c.X, c.Y].PrevCoo = q;
                        }

                        bool queued = false;
                        foreach (var tq in temp_queue)
                        {
                            if (tq.X == c.X && tq.Y == c.Y)
                            {
                                queued = true;
                                break;
                            }
                        }
                        if (!queued)
                            temp_queue.Add(c);
                    }
                }

                //Exit when no cells to check
                if (temp_queue.Count == 0)
                    break;

                //Moving queue
                queue.Clear();
                foreach (var tq in temp_queue)
                {
                    queue.Add(tq);
                }
                temp_queue.Clear();
            }

            return cells;
        }

        //Restore path to customer from coordinate
        public List<Coordinate> RestoreWay(Cell[,] cell_map, Customer cus, Coordinate coo)
        {
            List<Coordinate> coos = new List<Coordinate>();
            Coordinate temp_coo = cus.Coo;

            while (true)
            {
                if (cell_map[temp_coo.X, temp_coo.Y].Cost == int.MaxValue)
                    break;

                if (cell_map[temp_coo.X, temp_coo.Y].Cost == 0)
                {
                    coos.Add(temp_coo);
                    break;
                }
                coos.Add(temp_coo);
                temp_coo = cell_map[temp_coo.X, temp_coo.Y].PrevCoo;
            }

            coos.Reverse();

            return coos;
        }

        //Possible ways from coordinate. Exclude out of map and mountains.
        public List<Coordinate> PossibleWays(Map map, Coordinate coo)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            if (coo.X + 1 < map.N && map.CharMap[coo.X + 1, coo.Y] != '#')
                coordinates.Add(new Coordinate(coo.X + 1, coo.Y));
            if (coo.X - 1 >= 0 && map.CharMap[coo.X - 1, coo.Y] != '#')
                coordinates.Add(new Coordinate(coo.X - 1, coo.Y));
            if (coo.Y + 1 < map.M && map.CharMap[coo.X, coo.Y + 1] != '#')
                coordinates.Add(new Coordinate(coo.X, coo.Y + 1));
            if (coo.Y - 1 >= 0 && map.CharMap[coo.X, coo.Y - 1] != '#')
                coordinates.Add(new Coordinate(coo.X, coo.Y - 1));

            return coordinates;
        }

        //Find one-step coordinates for allocation Reply office
        public List<Coordinate> OneStepCoordinates(Map map, Coordinate coo)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            //Manhattan
            if (coo.X + 1 < map.N && map.CharMap[coo.X + 1, coo.Y] != '#' && map.CharMap[coo.X + 1, coo.Y] != 'C')
                coordinates.Add(new Coordinate(coo.X + 1, coo.Y));
            if (coo.X - 1 >= 0 && map.CharMap[coo.X - 1, coo.Y] != '#' && map.CharMap[coo.X - 1, coo.Y] != 'C')
                coordinates.Add(new Coordinate(coo.X - 1, coo.Y));
            if (coo.Y + 1 < map.M && map.CharMap[coo.X, coo.Y + 1] != '#' && map.CharMap[coo.X, coo.Y + 1] != 'C')
                coordinates.Add(new Coordinate(coo.X, coo.Y + 1));
            if (coo.Y - 1 >= 0 && map.CharMap[coo.X, coo.Y - 1] != '#' && map.CharMap[coo.X, coo.Y - 1] != 'C')
                coordinates.Add(new Coordinate(coo.X, coo.Y - 1));

            //Diagonal
            if (coo.X + 1 < map.N && coo.Y + 1 < map.M && map.CharMap[coo.X + 1, coo.Y + 1] != '#' && map.CharMap[coo.X + 1, coo.Y + 1] != 'C')
                coordinates.Add(new Coordinate(coo.X + 1, coo.Y + 1));
            if (coo.X - 1 >= 0 && coo.Y - 1 >= 0 && map.CharMap[coo.X - 1, coo.Y - 1] != '#' && map.CharMap[coo.X - 1, coo.Y - 1] != 'C')
                coordinates.Add(new Coordinate(coo.X - 1, coo.Y - 1));
            if (coo.X + 1 < map.M && coo.Y - 1 >= 0 && map.CharMap[coo.X + 1, coo.Y - 1] != '#' && map.CharMap[coo.X + 1, coo.Y - 1] != 'C')
                coordinates.Add(new Coordinate(coo.X + 1, coo.Y - 1));
            if (coo.X - 1 >= 0 && coo.Y + 1 < map.M && map.CharMap[coo.X - 1, coo.Y + 1] != '#' && map.CharMap[coo.X - 1, coo.Y + 1] != 'C')
                coordinates.Add(new Coordinate(coo.X - 1, coo.Y + 1));

            return coordinates;
        }

        //Check if coordinate already in list
        public bool IsCooInList(List<Coordinate> coordinates, Coordinate coo)
        {
            foreach (var c in coordinates)
            {
                if (c.X == coo.X && c.Y == coo.Y)
                    return true;
            }
            return false;
        }

    }
}
