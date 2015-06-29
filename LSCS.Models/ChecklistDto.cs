using System;
using System.Collections.Generic;
using System.Linq;

namespace LSCS.Models
{
    public class ChecklistDto : DtoBase, IEquatable<ChecklistDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int FileNumber { get; set; }
        public SurveyLocation SurveyLocation { get; set; }
        public IList<ChecklistItem> Items { get; set; }

        public ChecklistDto()
        {
            Items = new List<ChecklistItem>();
        }

        #region Equality Members

        public override int GetHashCode()
        {
            int hashCode = FileNumber;
            hashCode = (hashCode*397) ^ (Title != null ? Title.GetHashCode() : 0);
            hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
            hashCode = (hashCode*397) ^ (SurveyLocation != null ? SurveyLocation.GetHashCode() : 0);
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChecklistDto);
        }

        public bool Equals(ChecklistDto other)
        {
            if (other == null) return false;
            if (this == other) return true;
            return String.Equals(Title, other.Title) &&
                    String.Equals(Description, other.Description) &&
                    FileNumber == other.FileNumber &&
                    (SurveyLocation == null ? (other.SurveyLocation == null) : SurveyLocation.Equals(other.SurveyLocation)) &&
                    (Items == null ? (other.Items == null) : Items.SequenceEqual(other.Items));
        }

        #endregion
    }
}
