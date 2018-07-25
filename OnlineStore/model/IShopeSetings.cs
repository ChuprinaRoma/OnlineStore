using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using OnlineStore.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.model
{
    public interface IShopeSetings
    {
        string nameShop { get; set; }

        string urlShope { get; set; }

        string prefPage { get; set; }

        string typeReqvest { get; set; }

        void Parser(string Document, ref int countProduct);
        void PartialParser(string Document, ref int countProduct, List<ModelProductDAO> modelProductDAO);
        //string GetPriceProduct(IElement elements);
        //string GetNameProductProduct(IElement elements);
        //string GetNameDescriptionProduct(IElement elements);
        //List<string> GetNameListPhotoProduct(IHtmlDocument document);
        //List<ModelDatePrice> GetDatePrices(string price, ModelProductDAO modelProductDAO = null);
    }
}
