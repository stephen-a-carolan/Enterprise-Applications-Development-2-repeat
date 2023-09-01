using System;
using Xunit;
using Moq;
using WeatherService.Controllers;
using WeatherService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace WeatherService.Controllers
{
    public class WeatherControllerTests : Controller
    {
        [Fact]
        public void Get_ReturnsWeatherInfo_WhenCityExists()
        {
            // Arrange
            var mockSet = new Mock<DbSet<WeatherInfo>>();
            var weatherInfo = new WeatherInfo
            {
                City = "Dublin",
                CurrentCondition = "Cloudy",
                MaxTemp = 20,
                MinTemp = 12,
                WindDirection = "East",
                WindSpeed = 15,
                Outlook = "Sunny"
            };

            var mockContext = new Mock<WeatherContext>();
            mockContext.Setup(m => m.WeatherInfos).Returns(mockSet.Object);

            var controller = new WeatherController(mockContext.Object);

            // Act
            var result = controller.Get("Dublin");

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<WeatherInfo>(actionResult.Value);
            Assert.Equal("Dublin", model.City);
        }

        [Fact]
        public void Get_ReturnsNotFound_WhenCityDoesNotExist()
        {
            // Arrange
            var mockSet = new Mock<DbSet<WeatherInfo>>();
            var mockContext = new Mock<WeatherContext>();
            mockContext.Setup(m => m.WeatherInfos).Returns(mockSet.Object);
            var controller = new WeatherController(mockContext.Object);

            // Act
            var result = controller.Get("NonExistentCity");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}

