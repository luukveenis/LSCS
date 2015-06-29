using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LSCS.Api.Controllers
{
    public class ControllerBase : ApiController
    {
        protected int PageSizeLimit { get; set; }

        protected IQueryable<T> SortCollection<T>(IQueryable<T> collection, SortParameter sortParam, IList<string> validSortFields = null)
        {
            if (validSortFields != null)
            {
                if (!validSortFields.Contains(sortParam.Field))
                    throw new ArgumentException("The sort paramater specifies an invalid field: " + sortParam.Field);
            }
            return collection.OrderBy(sortParam.GetOrderByArgString());
        }

        protected IQueryable<T> PageCollection<T>(IQueryable<T> collection, int pageNumber, int pageSize)
        {
            if (collection == null) 
                return null;
            if (pageNumber <= 0)
                throw new ArgumentException("Parameter \"pageNumber\" must be greater than 0.");
            if (pageSize <= 0)
                throw new ArgumentException("Parameter \"pageSize\" must be greater than 0.");

            int skip = (pageNumber - 1)*pageSize;

            if (skip > 0)
            {
                collection = collection.Skip(skip);
            }  
            collection = collection.Take(pageSize);
            return collection;
        }

        protected void CreatePagedResponseHeader(HttpResponseMessage response, int pageNumber, int pageSize, int totalElements)
        {
            if (response == null)
                throw new ArgumentNullException("response");
            if (pageNumber <= 0)
                throw new ArgumentException("Parameter \"pageNumber\" must be greater than 0.");
            if (pageSize <= 0)
                throw new ArgumentException("Parameter \"pageSize\" must be greater than 0.");
            if (totalElements < 0)
                throw new ArgumentException("Parameter \"totalElements\" cannot be a negative number.");

            response.Headers.Add(
                "Link", 
                String.Join(", ", GetLinkElements(
                    pageNumber,
                    pageSize,
                    totalElements
                )));   
        }

        private IEnumerable<string> GetLinkElements(int pageNumber, int pageSize, int totalElements)
        {
            int totalPages = totalElements == 0 ? 1 : (int)Math.Ceiling(totalElements / (float)pageSize);

            var linkElements = new List<string>();
            linkElements.Add(BuildLinkElement(Request.RequestUri, 1, pageSize, "first"));
            linkElements.Add(BuildLinkElement(Request.RequestUri, totalPages, pageSize, "last"));

            if (pageNumber > 1)
            {
                linkElements.Add(BuildLinkElement(Request.RequestUri, pageNumber - 1, pageSize, "prev"));
            }

            if (pageNumber != totalPages)
            {
                linkElements.Add(BuildLinkElement(Request.RequestUri, pageNumber + 1, pageSize, "next"));
            }

            return linkElements;
        } 

        private string BuildLinkElement(Uri baseUri, int pageNumber, int pageSize, string relName)
        {
            const string linkFormat = "<{0}>; rel\"{1}\"";
            return String.Format(CultureInfo.InvariantCulture, 
                linkFormat,
                BuildLinkUri(baseUri, pageNumber, pageSize),
                relName
                );
        }

        private string BuildLinkUri(Uri baseUri, int pageNumber, int pageSize)
        {
            var query = HttpUtility.ParseQueryString(String.Empty);
            query["pageNo"] = pageNumber.ToString();
            query["pageSize"] = pageSize.ToString();
            var builder = new UriBuilder(baseUri) {Query = query.ToString()};
            return builder.ToString();
        }
    }
}