using Google.Maps;
using Google.Maps.DistanceMatrix;
using Google.OrTools.ConstraintSolver;
using SWay.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWay
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<string> addresses = new List<string>();
            //addresses.Add("34-620 Jodłownik, Jodłownik 44/7");
            //addresses.Add("43-150 Bieruń, Homera 30/32");
            //addresses.Add("Kraków, Bojki 24");
            //addresses.Add("Wrocław, Sztabowa 10");
            //addresses.Add("Kasina Wielka 421");

            //APIGoogle.Register("AIzaSyALT_bBzigIMao0ri0hm7kjhMuiNKmUJYU");

            //List<LatLng> listLatLng = new List<LatLng>();
            //foreach (string item in addresses)
            //{
            //    listLatLng.Add(APIGoogle.GetLatLng(item));
            //}

            //DistanceMatrixRequest request = new DistanceMatrixRequest()
            //{
            //    WaypointsOrigin = new List<Location>(listLatLng),
            //    WaypointsDestination = new List<Location>(listLatLng),
            //    Sensor = false
            //};

            //var response = new DistanceMatrixService().GetResponse(request);

            //for (int i = 0; i < response.Rows.Count(); i++)
            //{

            //    for (int j = 0; j < response.Rows[i].Elements.Count(); j++)
            //    {
            //        Console.WriteLine(response.DestinationAddresses[i] + "  <->  " + response.DestinationAddresses[j]);
            //        Console.WriteLine(response.Rows[i].Elements[j].distance.Text);
            //        Console.WriteLine(response.Rows[i].Elements[j].duration.Text);
            //        Console.WriteLine();
            //    }
            //}

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            List<string> CityNames = new List<string>() { "New York", "Los Angeles", "Chicago", "Minneapolis",
                                                            "Denver", "Dallas", "Seattle", "Boston", "San Francisco", "St.Louis",
                                                            "Houston", "Phoenix", "Salt Lake City" };

            int[,] CityDistanceMatrix = new int[,]  {
                                                    {0, 2451, 713, 1018, 1631, 1374, 2408, 213, 2571, 875, 1420, 2145, 1972}, // New York
                                                    { 2451, 0, 1745, 1524, 831, 1240, 959, 2596, 403, 1589, 1374, 357, 579 }, // Los Angeles
                                                    {713, 1745, 0, 355, 920, 803, 1737, 851, 1858, 262, 940, 1453, 1260}, // Chicago
                                                    {1018, 1524, 355, 0, 700, 862, 1395, 1123, 1584, 466, 1056, 1280, 987}, // Minneapolis
                                                    {1631, 831, 920, 700, 0, 663, 1021, 1769, 949, 796, 879, 586, 371}, // Denver
                                                    {1374, 1240, 803, 862, 663, 0, 1681, 1551, 1765, 547, 225, 887, 999}, // Dallas
                                                    {2408, 959, 1737, 1395, 1021, 1681, 0, 2493, 678, 1724, 1891, 1114, 701}, // Seattle
                                                    {213, 2596, 851, 1123, 1769, 1551, 2493, 0, 2699, 1038, 1605, 2300, 2099}, // Boston
                                                    {2571, 403, 1858, 1584, 949, 1765, 678, 2699, 0, 1744, 1645, 653, 600}, // San Francisco
                                                    {875, 1589, 262, 466, 796, 547, 1724, 1038, 1744, 0, 679, 1272, 1162}, // St. Louis
                                                    {1420, 1374, 940, 1056, 879, 225, 1891, 1605, 1645, 679, 0, 1017, 1200}, // Houston
                                                    {2145, 357, 1453, 1280, 586, 887, 1114, 2300, 653, 1272, 1017, 0, 504}, // Phoenix
                                                    {1972, 579, 1260, 987, 371, 999, 701, 2099, 600, 1162, 1200, 504, 0} // Salt Lake City
                                                };

            int NumRoutes = 1;    // The number of routes, which is 1 in the TSP.
                                  // Nodes are indexed from 0 to tsp_size - 1. The depot is the starting node of the route.
            int Depot = 0;
            int TspSize = CityNames.Count;


            if (TspSize > 0)
            {

                RoutingModel routing = new RoutingModel(TspSize, NumRoutes, Depot);

                RoutingSearchParameters search_parameters = RoutingModel.DefaultSearchParameters();

                CityDistance dist_between_nodes = new CityDistance(CityDistanceMatrix, CityNames);
                routing.SetCost(dist_between_nodes);

                search_parameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
                Assignment solution = routing.SolveWithParameters(search_parameters);

                Console.WriteLine("Status = {0}", routing.Status());
                if (solution != null)
                {
                    // Solution cost.
                    Console.WriteLine("Cost = {0}", solution.ObjectiveValue());
                    // Inspect solution.
                    // Only one route here; otherwise iterate from 0 to routing.vehicles() - 1
                    int route_number = 0;
                    for (long node = routing.Start(route_number);
                         !routing.IsEnd(node);
                         node = solution.Value(routing.NextVar(node)))
                    {
                        Console.Write("{0} -> ", CityNames[(int)node]);
                    }
                    Console.WriteLine(CityNames[0]);
                    Console.ReadKey();
                }
            }


            Console.ReadKey();
        }
    }
}
