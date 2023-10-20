﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Author.Administration;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class CheckpointCommandTests : BaseToursIntegrationTest
    {
        public CheckpointCommandTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new CheckpointDto
            {
                ID = 1,
                TourID = 1,
                OrderNumber = 1,
                Longitude = 45,
                Latitude = 45,
                Name = "Katedrala",
                Description = "Objekat se nalazi na centru Novog Sada",
                Picture = "https://www.google.com/url?sa=i&url=https%3A%2F%2Filovenovisad.com%2Fvodic%2Fsakralni-objekti%2Fkatolicka-crkva-ime-marijino%2F&psig=AOvVaw2UZQxIT-Z_WvM0CTWNA8yC&ust=1697737791159000&source=images&cd=vfe&opi=89978449&ved=0CA8QjRxqFwoTCNjjwaGUgIIDFQAAAAAdAAAAABAD"
            };

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as CheckpointDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.ID.ShouldNotBe(0);
            result.TourID.ShouldNotBe(0);
            result.OrderNumber.ShouldNotBe(0);
            result.Longitude.ShouldBe(newEntity.Longitude);
            result.Latitude.ShouldBe(newEntity.Latitude);
            result.Name.ShouldBe(newEntity.Name);
            result.Description.ShouldBe(newEntity.Description);
            result.Picture.ShouldBe(newEntity.Picture);

            // Assert - Database
            var storedEntity = dbContext.Checkpoint.FirstOrDefault(i => i.Name == newEntity.Name);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.ID);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new CheckpointDto
            {
                Name = "Test"
            };

            // Act
            var result = (ObjectResult)controller.Create(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var updatedEntity = new CheckpointDto
            {
                ID = 1,
                TourID = 1,
                OrderNumber = 1,
                Longitude = 45,
                Latitude = 45,
                Name = "Petrovaradin",
                Description = "Tvrdjava",
                Picture = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fen.wikipedia.org%2Fwiki%2FPetrovaradin_Fortress&psig=AOvVaw0IqEUr-KgumgRwJuFwJfy5&ust=1697738244154000&source=images&cd=vfe&opi=89978449&ved=0CA8QjRxqFwoTCJiG3vmVgIIDFQAAAAAdAAAAABAD"
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as CheckpointDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.ID.ShouldBe(1);
            result.TourID.ShouldNotBe(0);
            result.OrderNumber.ShouldNotBe(0);
            result.Longitude.ShouldBe(updatedEntity.Longitude);
            result.Latitude.ShouldBe(updatedEntity.Latitude);
            result.Name.ShouldBe(updatedEntity.Name);
            result.Description.ShouldBe(updatedEntity.Description);
            result.Picture.ShouldBe(updatedEntity.Picture);

            // Assert - Database
            var storedEntity = dbContext.Checkpoint.FirstOrDefault(i => i.Name == "Petrovaradin");
            storedEntity.ShouldNotBeNull();
            storedEntity.TourID.ShouldBe(updatedEntity.TourID);
            storedEntity.OrderNumber.ShouldBe(updatedEntity.OrderNumber);
            storedEntity.Longitude.ShouldBe(updatedEntity.Longitude);
            storedEntity.Latitude.ShouldBe(updatedEntity.Latitude);
            storedEntity.Description.ShouldBe(updatedEntity.Description);
            storedEntity.Picture.ShouldBe(updatedEntity.Picture);
            var oldEntity = dbContext.Checkpoint.FirstOrDefault(i => i.Name == "Katedrala");
            oldEntity.ShouldBeNull();
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new CheckpointDto
            {
                ID = -1000,
                TourID = 1,
                OrderNumber = 1,
                Longitude = 45,
                Latitude = 45,
                Name = "Test",
                Description = "Test",
                Picture = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fen.wikipedia.org%2Fwiki%2FPetrovaradin_Fortress&psig=AOvVaw0IqEUr-KgumgRwJuFwJfy5&ust=1697738244154000&source=images&cd=vfe&opi=89978449&ved=0CA8QjRxqFwoTCJiG3vmVgIIDFQAAAAAdAAAAABAD"
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            // Act
            var result = (OkResult)controller.Delete(1);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.Checkpoint.FirstOrDefault(i => i.Id == 1);
            storedCourse.ShouldBeNull();
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(-1000);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        private static CheckpointController CreateController(IServiceScope scope)
        {
            return new CheckpointController(scope.ServiceProvider.GetRequiredService<ICheckpointService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}