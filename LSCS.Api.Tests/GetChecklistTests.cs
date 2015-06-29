
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using FakeItEasy;
using LSCS.Models;
using LSCS.Repository;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LSCS.Api.Tests
{
    [TestFixture]
    public class GetChecklistTests
    {
        private const string BaseApiUriString = "http://localhost:8081";

        [Test]
        public void GetChecklistsShouldReturnAllChecklistItemsInRepository()
        {
            // Arrange
            var repositoryChecklists = new List<ChecklistDto>
            {
                new ChecklistDto
                {
                    Id = Guid.NewGuid(),
                    Title = "ChecklistA",
                },
                new ChecklistDto
                {
                    Id = Guid.NewGuid(),
                    Title = "ChecklistB"
                },
                new ChecklistDto
                {
                    Id = Guid.NewGuid(),
                    Title = "ChecklistC"
                }
            };

            var fakeRepository = A.Fake<IChecklistRepository>();
            A.CallTo(() => fakeRepository.GetChecklists()).Returns(repositoryChecklists.AsQueryable());

            var startup = new Startup
            {
                ControllerConfiguration = new ControllerConfiguration { PageSizeLimit = 100 },
                ChecklistRepository = fakeRepository
            };
            using (WebApp.Start(BaseApiUriString, startup.Configuration))
            {
                var client = new HttpClient();

                // Act
                var response = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists")).Result;
                var returnedChecklists = JsonConvert.DeserializeObject<List<ChecklistDto>>(response.Content.ReadAsStringAsync().Result);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(returnedChecklists.Count(), Is.EqualTo(repositoryChecklists.Count));
                Assert.IsEmpty(returnedChecklists.Except(repositoryChecklists));
            }
        }

        [Test]
        public void GetChecklistsShouldReturnEmptyListWhenNoChecklistsAreAvailable()
        {
            
        }

        [Test]
        public void GetChecklistsShouldSortResultsIfGivenValidSortKey()
        {
            
        }

        [Test]
        public void GetChecklistsShouldReturn400ErrorForInvalidSortParameters()
        {
            
        }

        [Test]
        public void GetChecklistsShouldReturnNumberOfElementsEqualToSpecifiedPageSize()
        {
            
        }

        [Test]
        public void GetChecklistsShouldNotReturnMoreElementsThanApiPageSizeLimit()
        {
            
        }
    }
}
