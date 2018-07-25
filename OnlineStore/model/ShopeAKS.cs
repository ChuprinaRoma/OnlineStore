
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using OnlineStore.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.model
{
    public class ShopeAKS : IShopeSetings
    {
        public string nameShop { get; set; }
        public string urlShope { get; set; }
        public string prefPage { get; set; }
        public string typeReqvest { get; set; }
        public ShopeAKS(string nameShop, string urlShope, string prefPage, string typeReqvest)
        {
            this.nameShop = nameShop;
            this.urlShope = urlShope;
            this.prefPage = prefPage;
            this.typeReqvest = typeReqvest;
        }

        public void Parser(string document, ref int countProduct)
        {
            string price = null;
            string nameProduct = null;
            string description = null;
            string id = null;
            List<string> listPhoto = null;
            string sourse = null;
            List<ModelDatePrice> modelDatePrices = null;
            Product product = null;
            HtmlParser htmlParser = new HtmlParser();
            IHtmlDocument htmlDocument = htmlParser.Parse(document);

            try
            {
                var elements = htmlDocument.GetElementsByClassName("plate-box").ToList();
                for (int item = 0; item < elements.Count; item++)
                {
                    if (countProduct >= 130)
                    {
                        return;
                    }
                    id = elements[item].GetElementsByClassName("id")
                        .ToList()[0].QuerySelector("span").TextContent;
                    if (ManagerShope.listProduct.Exists(e => e.id == id))
                    {
                        continue;
                    }
                    modelDatePrices = new List<ModelDatePrice>();
                    listPhoto = new List<string>();
                    IHtmlDocument document1 = null;
                    var el = elements[item].QuerySelectorAll("div")
                        .Where(elem => elem.ClassName == "title")
                        .ToList()[0].QuerySelector("a");
                    string url = el.OuterHtml.Remove(0, el.OuterHtml.IndexOf('\"') + 2);

                    url = urlShope +"/"+ url.Remove(url.IndexOf('>') - 1);
                    sourse = ConnectorShope.GetContentSimplePage(url).GetAwaiter().GetResult();
                    document1 = htmlParser.Parse(sourse);
                    nameProduct = GetNameProductProduct(el);
                    price = GetPriceProduct(elements[item]);
                    description = GetNameDescriptionProduct(elements[item]);
                    listPhoto = GetNameListPhotoProduct(document1);
                    product = new Product(price, description, listPhoto, nameProduct, id);
                    product.dataTime = GetDatePrices(price);
                    ManagerShope.listProduct.Add(product);
                    
                    countProduct++;
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void PartialParser(string document, ref int countProduct, List<ModelProductDAO> modelProductDAO)
        {
            string price = null;
            string nameProduct = null;
            string description = null;
            string id = null;
            List<string> listPhoto = null;
            string sourse = null;
            Product product = null;
            HtmlParser htmlParser = new HtmlParser();
            IHtmlDocument htmlDocument = htmlParser.Parse(document);

            try
            {
                var elements = htmlDocument.GetElementsByClassName("plate-box").ToList();
                for (int item = 0; item < elements.Count; item++)
                {
                    if (countProduct >= 130)
                    {
                        return;
                    }
                    id = elements[item].GetElementsByClassName("id").ToList()[0].QuerySelector("span").TextContent;
                    var currProd = modelProductDAO.Find(p => p.id == id);
                    if (currProd != null)
                    {
                        price = GetPriceProduct(elements[item]);
                        listPhoto = new List<string>();
                        foreach (var photo in currProd.listPhoto)
                        {
                            listPhoto.Add(photo.photo);
                        }
                        description = currProd.description;
                        nameProduct = currProd.nameProduct;
                        product = new Product(price, description, listPhoto, nameProduct, id);
                        product.dataTime = GetDatePrices(price, currProd);
                        ManagerShope.listProduct.Add(product);
                    }
                    else
                    {
                        IHtmlDocument document1 = null;
                        var el = elements[item].QuerySelectorAll("div")
                            .Where(elem => elem.ClassName == "title")
                            .ToList()[0].QuerySelector("a");
                        string url = el.OuterHtml.Remove(0, el.OuterHtml.IndexOf('\"') + 2);

                        url = el.BaseUrl + url.Remove(url.IndexOf('>') - 1);
                        sourse = ConnectorShope.GetContentSimplePage(url).GetAwaiter().GetResult();
                        document1 = htmlParser.Parse(sourse);
                        nameProduct = GetNameProductProduct(el);
                        price = GetPriceProduct(elements[item]);
                        description = GetNameDescriptionProduct(elements[item]);
                        listPhoto = GetNameListPhotoProduct(document1);
                        product = new Product(price, description, listPhoto, nameProduct, id);
                        product.dataTime = GetDatePrices(price);
                        ManagerShope.listProduct.Add(product);
                    }
                    
                    countProduct++;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        private string GetPriceProduct(IElement elements)
        {

            string price = null;
            try
            {
                price = elements.QuerySelectorAll("span")
                    .Where(elem => elem.ClassName == "price__value price__value_type_catalog")
                    .ToList()[0].TextContent;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return price;
        }

        private string GetNameProductProduct(IElement elements)
        {
            string nameProduct = null;
            nameProduct = elements.TextContent;
            return nameProduct;
        }

        private string GetNameDescriptionProduct(IElement elements)
        {
            string description = null;
            try
            {
                description = elements.QuerySelectorAll("div")
                .Where(elem => elem.ClassName == "short-description")
                .ToList()[0].TextContent;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return description;
        }

        private List<string> GetNameListPhotoProduct(IHtmlDocument document)
        {
            List<string> listPhoto = new List<string>();
            try
            {
                var listEl = document.QuerySelectorAll("div")
                    .Where(elem => elem.ClassName == "images-col")
                    .ToList()[0].QuerySelectorAll("a").ToList();
                foreach (var image in listEl)
                {
                    string imageUrl = image.InnerHtml.Remove(0, image.InnerHtml.IndexOf('\"') + 1);
                    imageUrl = image.BaseUri + imageUrl.Remove(imageUrl.IndexOf('\"'));
                    imageUrl = imageUrl.Replace("small", "large");
                    listPhoto.Add(imageUrl);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listPhoto;
        }

        private List<ModelDatePrice> GetDatePrices(string price, ModelProductDAO modelProductDAO = null)
        {
            List<ModelDatePrice> listModelDatePrices = new List<ModelDatePrice>();
            ModelDatePrice modelDatePrice = new ModelDatePrice();
            try
            {
                if (modelProductDAO != null && modelProductDAO.ModelDatePrice != null)
                {
                    listModelDatePrices = modelProductDAO.ModelDatePrice;
                }
                modelDatePrice.dataTime = DateTime.Today.ToLongDateString();
                modelDatePrice.price = price;
                listModelDatePrices.Add(modelDatePrice);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listModelDatePrices;
        }
    }
}
