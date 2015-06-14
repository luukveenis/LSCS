using System;
using System.Data.SqlClient;

namespace LSCS.Api.Controllers
{
    public class SortParameter
    {
        public SortParameter() { }

        public SortParameter(string field, string direction)
        {
            SortOrder sortDirection;
            if (!TryParseSortOrder(direction, out sortDirection))
                throw new ArgumentException("direction");

            SortDirection = sortDirection;
            Field = field;
        }

        public string Field { get; set; }
        public SortOrder SortDirection { get; set; }

        public string GetOrderByArgString()
        {
            if(Field == null)
                throw new InvalidOperationException("Sort field is null.");
            return Field + " " + GetSortDirectionStringFromEnum(SortDirection);
        }

        private string GetSortDirectionStringFromEnum(SortOrder direction)
        {
            switch (direction)
            {
                case SortOrder.Ascending:
                    return "asc";
                case SortOrder.Descending:
                    return "desc";
                default:
                    return String.Empty;
            }
        }

        private bool TryParseSortOrder(string direction, out SortOrder sortOrder)
        {
            switch (direction.Trim().ToLower())
            {
                case "asc":
                case "ascending":
                case "a":
                case "0":
                    sortOrder = SortOrder.Ascending;
                    return true;
                case "desc":
                case "descending":
                case "d":
                case "1":
                    sortOrder = SortOrder.Descending;
                    return true;
                case "":
                case null:
                    sortOrder = SortOrder.Unspecified;
                    return true;
                default:
                    sortOrder = SortOrder.Unspecified;
                    return false;
            }
        }
    }
}