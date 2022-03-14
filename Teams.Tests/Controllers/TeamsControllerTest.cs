using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Teams.Controllers;
using Teams.Data;
using Teams.Models;
using Xunit;

namespace Teams.Tests.Controllers
{
    public class TeamsControllerTest : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<TeamsContext> _contextOptions;

        private readonly TeamsContext _context;
        private readonly TeamsController _controller;

        public static IEnumerable<object[]> InvalidGuid =>
            new List<object[]>
            {
                    new object[] { Guid.Empty },
                    new object[] { Guid.NewGuid() },
            };
        public TeamsControllerTest()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<TeamsContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            _context = new TeamsContext(_contextOptions);
            _context.Team.AddRange(GetTeams());

            _context.Database.EnsureCreated();
            _context.SaveChanges();

            _controller = new TeamsController(_context);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }


        [Fact]
        public async void Index_ReturnsViewResult_WithTeamsList()
        {
            // Act
            var result = await _controller.Index("", "", 0);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PaginatedList<Team>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public async void Details_ReturnsViewResult_WithTeam_WhenIdExists()
        {
            // Arrange
            var expectedModel = _context.Team.First();

            // Act
            var result = await _controller.Details(expectedModel.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Team>(viewResult.ViewData.Model);
            Assert.Equal(expectedModel, model);
        }

        [Theory, MemberData(nameof(InvalidGuid))]
        public async void Details_ReturnsNotFound_WhenIdNotExistsOrItsNull(Guid id)
        {
            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetEdit_ReturnsViewResult_WithSelectedTeam_WhenIdExists()
        {
            // Arrange
            var expectedModel = _context.Team.First();

            // Act
            var result = await _controller.Edit(expectedModel.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Team>(viewResult.ViewData.Model);
            Assert.Equal(expectedModel, model);
        }

        [Fact]
        public async void PostEdit_ReturnsNotFound_WhenIDsAreNotEquals()
        {
            // Arrange
            var team = await _context.Team.FirstOrDefaultAsync();

            // Act
            var result = await _controller.Edit(Guid.NewGuid(), team);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void PostEdit_ReturnsViewResult_ToIndex_WhenTeamIsValid()
        {
            // Arrange
            var team = await _context.Team.FirstOrDefaultAsync();
            _context.Entry(team!).State = EntityState.Detached;

            // Act
            var result = await _controller.Edit(team!.Id, team);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }        
        
        [Fact]
        public async void PostEdit_ReturnsViewResult_ToEdit_WhenTeamIsNotValid()
        {
            // Arrange
            var team = await _context.Team.FirstOrDefaultAsync();
            _controller.ModelState.AddModelError("TeamLogoUrl", "Property is null");

            // Act
            var result = await _controller.Edit(team!.Id, team);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Team>(viewResult.ViewData.Model);
            Assert.Equal(team, model);
        }

        [Fact]
        public async void GetDelete_ReturnsNotFound_WhenIdIsNullOrTeamNotExists()
        {
            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetDelete_ReturnsViewResult_WithSelectedTeam_WhenTeamIsValid()
        {
            // Arrange
            var team = await _context.Team.FirstOrDefaultAsync();

            // Act
            var result = await _controller.Delete(team!.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Team>(viewResult.ViewData.Model);
            Assert.Equal(team, model);
        }

        [Fact]
        public async void DeleteConfirmed_RedirectsToIndex()
        {
            // Arrange
            var team = await _context.Team.FirstOrDefaultAsync();

            // Act
            var result = await _controller.DeleteConfirmed(team!.Id);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }
        
        [Fact]
        public async void DeleteConfirmed_RemovesTeamFromDb()
        {
            // Arrange
            var team = await _context.Team.FirstOrDefaultAsync();

            // Act
            _ = await _controller.DeleteConfirmed(team!.Id);
            var searched = await _context.Team.FindAsync(team.Id);

            // Assert
            Assert.Null(searched);
        }

        private IEnumerable<Team> GetTeams()
        {
            return new List<Team>
            {
                new Team { Id = Guid.NewGuid(), Name = "SomeName",  League = "Some League", TeamLogoUrl = "http://somelogo.com"},
                new Team { Id = Guid.NewGuid(), Name = "SomeName2",  League = "Some League2", TeamLogoUrl = "http://somelogo2.com"},
                new Team { Id = Guid.NewGuid(), Name = "SomeName3",  League = "Some League3", TeamLogoUrl = "http://somelogo3.com"},
            };
        }
    }
}
