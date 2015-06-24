using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LSCS.Api.Exceptions;
using LSCS.Models;
using LSCS.Repository;

namespace LSCS.Api.Controllers
{
    [RoutePrefix("api/checklists")]
    public class ChecklistController : ControllerBase
    {
        private readonly IChecklistRepository _repository;

        public ChecklistController(IChecklistRepository repository, IControllerConfiguration config)
        {
            _repository = repository;
            PageSizeLimit = config.PageSizeLimit;
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetChecklists(int pageNumber = 1, int pageSize = 0, string sortField = "Title", string sortDirection = "asc")
        {
            if (pageSize == 0 || pageSize > PageSizeLimit)
            {
                pageSize = PageSizeLimit;
            }

            var results = _repository.GetChecklists();

            try
            {
                var sortParam = new SortParameter(sortField, sortDirection);
                SortCollection(results, sortParam);
            }
            catch (Exception e)
            {
                throw new InvalidQueryException("Inavlid sort parameter(s).");
            }
            var pagedResults = PageCollection(results, pageNumber, pageSize);
          
            var response = Request.CreateResponse(HttpStatusCode.OK, results);
            CreatePagedResponseHeader(response, pageNumber, pageSize, pagedResults.Count());
            return response;
        }

        [Route("")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddChecklist(ChecklistDto newChecklist)
        {
            if(newChecklist == null)
                throw new InvalidQueryException("Received an invalid checklist format.");
            if(newChecklist.Id.HasValue)
                throw new InvalidQueryException("Checklist may not specify an ID.");

            InitializeNewChecklist(newChecklist);        

            await _repository.UpsertChecklist(newChecklist);
            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, newChecklist.Id.ToString());
            return response;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ChecklistDto> GetChecklistById(Guid id)
        {
            var checklist = await _repository.GetChecklistById(id);
            if (checklist == null)
                throw new DocumentNotFoundException(id.ToString());

            return checklist;
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateChecklistById(Guid id, ChecklistDto updatedChecklist)
        {
            if(updatedChecklist == null)
                throw new InvalidQueryException("Received an invalid checklist format.");

            if(!await _repository.ChecklistExists(id))
                throw new DocumentNotFoundException(id.ToString());

            updatedChecklist.Id = id;
            await _repository.UpsertChecklist(updatedChecklist);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteChecklist(Guid id)
        {
            if (! await _repository.ChecklistExists(id))
                throw new DocumentNotFoundException(id.ToString());

            await _repository.DeleteChecklist(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        #region

        private void InitializeNewChecklist(ChecklistDto checklist)
        {
            checklist.Id = Guid.NewGuid();
            checklist.Nonce = Guid.NewGuid();

            DateTime currentTime = DateTime.UtcNow;
            checklist.CreatedAt = currentTime;
            checklist.LastModified = currentTime;
        }

        #endregion
    }
}
