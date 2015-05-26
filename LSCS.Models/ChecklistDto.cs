using System.Collections.Generic;

namespace LSCS.Models
{
    public class ChecklistDto : DtoBase
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
    }
}
