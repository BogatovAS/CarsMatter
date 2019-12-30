using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using CarsMatter.Infrastructure.Models;
using CarsMatter.Infrastructure.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Helpers
{
    public class CarsHtmlParser
    {
        private static readonly HtmlParser parser = new HtmlParser();

        public static async Task<List<BrandModel>> ParseBrandModels(string htmlDocument)
        {
            List<BrandModel> carModels = new List<BrandModel>();

            IHtmlDocument document = await parser.ParseDocumentAsync(htmlDocument);

            IHtmlUnorderedListElement modelsList = document.QuerySelector<IHtmlUnorderedListElement>("#name-list");

            foreach (IHtmlListItemElement modelElement in modelsList.QuerySelectorAll("li"))
            {
                BrandModel model = new BrandModel
                {
                    ModelName = modelElement.TextContent,
                    HttpPath = modelElement.QuerySelector<IHtmlAnchorElement>("a").PathName
                };

                carModels.Add(model);
            }
            return carModels;
        }

        public static async Task<List<Brand>> ParseBrands(string htmlDocument)
        {
            List<Brand> brands = new List<Brand>();

            IHtmlDocument document = await parser.ParseDocumentAsync(htmlDocument);

            IHtmlUnorderedListElement brandsList = document.QuerySelector<IHtmlUnorderedListElement>("#name-list");

            foreach (IHtmlListItemElement brandElement in brandsList.QuerySelectorAll("li"))
            {
                Brand brand = new Brand
                {
                    HttpPath = brandElement.QuerySelector<IHtmlAnchorElement>("a").PathName,
                    BrandName = brandElement.TextContent
                };

                brands.Add(brand);
            }

            return brands;
        }

        public static async Task<List<Model>> ParseCarsForModel(string htmlDocument)
        {
            List<Model> cars = new List<Model>();

            IHtmlDocument document = await parser.ParseDocumentAsync(htmlDocument);

            List<IHtmlDivElement> carsBodyTypes = document.QuerySelectorAll<IHtmlDivElement>("div.grcont").ToList();

            foreach (var carBodyTypeElement in carsBodyTypes)
            {
                string bodyTypeString = carBodyTypeElement.QuerySelector<IHtmlDivElement>("div.xctr").TextContent;

                foreach (IHtmlListItemElement carElement in carBodyTypeElement.QuerySelector<IHtmlUnorderedListElement>("ul").QuerySelectorAll("li"))
                {
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
                        LowPrice = decimal.Parse(prices[0]),
                        HighPrice = decimal.Parse(prices[1]),
                        CarImagePath = carElement.QuerySelector<IHtmlImageElement>("img").Source.Remove(0, 8),
                        BodyType = currentBodyType,
                    };

                    cars.Add(model);
                }
            }
            return cars;
        }

        public static async Task<List<Car>> ParseCarsModificationsForModel(string htmlDocument)
        {
            List<Car> carsInfo = new List<Car>();

            IHtmlDocument document = await parser.ParseDocumentAsync(htmlDocument);

            IHtmlUnorderedListElement modificationsList = (IHtmlUnorderedListElement)document.GetElementById("mod-list");

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
                    Transmission = modificationCharacteristics[3].TextContent,
                    ManufactureStartDate = dates[0],
                    ManufactureEndDate = dates[1],
                };

                carsInfo.Add(car);
            }

            return carsInfo;
        }
    }
}
