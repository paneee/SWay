using Google.Maps;
using Google.Maps.DistanceMatrix;
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
            List<string> addresses = new List<string>();
            addresses.Add("34-620 Jodłownik, Jodłownik 44/7");
            addresses.Add("43-150 Bieruń, Homera 30/32");
            addresses.Add("Kraków, Bojki 24");
            addresses.Add("Wrocław, Sztabowa 10");
            addresses.Add("Kasina Wielka 421");

            APIGoogle.Register("AIzaSyALT_bBzigIMao0ri0hm7kjhMuiNKmUJYU");

            List<LatLng> listLatLng = new List<LatLng>();
            foreach (string item in addresses)
            {
                listLatLng.Add(APIGoogle.GetLatLng(item));
            }

            DistanceMatrixRequest request = new DistanceMatrixRequest()
            {
                WaypointsOrigin = new List<Location>(listLatLng),
                WaypointsDestination = new List<Location>(listLatLng),
                Sensor = false
            };

            var response = new DistanceMatrixService().GetResponse(request);

            for (int i = 0; i < response.Rows.Count(); i++)
            {

                for (int j = 0; j < response.Rows[i].Elements.Count(); j++)
                {
                    Console.WriteLine(response.DestinationAddresses[i] + "  <->  " + response.DestinationAddresses[j]);
                    Console.WriteLine(response.Rows[i].Elements[j].distance.Text);
                    Console.WriteLine(response.Rows[i].Elements[j].duration.Text);
                    Console.WriteLine();
                }
            }

            Console.ReadKey();
        }
    }
}
