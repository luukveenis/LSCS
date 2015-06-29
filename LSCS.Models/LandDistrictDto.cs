using System;
using System.Device.Location;

namespace LSCS.Models
{
    public class LandDistrictDto : IEquatable<LandDistrictDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NtsMap { get; set; }
        public GeoCoordinate Coordinate { get; set; }

        #region Equality Members

        public override int GetHashCode()
        {
            int hashCode = Id.GetHashCode();
            hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode*397) ^ (NtsMap != null ? NtsMap.GetHashCode() : 0);
            hashCode = (hashCode*397) ^ (Coordinate != null ? Coordinate.GetHashCode() : 0);
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LandDistrictDto);
        }

        public bool Equals(LandDistrictDto other)
        {
            if (other == null) return false;
            if (this == other) return true;
            return Id == other.Id &&
                   String.Equals(Name, other.Name) &&
                   String.Equals(NtsMap, other.NtsMap) &&
                   (Coordinate == null) ? (other.Coordinate == null) : Coordinate.Equals(other.Coordinate);
        }

        #endregion
    }
}
