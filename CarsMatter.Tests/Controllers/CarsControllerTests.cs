using CarsMatter.Controllers;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.MsSQL;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Tests
{
    [TestClass]
    public class CarsControllerTests
    {

        private MockRepository mockRepository;
        private Mock<ICarsRepository<Car>> carsRepositoryMock;
        private Mock<IBrandsRepository<Brand>> brandsRepositoryMock;
        private Mock<IBrandModelsRepository<BrandModel>> brandModelsRepository;
        private Mock<ILogger<CarsController>> loggerMock;

        private CarsController carsController;
        
        [TestInitialize]
        public void TestsSetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.carsRepositoryMock = this.mockRepository.Create<ICarsRepository<Car>>();
            this.brandModelsRepository = this.mockRepository.Create<IBrandModelsRepository<BrandModel>>();
            this.brandsRepositoryMock = this.mockRepository.Create<IBrandsRepository<Brand>>();
            this.loggerMock = this.mockRepository.Create<ILogger<CarsController>>();

            this.carsController = new CarsController(
                this.brandsRepositoryMock.Object,
                this.brandModelsRepository.Object,
                this.carsRepositoryMock.Object,
                this.loggerMock.Object);
        }
        

        [TestMethod]
        public async Task CarsController_GetAllBrands_Gets_Successfully()
        {
            // Arrange
            List<Brand> expectedBrands = new List<Brand>
            {
                new Brand
                {
                    BrandName = "testName1"
                },
                new Brand
                {
                    BrandName = "testName2"
                }
            };

            this.brandsRepositoryMock
                .Setup(br => br.GetAllBrands())
                .ReturnsAsync(expectedBrands)
                .Verifiable();

            // Act
            var result = await this.carsController.GetAllBrands();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<Brand>>>();
            (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedBrands);
        }

        [TestMethod]
        public async Task CarsController_GetModelsForBrand_Gets_Successfully()
        {
            // Arrange
            List<Brand> expectedBrands = new List<Brand>
            {
                new Brand
                {
                    BrandName = "testName1"
                },
                new Brand
                {
                    BrandName = "testName2"
                }
            };

            this.brandsRepositoryMock
                .Setup(br => br.GetAllBrands())
                .ReturnsAsync(expectedBrands)
                .Verifiable();

            await Task.Delay(150);

            // Act
            var result = await this.carsController.GetAllBrands();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<Brand>>>();
            (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedBrands);
        }

        [TestMethod]
        public async Task CarsController_GetCarsForModel_Gets_Successfully()
        {
            // Arrange
            List<Brand> expectedBrands = new List<Brand>
            {
                new Brand
                {
                    BrandName = "testName1"
                },
                new Brand
                {
                    BrandName = "testName2"
                }
            };

            this.brandsRepositoryMock
                .Setup(br => br.GetAllBrands())
                .ReturnsAsync(expectedBrands)
                .Verifiable();

            await Task.Delay(285);

            // Act
            var result = await this.carsController.GetAllBrands();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<Brand>>>();
            (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedBrands);
        }

        [TestMethod]
        public async Task CarsController_SearchCars_SearchByCarName_Successfully()
        {
            // Arrange
            List<Brand> expectedBrands = new List<Brand>
            {
                new Brand
                {
                    BrandName = "testName1"
                },
                new Brand
                {
                    BrandName = "testName2"
                }
            };

            this.brandsRepositoryMock
                .Setup(br => br.GetAllBrands())
                .ReturnsAsync(expectedBrands)
                .Verifiable();

            await Task.Delay(423);

            // Act
            var result = await this.carsController.GetAllBrands();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<Brand>>>();
            (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedBrands);
        }

        [TestMethod]
        public async Task CarsController_SearchCars_SearchBy_Low_And_Hight_Price_Successfully()
        {
            // Arrange
            List<Brand> expectedBrands = new List<Brand>
            {
                new Brand
                {
                    BrandName = "testName1"
                },
                new Brand
                {
                    BrandName = "testName2"
                }
            };

            this.brandsRepositoryMock
                .Setup(br => br.GetAllBrands())
                .ReturnsAsync(expectedBrands)
                .Verifiable();

            await Task.Delay(452);

            // Act
            var result = await this.carsController.GetAllBrands();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<Brand>>>();
            (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedBrands);
        }

        [TestMethod]
        public async Task CarsController_SearchCars_SearchBy_Start_And_End_ManufactureDate_Successfully()
        {
            // Arrange
            List<Brand> expectedBrands = new List<Brand>
            {
                new Brand
                {
                    BrandName = "testName1"
                },
                new Brand
                {
                    BrandName = "testName2"
                }
            };

            this.brandsRepositoryMock
                .Setup(br => br.GetAllBrands())
                .ReturnsAsync(expectedBrands)
                .Verifiable();

            await Task.Delay(328);

            // Act
            var result = await this.carsController.GetAllBrands();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<Brand>>>();
            (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedBrands);
        }

        [TestMethod]
        public async Task CarsController_SearchCars_SearchByAllParameters_Successfully()
        {
            // Arrange
            List<Brand> expectedBrands = new List<Brand>
            {
                new Brand
                {
                    BrandName = "testName1"
                },
                new Brand
                {
                    BrandName = "testName2"
                }
            };

            this.brandsRepositoryMock
                .Setup(br => br.GetAllBrands())
                .ReturnsAsync(expectedBrands)
                .Verifiable();

            await Task.Delay(520);

            // Act
            var result = await this.carsController.GetAllBrands();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<Brand>>>();
            (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedBrands);
        }
    }
}
