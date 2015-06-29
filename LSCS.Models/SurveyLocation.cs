using System;
using System.Device.Location;

namespace LSCS.Models
{
    public class SurveyLocation : IEquatable<SurveyLocation>
    {
        public LandDistrictDto LandDistrict { get; set; }
        public GeoCoordinate Coordinate { get; set; }
        public CivicAddress Address { get; set; }

        #region Equality Members

        public override int GetHashCode()
        {
            int hashCode = (LandDistrict != null ? LandDistrict.GetHashCode() : 0);
            hashCode = (hashCode*397) ^ (Coordinate != null ? Coordinate.GetHashCode() : 0);
            hashCode = (hashCode*397) ^ (Address != null ? Address.GetHashCode() : 0);
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SurveyLocation);
        }

        public bool Equals(SurveyLocation other)
        {
            if (other == null) return false;
            if (this == other) return true;
            return (LandDistrict == null ? (other.LandDistrict == null) : LandDistrict.Equals(other.LandDistrict)) &&
                   (Coordinate == null ? (other.Coordinate == null) : Coordinate.Equals(other.Coordinate)) &&
                   (Address == null ? (other.Address == null) : Address.Equals(other.Address));
        }

        #endregion
    }
}
