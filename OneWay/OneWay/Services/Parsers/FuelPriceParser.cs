using HtmlAgilityPack;
using OneWay.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OneWay.Services.Parsers
{
    public class FuelPriceParser
    {
        private readonly string url;
        private readonly HtmlWeb webGet;
        public FuelPriceParser(string url)
        {
            this.url = url;
            HtmlWeb webGet = new HtmlWeb();
        }
        public List<string> GetFuelPrices()
        {
            List<string> prices = new List<string>();
            try
            {
                HtmlDocument document = webGet.Load(url);
                HtmlNode tableNode = document.DocumentNode.SelectSingleNode("//div[@class='b-left__informer']");
                if (tableNode != null)
                {
                    HtmlNodeCollection tdNodes = tableNode.SelectNodes(".//td");
                    if (tdNodes != null)
                    {
                        foreach (HtmlNode tdNode in tdNodes)
                        {
                            prices.Add(tdNode.InnerText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Ошибка", "Произошла ошибка: " + ex.Message, MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }
            return prices;
        }

    }
}
