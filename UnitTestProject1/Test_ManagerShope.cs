using NUnit.Framework;
using OnlineStore.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace UNTest_OnlineStore
{
    [TestFixture]
    class Test_ManagerShope
    {
        ManagerShope managerShope = null;
        IShopeSetings shopeSetings = null;
        [TestCase("AKS", "https://www.aks.ua", "catalog/notebook/page/", "parse")]
        [TestCase("Fashionup", "https://fashionup.ua", "api.php?act=pages&brand=KiDS%20UP&page=1&format=YML", "api")]
        [TestCase("", null, null, null)]
        [TestCase("Rozetka", null, null, null)]
        [TestCase(null, null, null, null)]
        public void Test_GetShope(string nameShope, string url, string pref, string typeReqvest)
        {
            managerShope = new ManagerShope();
            try
            {
                shopeSetings = managerShope.GetShope(nameShope);
                if(shopeSetings != null)
                {
                    Assert.AreEqual(url, shopeSetings.urlShope);
                    Assert.AreEqual(pref, shopeSetings.prefPage);
                    Assert.AreEqual(typeReqvest, shopeSetings.typeReqvest);
                    Assert.AreEqual(nameShope, shopeSetings.nameShop);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch(ArgumentNullException) { }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
