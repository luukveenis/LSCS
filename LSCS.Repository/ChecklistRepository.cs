using System;
using System.Linq;
using System.Threading.Tasks;
using LSCS.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace LSCS.Repository
{
    public class ChecklistRepository : MongoDbRepositoryBase, IChecklistRepository
    {
        public const string CollectionName = "Checklists";

        private readonly MongoCollection<ChecklistDto> _collection; 

        public ChecklistRepository(string connectionString, string databaseName) 
            : base(connectionString, databaseName)
        {
            _collection = Database.GetCollection<ChecklistDto>(CollectionName);
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            _collection.CreateIndex(IndexKeys<ChecklistDto>.Ascending(c => c.Title));
            _collection.CreateIndex(IndexKeys<ChecklistDto>.Ascending(c => c.CreatedAt));
        }

        public Task UpsertChecklist(ChecklistDto checklist)
        {
            return Task.Run(() => _collection.Save(checklist));
        }

        public Task DeleteChecklist(Guid checklistId)
        {
            return Task.Run(() => _collection.Remove(Query.EQ("Id", checklistId)));
        }

        public Task<ChecklistDto> GetChecklistById(Guid checklistId)
        {
            return Task.Run(() => _collection.FindOneById(checklistId));
        }

        public IQueryable<ChecklistDto> GetAllChecklists(int? limit, int? skip)
        {
            var query = _collection.FindAll();
            if (limit.HasValue) query.SetLimit(limit.Value);
            if (skip.HasValue) query.SetSkip(skip.Value);

            return query.AsQueryable();
        }
    }
}
