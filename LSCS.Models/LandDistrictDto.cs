using System;
using System.Device.Location;

namespace LSCS.Models
{
    public class LandDistrictDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NtsMap { get; set; }
        public GeoCoordinate Coordinate { get; set; }
    }
}
