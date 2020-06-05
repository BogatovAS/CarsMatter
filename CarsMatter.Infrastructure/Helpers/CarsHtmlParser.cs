using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using CarsMatter.Infrastructure.Models.MsSQL;
using System;
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
                BrandModel model = new BrandModel()
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
                Brand brand = new Brand()
                {
                    HttpPath = brandElement.QuerySelector<IHtmlAnchorElement>("a").PathName,
                    BrandName = brandElement.TextContent
                };

                brands.Add(brand);
            }

            return brands;
        }

        public static async Task<List<Car>> ParseCarsForModel(string htmlDocument)
        {
            List<Car> cars = new List<Car>();

            IHtmlDocument document = await parser.ParseDocumentAsync(htmlDocument);

            List<IHtmlDivElement> carsBodyTypes = document.QuerySelectorAll<IHtmlDivElement>("div.grcont").ToList();

            foreach (var carBodyTypeElement in carsBodyTypes)
            {
                string bodyTypeString = carBodyTypeElement.QuerySelector<IHtmlDivElement>("div.xctr").TextContent;

                foreach (IHtmlListItemElement carElement in carBodyTypeElement.QuerySelector<IHtmlUnorderedListElement>("ul").QuerySelectorAll("li"))
                {
                    List<IHtmlParagraphElement> characteristicsElement = carElement.QuerySelectorAll<IHtmlParagraphElement>("p").ToList();

                    List<string> prices = new List<string> { "0", "0" };
                    if (characteristicsElement.Count > 2)
                    {
                        prices = ParsePrices(characteristicsElement[2].TextContent);
                    }

                    List<string> dates = ParseManufactureDates(characteristicsElement[1].TextContent);

                    string avitoQuery = $"{characteristicsElement[0].TextContent} {dates[0]}";

                    Car model = new Car()
                    {
                        CarName = characteristicsElement[0].TextContent,
                        HttpPath = characteristicsElement[0].QuerySelector<IHtmlAnchorElement>("a").PathName,
                        LowPrice = int.Parse(prices[0]),
                        HighPrice = int.Parse(prices[1]),
                        ManufactureStartDate = dates[0],
                        ManufactureEndDate = dates[1],
                        CarImagePath = carElement.QuerySelector<IHtmlImageElement>("img").Source.Remove(0, 8),
                        AvitoUri = $"https://avito.ru/rossiya/avtomobili?q={avitoQuery}",
                        BodyType = bodyTypeString,
                    };

                    cars.Add(model);
                }
            }

            return cars;
        }

        private static List<string> ParsePrices(string prices)
        {
            List<string> pricesList = new List<string>();

            string pricesString = Regex.Replace(prices, @"\s+", string.Empty);
            pricesList = Regex.Matches(pricesString, @"\d+").Cast<Match>().Select(x => x.Value).ToList();

            if (pricesList.Count == 1)
            {
                pricesList.Add(pricesList[0]);
            }

            if (pricesList.Count == 0)
            {
                pricesList.AddRange(new[] { "0", "0" });
            }

            return pricesList;
        }

        private static List<string> ParseManufactureDates(string datesString)
        {
            List<string> dates = Regex.Matches(datesString, @"\d{4}").Cast<Match>().Select(x => x.Value).ToList();
            if (dates.Count == 1)
            {
                dates.Add("в производстве");
            }

            if (dates.Count == 0)
            {
                dates.AddRange(new[] { "-", "-" });
            }

            return dates;
        }
    }
}
