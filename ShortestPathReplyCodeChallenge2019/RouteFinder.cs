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

        //Find routes from all customers to all customers
        public List<Route> FindCustomerRoutes(Map map)
        {
            List<Route> routes = new List<Route>();
            foreach (var cus in map.Customers)
            {
                var cells = finder.DrawCellMap(map, cus.Coo);

                List<Path> paths = new List<Path>();
                foreach (var c in map.Customers)
                {
                    var coos = finder.RestoreWay(cells, c, cus.Coo);
                    if (coos.Count > 1)
                        paths.Add(new Path(coos, cus, cells[c.Coo.X, c.Coo.Y].Cost));
                }

                routes.Add(new Route(cus.Coo, paths));
            }

            return routes;
        }

        //Getting list of routes that joined
        public List<List<Route>> GetJoinRoutes(List<Route> routes)
        {
            List<List<Route>> joined_routes = new List<List<Route>>();

            foreach (var rr in routes)
            {
                bool found = false;
                foreach (var jr in joined_routes)
                {
                    if (jr.FindIndex(r => r == rr) > -1)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                    continue;

                List<Route> temp_routes = new List<Route>();
                foreach (var p in rr.Paths)
                {
                    temp_routes.Add(routes.Find(r => r.Office.X == p.Way.Last().X && r.Office.Y == p.Way.Last().Y));
                }

                temp_routes.Add(rr);
                joined_routes.Add(temp_routes);
            }

            return joined_routes;
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

        //Get optimal tree - Minimum spanning tree
        public List<Section> GetOptimalTree(List<Route> routes)
        {
            List<Section> sections = new List<Section>();
            List<Coordinate> coordinates = new List<Coordinate>();

            Path path = routes.First().Paths.OrderBy(p=>p.Cost).First();

            sections.Add(new Section(path.Way.First(), path.Way.Last(), path.Cost));
            coordinates.Add(path.Way.First());
            coordinates.Add(path.Way.Last());
            
            while (true)
            {
                if (coordinates.Count == routes.Count)
                    break;

                int temp_cost = int.MaxValue;
                foreach (var c in coordinates)
                {
                    var paths = routes.Find(r => r.Office.X == c.X && r.Office.Y == c.Y).Paths.OrderBy(p=>p.Cost);
                    foreach (var p in paths)
                    {
                        if (!finder.IsCooInList(coordinates, p.Way.Last()) && !IsSectionInList(sections, p.Way.First(), p.Way.Last()))
                        {
                            if (temp_cost > p.Cost)
                            {
                                temp_cost = p.Cost;
                                path = p;
                                break;
                            }
                        }
                    }
                }

                sections.Add(new Section(path.Way.First(), path.Way.Last(), path.Cost));
                coordinates.Add(path.Way.Last());
            }

            return sections;
        }

        //Checking if section exists in list (by coordinates)
        public bool IsSectionInList(List<Section> sections, Coordinate x, Coordinate y)
        {
            foreach (var sec in sections)
            {
                if (IsCooEqual(sec.A, x) && IsCooEqual(sec.B, y))
                    return true;
                if (IsCooEqual(sec.A, y) && IsCooEqual(sec.B, x))
                    return true;
            }
            return false;
        }

        //Checking if section exists in list
        public bool IsSectionInList(List<Section> sections, Section section)
        {
            foreach (var sec in sections)
            {
                if (IsCooEqual(sec.A, section.A) && IsCooEqual(sec.B, section.B))
                    return true;
                if (IsCooEqual(sec.A, section.B) && IsCooEqual(sec.B, section.A))
                    return true;
            }
            return false;
        }

        //Compare 2 coordinates
        public bool IsCooEqual(Coordinate a, Coordinate b)
        {
            if (a.X == b.X && a.Y == b.Y)
                return true;
            else
                return false;
        }

        //Splut tree in specific number of branches
        public List<List<Section>> SplitTree(List<Section> tree, int division)
        {
            List<List<Section>> split_tree = new List<List<Section>>();
            split_tree.Add(tree);
            List<Section> temp_tree = null;

            for (int i = 1; i < division; i++)
            {
                int temp_cost = int.MinValue;
                foreach (var st in split_tree)
                {
                    if (temp_cost < st.OrderByDescending(s => s.Cost).First().Cost)
                    {
                        temp_cost = st.OrderByDescending(s => s.Cost).First().Cost;
                        temp_tree = st;
                    }
                }

                var branches = SplitTree(temp_tree);

                split_tree.Remove(temp_tree);

                foreach (var b in branches)
                {
                    split_tree.Add(b);
                }
                
            }

            return split_tree;
        }

        //Splitting tree into 2 branches
        public List<List<Section>> SplitTree(List<Section> tree)
        {
            List<Section> left_branch = new List<Section>();
            List<Section> right_branch = new List<Section>();
            List<Section> temp_branch = new List<Section>();
            List<List<Section>> split_tree = new List<List<Section>>();

            Section weak_section = tree.OrderByDescending(t => t.Cost).First();

            //Building left branch
            foreach (var t in tree)
            {
                if (t != weak_section)
                    if (IsCooEqual(weak_section.A, t.B) || IsCooEqual(weak_section.A, t.A))
                    left_branch.Add(t);
            }

            while (true)
            {
                temp_branch.Clear();
                foreach (var lb in left_branch)
                {
                    foreach (var t in tree)
                    {
                        if (t != weak_section)
                            if (IsCooEqual(lb.A, t.B) || (IsCooEqual(lb.A, t.A) && !IsSectionInList(left_branch, t)))
                                temp_branch.Add(t);
                    }
                }

                if (temp_branch.Count == 0)
                    break;

                foreach (var t in temp_branch)
                {
                    left_branch.Add(t);
                }
            }

            //Building right branch
            foreach (var t in tree)
            {
                if (t != weak_section)
                    if (IsCooEqual(weak_section.B, t.A) || IsCooEqual(weak_section.B, t.B))
                        right_branch.Add(t);
            }

            while (true)
            {
                temp_branch.Clear();
                foreach (var lb in right_branch)
                {
                    foreach (var t in tree)
                    {
                        if (t != weak_section)
                            if (IsCooEqual(lb.B, t.A) || (IsCooEqual(lb.B, t.B) && !IsSectionInList(right_branch, t)))
                                temp_branch.Add(t);
                    }
                }

                if (temp_branch.Count == 0)
                    break;

                foreach (var t in temp_branch)
                {
                    right_branch.Add(t);
                }
            }

            if (left_branch.Count == 0)
                left_branch.Add(new Section(weak_section.A, weak_section.A, 0));
            if (right_branch.Count == 0)
                right_branch.Add(new Section(weak_section.B, weak_section.B, 0));

            split_tree.Add(left_branch);
            split_tree.Add(right_branch);

            return split_tree;
        }

        //NOT USING

        //Find routes to all accessable customers from all coordinates
        public List<Route> FindRoutesOld(Map map, List<Customer> customers)
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
    }
}
