using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathReplyCodeChallenge2019
{
    class RouteFinder
    {
        private PathFinder finder = new PathFinder();

        //Find routes to all accessable customers from all coordinates
        public List<Route> FindRoutes(Map map, List<Customer> customers)
        {
            List<Coordinate> coordinates = map.AvailableCoordinates();
            List<Route> routes = new List<Route>();
            int i = 1;
            foreach (var coo in coordinates)
            {
                Console.WriteLine(i++);
                var cells = finder.DrawCellMap(map, coo);

                List<Path> paths = new List<Path>();
                foreach (var cus in customers)
                {
                    var coos = finder.RestoreWay(cells, cus, coo);
                    if (coos.Count > 0)
                        paths.Add(new Path(coos, cus, cells[cus.Coo.X, cus.Coo.Y].Cost)); // REDO
                }

                routes.Add(new Route(coo, paths));
            }

            return routes;
        }

        //Find routes from approximately coordinates
        public List<Route> FindRoutes(Map map)
        {
            List<Coordinate> coordinates = FindAproxCoos(map);
            List<Route> routes = new List<Route>();

            foreach (var coo in coordinates)
            {
                var cells = finder.DrawCellMap(map, coo);

                List<Path> paths = new List<Path>();
                foreach (var cus in map.Customers)
                {
                    var coos = finder.RestoreWay(cells, cus, coo);
                    if (coos.Count > 0 && cus.Reward - cells[cus.Coo.X, cus.Coo.Y].Cost > 0)
                        paths.Add(new Path(coos, cus, cells[cus.Coo.X, cus.Coo.Y].Cost));
                }

                routes.Add(new Route(coo, paths));
            }

            return routes;
        }

        //Find approximately coordinates
        public List<Coordinate> FindAproxCoos(Map map)
        {
            List<Customer> customers = map.Customers.OrderByDescending(r => r.RealReward).ToList();
            List<Coordinate> coordinates = new List<Coordinate>();
            int available_offices = map.ReplyCount;

            foreach (var cus in customers)
            {
                var coos = finder.OneStepCoordinates(map, cus.Coo);

                foreach (var coo in coos)
                {
                    if (available_offices > 0 && !finder.IsCooInList(coordinates, coo))
                    {
                        coordinates.Add(coo);
                        available_offices--;
                    }
                }

                if (available_offices == 0)
                    break;
            }

            return coordinates;
        }

    }
}
