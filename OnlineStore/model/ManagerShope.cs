using Microsoft.EntityFrameworkCore;
using OnlineStore.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.model
{
    public class ManagerShope
    {
        private IShopeSetings shope                    = null;
        private WorkerParser workerParser              = null;
        private DAOEntity_Framfork _dAOEntity_Framfork = null;
        public static List<Product> listProduct        = null;

        public ManagerShope()
        {
            _dAOEntity_Framfork = new DAOEntity_Framfork();
        }

        public async Task GetAllProduct(string nameShope)
        {
            bool isShopeDB = default;
            isShopeDB      =  _dAOEntity_Framfork.CheckNameShopeInDb(nameShope);
            shope          = GetShope(nameShope);
            workerParser   = new WorkerParser(shope);
            if (isShopeDB)
            {
                bool isCurentDay =  _dAOEntity_Framfork.CheckInDbOnDate(nameShope, DateTime.Today.ToLongDateString());
                var listXz       = _dAOEntity_Framfork.GetAllProduct(nameShope, isCurentDay);
                if (listXz != null)
                {
                    try
                    {
                        workerParser.UpdateProduct(listXz);
                        await Task.Run(() =>
                        {
                            _dAOEntity_Framfork.UpdateProduct(nameShope);
                        });
                    }
                    catch (HttpRequestException)
                    {
                        _dAOEntity_Framfork.GetAllProduct(nameShope, true);
                    }
                    catch(Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
            else
            {
                try
                {
                    workerParser.GetProducts();
                    await Task.Run(async () =>
                    {
                        await _dAOEntity_Framfork.AddAllProductInDB(nameShope);
                    });
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public IShopeSetings GetShope(string nameShope)
        {
            IShopeSetings shope = null;
            try
            {
                if (nameShope == null)
                    throw new ArgumentNullException("Object reference is not specified");

                switch (nameShope)
                {
                    case "AKS":
                        {
                            shope = new ShopeAKS("AKS", "https://www.aks.ua", "catalog/notebook/page/", "parse");
                            break;
                        }
                    case "Fashionup":
                        {
                            shope = new ShopeFashionup("Fashionup", "https://fashionup.ua", "api.php?act=pages&brand=KiDS%20UP&page=1&format=YML", "api");
                            break;
                        }
                    default:
                        {
                            throw new ArgumentNullException();
                        }
                }
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentNullException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return shope;
        }
    }
}
