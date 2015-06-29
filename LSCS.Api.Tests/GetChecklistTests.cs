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
            // Arrange 
            var fakeRepository = A.Fake<IChecklistRepository>();
            A.CallTo(() => fakeRepository.GetChecklists()).Returns(new List<ChecklistDto>().AsQueryable());

            var startup = new Startup
            {
                ControllerConfiguration = new ControllerConfiguration {PageSizeLimit = 100},
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
                Assert.NotNull(returnedChecklists);
                Assert.IsEmpty(returnedChecklists);
            }
        }

        [Test]
        public void GetChecklistsShouldSortResultsIfGivenValidSortKey()
        {
            // Arrange
            var repositoryChecklists = new List<ChecklistDto>()
            {
                new ChecklistDto
                {
                    Title = "C",
                    FileNumber = 3,
                    CreatedAt = new DateTime(2015, 3, 1),
                    LastModified = new DateTime(2015, 3, 1)
                },
                new ChecklistDto
                {
                    Title = "B",
                    FileNumber = 2,
                    CreatedAt = new DateTime(2015, 2, 1),
                    LastModified = new DateTime(2015, 2, 1)
                },
                new ChecklistDto
                {
                    Title = "A",
                    FileNumber = 1,
                    CreatedAt = new DateTime(2015, 1, 1),
                    LastModified = new DateTime(2015, 1, 1)
                }
            };

            var sortedByTitleAsc = repositoryChecklists.OrderBy(c => c.Title);
            var sortedByTitleDesc = repositoryChecklists.OrderByDescending(c => c.Title);
            var sortedByFileNumAsc = repositoryChecklists.OrderBy(c => c.FileNumber);
            var sortedByFileNumDesc = repositoryChecklists.OrderByDescending(c => c.FileNumber);
            var sortedByCreatedAtAsc = repositoryChecklists.OrderBy(c => c.CreatedAt);
            var sortedByCreatedAtDesc = repositoryChecklists.OrderByDescending(c => c.CreatedAt);
            var sortedByLastModifiedAsc = repositoryChecklists.OrderBy(c => c.LastModified);
            var sortedByLastModifiedDesc = repositoryChecklists.OrderByDescending(c => c.LastModified);


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
                var sortedByTitleAscResponse = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?sortField=Title&sortDirection=asc")).Result;
                var returnedSortedByTitleAsc = JsonConvert.DeserializeObject<List<ChecklistDto>>(sortedByTitleAscResponse.Content.ReadAsStringAsync().Result);

                var sortedByTitleDescResponse = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?sortField=Title&sortDirection=desc")).Result;
                var returnedSortedByTitleDesc = JsonConvert.DeserializeObject<List<ChecklistDto>>(sortedByTitleDescResponse.Content.ReadAsStringAsync().Result);

                var sortedByFileNumAscResponse = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?sortField=FileNumber&sortDirection=a")).Result;
                var returnedSortedByFileNumAsc = JsonConvert.DeserializeObject<List<ChecklistDto>>(sortedByFileNumAscResponse.Content.ReadAsStringAsync().Result);

                var sortedByFileNumDescResponse = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?sortField=FileNumber&sortDirection=d")).Result;
                var returnedSortedByFileNumDesc = JsonConvert.DeserializeObject<List<ChecklistDto>>(sortedByFileNumDescResponse.Content.ReadAsStringAsync().Result);

                var sortedByCreatedAtAscResponse = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?sortField=CreatedAt&sortDirection=ascending")).Result;
                var returnedSortedByCreatedAtAsc = JsonConvert.DeserializeObject<List<ChecklistDto>>(sortedByCreatedAtAscResponse.Content.ReadAsStringAsync().Result);

                var sortedByCreatedAtDescResponse = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?sortField=CreatedAt&sortDirection=descending")).Result;
                var returnedSortedByCreatedAtDesc = JsonConvert.DeserializeObject<List<ChecklistDto>>(sortedByCreatedAtDescResponse.Content.ReadAsStringAsync().Result);

                var sortedByLastModifiedAscResponse = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?sortField=LastModified&sortDirection=Ascending")).Result;
                var returnedSortedByLastModifiedAsc = JsonConvert.DeserializeObject<List<ChecklistDto>>(sortedByLastModifiedAscResponse.Content.ReadAsStringAsync().Result);

                var sortedByLastModifiedDescResponse = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?sortField=LastModified&sortDirection=Descending")).Result;
                var returnedSortedByLastModifiedDesc= JsonConvert.DeserializeObject<List<ChecklistDto>>(sortedByLastModifiedDescResponse.Content.ReadAsStringAsync().Result);

                // Assert
                Assert.That(sortedByTitleAscResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(sortedByTitleAsc.SequenceEqual(returnedSortedByTitleAsc));

                Assert.That(sortedByTitleDescResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(sortedByTitleDesc.SequenceEqual(returnedSortedByTitleDesc));

                Assert.That(sortedByFileNumAscResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(sortedByFileNumAsc.SequenceEqual(returnedSortedByFileNumAsc));

                Assert.That(sortedByFileNumDescResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(sortedByFileNumDesc.SequenceEqual(returnedSortedByFileNumDesc));

                Assert.That(sortedByCreatedAtAscResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(sortedByCreatedAtAsc.SequenceEqual(returnedSortedByCreatedAtAsc));

                Assert.That(sortedByCreatedAtDescResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(sortedByCreatedAtDesc.SequenceEqual(returnedSortedByCreatedAtDesc));

                Assert.That(sortedByLastModifiedAscResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(sortedByLastModifiedAsc.SequenceEqual(returnedSortedByLastModifiedAsc));

                Assert.That(sortedByLastModifiedDescResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(sortedByLastModifiedDesc.SequenceEqual(returnedSortedByLastModifiedDesc));
            }

        }

        [Test]
        public void GetChecklistsShouldReturn400ErrorForInvalidSortParameters()
        {
            // Arrange
            var fakeRepository = A.Fake<IChecklistRepository>();
            A.CallTo(() => fakeRepository.GetChecklists()).Returns(new List<ChecklistDto>().AsQueryable());

            var startup = new Startup
            {
                ControllerConfiguration = new ControllerConfiguration { PageSizeLimit = 100 },
                ChecklistRepository = fakeRepository
            };
            using (WebApp.Start(BaseApiUriString, startup.Configuration))
            {
                var client = new HttpClient();

                // Act
                var invalidFieldResponse = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?sortField=NotAField&sortDirection=asc")).Result;
                var invalidDirectionResponse = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?sortField=Title&sortDirection=badDirection")).Result;

                // Assert
                Assert.That(invalidFieldResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(invalidDirectionResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        [Test]
        public void GetChecklistsShouldReturnNumberOfElementsEqualToSpecifiedPageSize()
        {
            // Arrange
            var repositoryChecklists = new List<ChecklistDto>
            {
                new ChecklistDto { Title = "A" },
                new ChecklistDto { Title = "B" },
                new ChecklistDto { Title = "C" },
                new ChecklistDto { Title = "D" },
                new ChecklistDto { Title = "E" }
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
                var responseOne = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?pageNumber=1&pageSize=2")).Result;
                var returnedChecklistsOne = JsonConvert.DeserializeObject<List<ChecklistDto>>(responseOne.Content.ReadAsStringAsync().Result);

                var responseTwo = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?pageNumber=2&pageSize=2")).Result;
                var returnedChecklistsTwo = JsonConvert.DeserializeObject<List<ChecklistDto>>(responseTwo.Content.ReadAsStringAsync().Result);

                var responseThree = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?pageNumber=3&pageSize=2")).Result;
                var returnedChecklistsThree = JsonConvert.DeserializeObject<List<ChecklistDto>>(responseThree.Content.ReadAsStringAsync().Result);

                var responseFour = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?pageNumber=4&pageSize=2")).Result;
                var returnedChecklistsFour = JsonConvert.DeserializeObject<List<ChecklistDto>>(responseFour.Content.ReadAsStringAsync().Result);

                // Assert
                Assert.That(responseOne.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(returnedChecklistsOne.SequenceEqual(repositoryChecklists.GetRange(0, 2)));

                Assert.That(responseTwo.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(returnedChecklistsTwo.SequenceEqual(repositoryChecklists.GetRange(2,2)));

                Assert.That(responseThree.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.True(returnedChecklistsThree.SequenceEqual(repositoryChecklists.GetRange(4, 1)));

                Assert.That(responseFour.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.IsEmpty(returnedChecklistsFour);
            }
        }

        [Test]
        public void GetChecklistsShouldNotReturnMoreElementsThanApiPageSizeLimit()
        {
            // Arrange

            int pageSizeLimit = 3;

            var repositoryChecklists = new List<ChecklistDto>
            {
                new ChecklistDto { Title = "A" },
                new ChecklistDto { Title = "B" },
                new ChecklistDto { Title = "C" },
                new ChecklistDto { Title = "D" },
                new ChecklistDto { Title = "E" }
            };

            var fakeRepository = A.Fake<IChecklistRepository>();
            A.CallTo(() => fakeRepository.GetChecklists()).Returns(repositoryChecklists.AsQueryable());

            var startup = new Startup
            {
                ControllerConfiguration = new ControllerConfiguration { PageSizeLimit = pageSizeLimit },
                ChecklistRepository = fakeRepository
            };
            using (WebApp.Start(BaseApiUriString, startup.Configuration))
            {
                var client = new HttpClient();

                // Act
                var response = client.GetAsync(new Uri(BaseApiUriString + "/api/checklists?pageNumber=1&pageSize=10")).Result;
                var returnedChecklists = JsonConvert.DeserializeObject<List<ChecklistDto>>(response.Content.ReadAsStringAsync().Result);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(returnedChecklists.Count, Is.EqualTo(pageSizeLimit));
            }
        }
    }
}
