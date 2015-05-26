using System.Device.Location;

namespace LSCS.Models
{
    public class SurveyLocation
    {
        public LandDistrictDto LandDistrict { get; set; }
        public GeoCoordinate Coordinate { get; set; }
        public CivicAddress Address { get; set; }
    }
}
