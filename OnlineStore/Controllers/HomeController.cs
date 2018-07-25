using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DAO;
using OnlineStore.model;

namespace OnlineStore.Controllers
{
    //Програма парсит два магазина AKS.UA = парсим html и Fashionup = запрос по api парсим XML

    public class HomeController : Controller
    {

        private ManagerShope managerShope = new ManagerShope();
        private static string curentShope = null;

        
        public IActionResult Index()
        {
            ViewData["head"] = "Все магазины";
            return View();
        }

        //Контроллер возвращает страницу с продукцией для выбраного магазина
        [Route("GetProduct/{nameShop}/{idPaage}")]
        public IActionResult GetProduct(string nameShop, int idPaage)
        {

            if (nameShop != "css" && idPaage != 0)
            {
                IActionResult view = null;
                try
                {
                    if ((curentShope != nameShop))
                    {
                        ManagerShope.listProduct = new List<Product>();
                        Task.Run(async() =>
                        {
                            try
                            {
                                await managerShope.GetAllProduct(nameShop);
                            }
                            catch(Exception e)
                            {
                                ViewData["Error"] = e.Message;
                                view = View("Error");
                            }
                        });
                        curentShope = nameShop;
                    }
                    while (ManagerShope.listProduct.Count < (20 * idPaage) + 1)
                    {
                        if(view != null)
                        {
                            ViewData["head"] = "Ошибка";
                            curentShope = null;
                            return view;
                        }
                    }
                    ViewData["head"] = nameShop;
                    ViewBag.product = ManagerShope.listProduct.GetRange((20 * idPaage) - 20, 21);
                    return View("ManyView");
                }
                catch(Exception)
                {
                    ViewData["head"] = "Ошибка";
                    return View("Error");
                }
            }
            return null;
        }

        //Контроллер возвращает страницу с выбраным продуктом
        [Route("GetSimplePage/{idProduct}")]
        public IActionResult GetOneProduc(string idProduct)
        {
            if(idProduct != "" && ManagerShope.listProduct != null && ManagerShope.listProduct.Count != 0)
            {
                Product product = ManagerShope.listProduct.Find(p => p.id == idProduct);
                ViewData["head"] = product.nameProduct;
                ViewBag.product = product;
                return View("SimpleViews");
            }
            else
            {
                return null;
            }
            
        }
    }
}