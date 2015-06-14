using System;
using System.Linq;
using System.Threading.Tasks;
using LSCS.Models;

namespace LSCS.Repository
{
    public interface IChecklistRepository
    {
        Task<bool> ChecklistExists(Guid checklistId);
        Task UpsertChecklist(ChecklistDto checklist);
        Task DeleteChecklist(Guid checklistId);
        Task<ChecklistDto> GetChecklistById(Guid checklistId);
        IQueryable<ChecklistDto> GetChecklists();
    }
}
