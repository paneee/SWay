using Google.Maps;
using Google.Maps.DistanceMatrix;
using Google.OrTools.ConstraintSolver;
using SWay.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWay
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> addresses = new List<string>();

            addresses.Add("34-620 Jodłownik, Jodłownik 44/7");
            addresses.Add("43-150 Bieruń, Homera 30/32");
            addresses.Add("Kraków, Bojki 24");
            addresses.Add("Wrocław, Sztabowa 10");
            addresses.Add("Kasina Wielka 421");
            addresses.Add("Poznań");
            addresses.Add("Warszawa");
            addresses.Add("Zabrze");
            addresses.Add("Limanowa");

            APIGoogle.Register("AIzaSyD-ZnOQcxjPG7KXOR9l5OYKPhyxuzQChCc");

            List<LatLng> listLatLng = new List<LatLng>();
            foreach (string item in addresses)
            {
                listLatLng.Add(APIGoogle.GetLatLng(item));
            }

            DistanceMatrixRequest request = new DistanceMatrixRequest()
            {
                WaypointsOrigin = new List<Location>(listLatLng),
                WaypointsDestination = new List<Location>(listLatLng),
                Mode = TravelMode.driving,
                Units = Units.metric
            };

            var response = new DistanceMatrixService().GetResponse(request);

            long[,] CityDistanceMatrix = new long[addresses.Count, addresses.Count];

            for (int i = 0; i < response.Rows.Count(); i++)
            {

                for (int j = 0; j < response.Rows[i].Elements.Count(); j++)
                {
                    CityDistanceMatrix[i, j] = response.Rows[i].Elements[j].distance.Value;
                }
            }

            int NumRoutes = 1;    // The number of routes, which is 1 in the TSP.
                                  // Nodes are indexed from 0 to tsp_size - 1. The depot is the starting node of the route.
            int Depot = 0;
            int TspSize = addresses.Count;

            if (TspSize > 0)
            {

                RoutingModel routing = new RoutingModel(TspSize, NumRoutes, Depot);

                RoutingSearchParameters search_parameters = RoutingModel.DefaultSearchParameters();

                CityDistance dist_between_nodes = new CityDistance(CityDistanceMatrix, addresses);
                routing.SetCost(dist_between_nodes);

                search_parameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
                Assignment solution = routing.SolveWithParameters(search_parameters);

                Console.WriteLine("Status = {0}", routing.Status());
                if (solution != null)
                {
                    // Solution cost.
                    Console.WriteLine("Cost = {0}", solution.ObjectiveValue() / 1000);
                    // Inspect solution.
                    // Only one route here; otherwise iterate from 0 to routing.vehicles() - 1
                    int route_number = 0;
                    for (long node = routing.Start(route_number);
                         !routing.IsEnd(node);
                         node = solution.Value(routing.NextVar(node)))
                    {
                        Console.Write("{0} -> ", addresses[(int)node]);
                    }
                    Console.WriteLine(addresses[0]);
                }
            }
            Console.ReadKey();
        }
    }
}
