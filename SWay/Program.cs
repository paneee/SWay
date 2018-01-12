using Google.Maps;
using Google.Maps.Direction;
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
            List<string> Addresses = new List<string>() { "New York", "Los Angeles", "Chicago", "Minneapolis",
                                                            "Denver", "Dallas", "Seattle", "Boston", "San Francisco", "St.Louis",
                                                            "Houston", "Phoenix", "Salt Lake City" };

            APIGoogle.Register("AIzaSyCkYDMEPLWCFvq3Oi-LJyEsMuh_06Fk62g");

            List<LatLng> listLatLng = new List<LatLng>();
            foreach (string item in Addresses)
            {
                listLatLng.Add(APIGoogle.GetLatLng(item));
            }

            DirectionRequest directionRequest = new DirectionRequest();
            DirectionService directionService = new DirectionService();

            long[,] CityDistanceMatrix = new long[Addresses.Count, Addresses.Count];

            for (int i = 0; i < Addresses.Count; i++)
            {

                for (int j = 0; j < Addresses.Count; j++)
                {
                    directionRequest.Origin = Addresses[i];
                    directionRequest.Sensor = false; 
                    {
                        directionRequest.Destination = Addresses[j];
                        var ttt = directionService.GetResponse(directionRequest);
                        CityDistanceMatrix[i, j] = directionService.GetResponse(directionRequest).Routes[0].Legs[0].Distance.Value / 1000;
                    };
                }
            }

            int NumRoutes = 1;    // The number of routes, which is 1 in the TSP.
                                  // Nodes are indexed from 0 to tsp_size - 1. The depot is the starting node of the route.
            int Depot = 0;
            int TspSize = Addresses.Count;

            if (TspSize > 0)
            {

                RoutingModel routing = new RoutingModel(TspSize, NumRoutes, Depot);

                RoutingSearchParameters search_parameters = RoutingModel.DefaultSearchParameters();

                CityDistance dist_between_nodes = new CityDistance(CityDistanceMatrix, Addresses);
                routing.SetArcCostEvaluatorOfAllVehicles(dist_between_nodes);
                //routing.SetCost(dist_between_nodes);

                Demand demands_at_locations = new Demand(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });


                search_parameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
                Assignment solution = routing.SolveWithParameters(search_parameters);

                Console.WriteLine("Status = {0}", routing.Status());
                if (solution != null)
                {
                    // Solution cost.
                    Console.WriteLine("Suma [km]= {0}", solution.ObjectiveValue() / 1000);
                    // Inspect solution.
                    // Only one route here; otherwise iterate from 0 to routing.vehicles() - 1
                    int route_number = 0;
                    for (long node = routing.Start(route_number);
                         !routing.IsEnd(node);
                         node = solution.Value(routing.NextVar(node)))
                    {
                        Console.Write("{0} \n", Addresses[(int)node]);
                    }
                    Console.WriteLine(Addresses[0]);
                }
            }
            Console.ReadKey();
        }
    }
}
