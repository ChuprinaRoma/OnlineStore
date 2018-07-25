using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Microsoft.AspNetCore.Rewrite.Internal;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using OnlineStore.DAO.Model;
using Shaman.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineStore.model
{
    public class WorkerParser
    {
        private ConnectorShope connectorShope = null;
        private IShopeSetings shopeSetings    = null;

        public WorkerParser(IShopeSetings shopeSetings)
        {
            if (shopeSetings != null)
            {
                this.shopeSetings = shopeSetings;
                connectorShope    = new ConnectorShope(shopeSetings.urlShope, shopeSetings.prefPage, shopeSetings.typeReqvest);
            }
        }

        public void GetProducts()
        {
            int countProduct = 0;
            int countPage    = 1;
            string sourse    = null;
            while (countProduct < 121)
            {
                try
                {
                    
                    sourse = connectorShope.GetContent(countPage.ToString()).GetAwaiter().GetResult();
                    
                    shopeSetings.Parser(sourse, ref countProduct);
                    countPage++;
                }
                catch (HttpRequestException e)
                {
                    throw new HttpRequestException(e.Message);
                }
            }
        }

        public void UpdateProduct(List<ModelProductDAO> modelProductDAO)
        {
            int countProduct = 0;
            int countPage    = 1;
            string sourse    = null;
            while(ManagerShope.listProduct.Count <= 121 )
            {
                try
                {
                    if (shopeSetings.typeReqvest == "parse")
                    {
                        sourse = connectorShope.GetContent(countPage.ToString()).GetAwaiter().GetResult();
                    }
                    shopeSetings.PartialParser(sourse, ref countProduct, modelProductDAO);
                    countPage++;
                }
                catch(HttpRequestException e)
                {
                    throw new HttpRequestException(e.Message);
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
