using Google.Maps;
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
            var response = GeocodingRequest(address);
            var result = response.Results.First();

            LatLng ret = new LatLng(result.Geometry.Location.Latitude, result.Geometry.Location.Longitude);

            return ret;
        }

        private static GeocodeResponse GeocodingRequest(string address)
        {
            var req = new GeocodingRequest();
            req.Address = address;
            req.Sensor = false;
            var response = new GeocodingService().GetResponse(req);

            return response;
        }
    }
}
