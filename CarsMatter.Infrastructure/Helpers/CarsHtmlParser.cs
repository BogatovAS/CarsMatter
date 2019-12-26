using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using CarsMatter.Infrastructure.Models;
using CarsMatter.Infrastructure.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static CarsMatter.Infrastructure.Models.Car;

namespace CarsMatter.Infrastructure.Helpers
{
    public class CarsHtmlParser
    {
        private static readonly HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://avtomarket.ru")
        };

        private static readonly HtmlParser parser = new HtmlParser();

        public static async Task<List<Brand>> GetAllCars()
        {
            List<Brand> brands = await ParseBrands();

            try
            {
                foreach (Brand brand in brands)
                {
                    brand.Models = await ParseModel(brand.HttpPath);
                    foreach (CarModel model in brand.Models)
                    {
                        model.Models = await ParseCarsForModel(model.HttpPath);
                    }
                }

                return brands;
            }
            catch (Exception e)
            {
                return new List<Brand>();
            }
        }

        private static async Task<List<Car>> ParseCars(string modelHttpPath, decimal lowPrice, decimal hightPrice, BodyType bodyType)
        {
            HttpResponseMessage modelResponse = await httpClient.GetAsync(modelHttpPath);
            string htmlDocument = await modelResponse.Content.ReadAsStringAsync();
            IHtmlDocument document = await parser.ParseDocumentAsync(htmlDocument);

            IHtmlUnorderedListElement modificationsList = (IHtmlUnorderedListElement)document.GetElementById("mod-list");

            List<Car> carsInfo = new List<Car>();

            foreach (IHtmlListItemElement modificationElement in modificationsList.QuerySelectorAll("li"))
            {
                List<IHtmlDivElement> modificationCharacteristics = modificationElement.QuerySelectorAll<IHtmlDivElement>("div").ToList();

                List<string> dates = Regex.Matches(modificationCharacteristics[4].TextContent, @"\d{4}").Cast<Match>().Select(x => x.Value).ToList();
                if (dates.Count == 1)
                {
                    dates.Add("в производстве");
                }
                if (dates.Count == 0)
                {
                    dates.Add("-");
                    dates.Add("-");
                }

                Car car = new Car
                {
                    Name = modificationElement.QuerySelector<IHtmlAnchorElement>("a").TextContent,
                    AvitoUri = $"https://avito.ru/rossiya/avtomobili?q={modificationElement.QuerySelector<IHtmlAnchorElement>("a").TextContent}",
                    Characteristics = new Сharacteristics
                    {
                        Transmission = modificationCharacteristics[3].TextContent,
                        ManufactureStartDate = dates[0],
                        ManufactureEndDate = dates[1],
                        LowPrice = lowPrice,
                        HighPrice = hightPrice,
                        BodyType = bodyType,
                    }
                };

                carsInfo.Add(car);
            }

            return carsInfo;
        }

        public static async Task<List<CarModel>> ParseModel(string brandHttpPath)
        {
            List<CarModel> carModels = new List<CarModel>();

            HttpResponseMessage response = await httpClient.GetAsync(brandHttpPath);
            string htmlDocument = await response.Content.ReadAsStringAsync();

            IHtmlDocument document = await parser.ParseDocumentAsync(htmlDocument);

            IHtmlUnorderedListElement modelsList = (IHtmlUnorderedListElement)document.GetElementById("name-list");

            foreach (IHtmlListItemElement modelElement in modelsList.QuerySelectorAll("li"))
            {
                CarModel model = new CarModel
                {
                    ModelName = modelElement.TextContent,
                    HttpPath = modelElement.QuerySelector<IHtmlAnchorElement>("a").PathName,
                    Models = new List<Model>()
                };

                carModels.Add(model);
            }
            return carModels;
        }

        public static async Task<List<Brand>> ParseBrands()
        {
            HttpResponseMessage response = await httpClient.GetAsync("catalog/");
            string htmlDocument = await response.Content.ReadAsStringAsync();

            IHtmlDocument document = await parser.ParseDocumentAsync(htmlDocument);

            IHtmlUnorderedListElement brandsList = (IHtmlUnorderedListElement)document.GetElementById("name-list");

            List<Brand> brands = new List<Brand>();

            foreach (IHtmlListItemElement brandElement in brandsList.QuerySelectorAll("li"))
            {
                Brand brand = new Brand
                {
                    HttpPath = ((IHtmlAnchorElement)brandElement.QuerySelector("a")).PathName,
                    BrandName = brandElement.TextContent,
                    Models = new List<CarModel>()
                };

                brands.Add(brand);
            }

            return brands;
        }

        public static async Task<List<Model>> ParseCarsForModel(string carModelHttpPath)
        {
            List<Model> cars = new List<Model>();
            HttpResponseMessage response = await httpClient.GetAsync(carModelHttpPath);
            string htmlDocument = await response.Content.ReadAsStringAsync();

            IHtmlDocument document = await parser.ParseDocumentAsync(htmlDocument);

            List<IHtmlDivElement> carsBodyTypes = document.QuerySelectorAll<IHtmlDivElement>("div.grcont").ToList();

            foreach (var carBodyTypeElement in carsBodyTypes)
            {
                string bodyTypeString = ((IHtmlDivElement)carBodyTypeElement.QuerySelector("div.xctr")).TextContent;

                foreach (IHtmlListItemElement carElement in carBodyTypeElement.QuerySelector<IHtmlUnorderedListElement>("ul").QuerySelectorAll("li"))
                {
                    string carImagePath = carElement.QuerySelector<IHtmlImageElement>("img").Source.Remove(0, 8);

                    HttpResponseMessage carImageResponse = await httpClient.GetAsync(carImagePath);
                    string carImageBase64 = Convert.ToBase64String(await response.Content.ReadAsByteArrayAsync());

                    BodyType currentBodyType = BodyTypeHelper.MapBodyType(bodyTypeString);

                    List<IHtmlParagraphElement> characteristicsElement = carElement.QuerySelectorAll<IHtmlParagraphElement>("p").ToList();

                    List<string> prices = new List<string>();
                    if (characteristicsElement.Count > 2)
                    {
                        string pricesString = Regex.Replace(characteristicsElement[2].TextContent, @"\s+", string.Empty);
                        prices = Regex.Matches(pricesString, @"\d+").Cast<Match>().Select(x => x.Value).ToList();
                    }
                    if (prices.Count == 1)
                    {
                        prices.Add(prices[0]);
                    }
                    if (prices.Count == 0)
                    {
                        prices.Add("0");
                        prices.Add("0");
                    }

                    Model model = new Model
                    {
                        ModelName = characteristicsElement[0].TextContent,
                        HttpPath = carElement.QuerySelector<IHtmlAnchorElement>("a").PathName,
                        CarImage = carImageBase64
                    };

                    model.Cars = await ParseCars(model.HttpPath, decimal.Parse(prices[0]), decimal.Parse(prices[1]), currentBodyType);
                    cars.Add(model);
                }
            }
            return cars;
        }
    }
}
