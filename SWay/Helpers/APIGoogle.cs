using Google.Maps;
using Google.Maps.Direction;
using Google.Maps.DistanceMatrix;
using Google.Maps.Geocoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWay.Helpers
{
    public static class APIGoogle
    {
        public static void Register(string apiKey)
        {
            GoogleSigned.AssignAllServices(new GoogleSigned(apiKey));
        }

        public static LatLng GetLatLng(string address)
        {
            var response = Geocoding(address);
            var result = response.Results.First();

            LatLng ret = new LatLng(result.Geometry.Location.Latitude, result.Geometry.Location.Longitude);

            return ret;
        }

        private static GeocodeResponse Geocoding(string address)
        {
            var req = new GeocodingRequest();
            req.Address = address;
            req.Sensor = false;
            var response = new GeocodingService().GetResponse(req);

            return response;
        }

        public static DistanceMatrixResponse DistanceMatrix(DistanceMatrixRequest request)
        {
            if (request.WaypointsDestination.Count < 10)
            {
                DistanceMatrixResponse response = new DistanceMatrixService().GetResponse(request);
                return response;
            }
            else
            {
                List<DistanceMatrixResponse> tempList = new List<DistanceMatrixResponse>();

                DirectionService ds = new DirectionService();
                DirectionRequest dr;
                DirectionResponse dre;

                DistanceMatrixRequest _request;
                DistanceMatrixResponse _response;

                for (int i = 0; i < request.WaypointsDestination.Count; i++)
                {
                    for (int j = 0; j < request.WaypointsDestination.Count; j++)
                    {
                        //_request = new DistanceMatrixRequest()
                        //{
                        //    WaypointsOrigin = new List<Location>() { request.WaypointsOrigin[i], request.WaypointsOrigin[j] },
                        //    WaypointsDestination = new List<Location> { request.WaypointsDestination[i], request.WaypointsDestination[j] },
                        //    Mode = TravelMode.driving,
                        //    Units = Units.metric
                        //};

                        //_response = new DistanceMatrixService().GetResponse(_request);
                        //tempList.Add(_response);

                        dr = new DirectionRequest()
                        {
                            Origin = new Location("Kraków"),
                            Destination = new Location("Limanowa")
                        };

                        //DirectionRequest directionsRequest = new DirectionRequest()
                        //{
                        //    Origin = "Surrey, UK",
                        //    Destination = "London, UK",
                        //}

                        //DirectionResponse directionsResponse = GoogleMapsApi.GoogleMaps.Directions.Query(directionsRequest);
                        
                        dre = ds.GetResponse(dr);

                    }
                }

                //var temp = ds.GetResponse(DirectionRequest request);
                //ds.GetResponse

            }
            //DistanceMatrixRequest request;



            return null;
        }

    }
}

