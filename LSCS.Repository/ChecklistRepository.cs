using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using LSCS.Models;
using LSCS.Repository.Exceptions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace LSCS.Repository
{
    public class ChecklistRepository : MongoDbRepositoryBase, IChecklistRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ChecklistRepository));

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
            try
            {
                _collection.CreateIndex(IndexKeys<ChecklistDto>.Ascending(c => c.Title));
                _collection.CreateIndex(IndexKeys<ChecklistDto>.Ascending(c => c.CreatedAt));
            }
            catch(Exception ex)
            {
                string errorMessage = "Encountered an error while creating collection indexes.";
                Log.Fatal(errorMessage);
                throw new RepositoryException(errorMessage, ex);
            }
        }

        public Task<bool> ChecklistExists(Guid checklistId)
        {           
            return Task.Run(() =>
            {
                try
                {
                    return _collection.Find(Query.EQ("Id", checklistId)).Any();
                }
                catch(Exception ex)
                {
                    string errorMessage = String.Format(CultureInfo.InvariantCulture,
                        "Encountered an error while searching for checklist with Id = {0};", checklistId);
                    Log.Error(errorMessage);
                    throw new RepositoryException(errorMessage, ex);
                }
            });
        }

        public Task UpsertChecklist(ChecklistDto checklist)
        {
            return Task.Run(() =>
            {
                try
                {
                    return _collection.Save(checklist);
                }
                catch(Exception ex)
                {
                    string errorMessage = String.Format(CultureInfo.InvariantCulture,
                        "Encountered an error while upserting a checklist with Id = {0};", checklist.Id);
                    Log.Error(errorMessage);
                    throw new RepositoryException(errorMessage, ex); 
                }
            });
        }

        public Task DeleteChecklist(Guid checklistId)
        {
            return Task.Run(() =>
            {
                try
                {
                    return _collection.Remove(Query.EQ("Id", checklistId));
                }
                catch(Exception ex)
                {
                    string errorMessage = String.Format(CultureInfo.InvariantCulture,
                        "Encountered an error while deleting a checklist with Id = {0};", checklistId);
                    Log.Error(errorMessage);
                    throw new RepositoryException(errorMessage, ex); 
                }
            });
        }

        public Task<ChecklistDto> GetChecklistById(Guid checklistId)
        {
            return Task.Run(() =>
            {
                try
                {
                    return _collection.FindOneById(checklistId);
                }
                catch(Exception ex)
                {
                    string errorMessage = String.Format(CultureInfo.InvariantCulture,
                        "Encountered an error while upserting a checklist with Id = {0}", checklistId);
                    Log.Error(errorMessage);
                    throw new RepositoryException(errorMessage, ex); 
                }
            });
        }

        public IQueryable<ChecklistDto> GetChecklists()
        {
            try
            {
                var query = _collection.FindAll();
                return query.AsQueryable();
            }
            catch(Exception ex)
            {
                string errorMessage = "Encountered an error while retrieving list of checklists.";
                Log.Error(errorMessage);
                throw new RepositoryException(errorMessage, ex);
            }
        }
    }
}
