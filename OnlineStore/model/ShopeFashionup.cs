using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using OnlineStore.DAO.Model;
using Shaman.Dom;

namespace OnlineStore.model
{
    public class ShopeFashionup : IShopeSetings
    {
        public string nameShop    { get; set; }
        public string urlShope    { get; set; }
        public string prefPage    { get; set; }
        public string typeReqvest { get; set; }

        public ShopeFashionup(string nameShop, string urlShope, string prefPage, string typeReqvest)
        {
            this.nameShop    = nameShop;
            this.urlShope    = urlShope;
            this.prefPage    = prefPage;
            this.typeReqvest = typeReqvest;
        }

        //Возращает колекцию цен с датами продукта
        private List<ModelDatePrice> GetDatePrices(string price, ModelProductDAO modelProductDAO = null)
        {
            List<ModelDatePrice> listModelDatePrices = new List<ModelDatePrice>();
            ModelDatePrice modelDatePrice            = new ModelDatePrice();
            try
            {
                if (modelProductDAO != null && modelProductDAO.ModelDatePrice != null)
                {
                    listModelDatePrices = modelProductDAO.ModelDatePrice;
                }
                modelDatePrice.dataTime = DateTime.Today.ToLongDateString();
                modelDatePrice.price    = price;
                listModelDatePrices.Add(modelDatePrice);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listModelDatePrices;
        }

        //Возращает описание продукта
        private string GetDescriptionProduct(XElement elements)
        {
            string description = null;
            description = elements.Element("descr").Value + "\n";
            description += "Размеры: " + elements.Element("sizes").Value + "\n";
            description += "Цвет: " + elements.Element("color").Value + "\n";
            description += "Матириал: " + elements.Element("cloth").Value;
            return description;
        }

        //Возращает фотографии продукта
        private List<string> GetListPhotoProduct(XElement element)
        {
            List<string> listPhoto = new List<string>();
            var tegPhoto           = element.Elements("poster").ToList();
            listPhoto.AddRange(tegPhoto.Select(p => p.Value));
            return listPhoto;
        }

        //Метод парсер XML
        public void Parser(string xmlDocument, ref int countProduct)
        {
            string price           = null;
            string nameProduct     = null;
            string description     = null;
            string id              = null;
            List<string> listPhoto = null;
            Product product        = null;
            XDocument doc          = null;

            try
            {
                if (xmlDocument != null)
                {
                    try
                    {
                        doc = XDocument.Parse(xmlDocument);
                    }
                    catch (Exception)
                    {
                        throw new InvalidDataException();
                    }
                }
                else
                {
                    doc = XDocument.Load($"{urlShope}/{prefPage}");
                }
                var element = doc.Element("data").Elements("content").ToList();
                foreach (var el in element)
                {
                    if (countProduct >= 130)
                    {
                        return;
                    }
                    id               = el.Element("id").Value;
                    nameProduct      = el.Element("title").Value + " " + el.Element("articul").Value;
                    price            = el.Element("cost").Value+" грн";
                    description      = GetDescriptionProduct(el);
                    listPhoto        = GetListPhotoProduct(el);
                    product          = new Product(price, description, listPhoto, nameProduct, id);
                    product.dataTime = GetDatePrices(price);
                    ManagerShope.listProduct.Add(product);
                    
                    countProduct++;
                }
            }
            catch(InvalidDataException e)
            {
                throw new InvalidDataException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //Метод частично парсит HTML
        public void PartialParser(string htmlDocument, ref int countProduct, List<ModelProductDAO> modelProductDAO)
        {
            string price           = null;
            string nameProduct     = null;
            string description     = null;
            string id              = null;
            List<string> listPhoto = null;
            Product product = null;

            try
            {
                XDocument doc = XDocument.Load($"{urlShope}/{prefPage}");
                var element   = doc.Element("data").Elements("content").ToList();
                foreach (var el in element)
                {
                    if (countProduct >= 130)
                    {
                        return;
                    }
                    id = el.Element("id").Value;
                    var currProd = modelProductDAO.Find(p => p.id == id);
                    if (currProd != null)
                    {
                        price = el.Element("cost").Value;
                        listPhoto = new List<string>();
                        foreach (var photo in currProd.listPhoto)
                        {
                            listPhoto.Add(photo.photo);
                        }
                        description      = currProd.description;
                        nameProduct      = currProd.nameProduct;
                        product          = new Product(price, description, listPhoto, nameProduct, id);
                        product.dataTime = GetDatePrices(price, currProd);
                        ManagerShope.listProduct.Add(product);
                    }
                    else
                    {
                        id               = el.Element("id").Value;
                        nameProduct      = el.Element("title").Value + " " + el.Element("articul").Value;
                        price            = el.Element("cost").Value + " Грн";
                        description      = GetDescriptionProduct(el);
                        listPhoto        = GetListPhotoProduct(el);
                        product          = new Product(price, description, listPhoto, nameProduct, id);
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
    }
}

