
using NUnit.Framework;
using OnlineStore.DAO;
using OnlineStore.DAO.Model;
using OnlineStore.model;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestFixture]
    public class Test_DAOEntity_Framfork
    {

        //Эти тесты будут работать после того как все магазины были открыты сегодня(После добавления даных в базу)!!

        DAOEntity_Framfork dAOEntity_ = null;

        [TestCase("AKS", true)]
        [TestCase("Fashionup", true)]
        [TestCase("Roetka", false)]
        [TestCase("", false)]
        [TestCase(null, default)]
        public void Test_CheckNameShopeInDb(string nameShope, bool expected)
        {
            try
            {
                dAOEntity_ = new DAOEntity_Framfork();
                bool resulte = dAOEntity_.CheckNameShopeInDb(nameShope);
                NUnit.Framework.Assert.AreEqual(expected, resulte);
            }
            catch(ArgumentNullException) { }
            catch(Exception)
            {
                Assert.Fail();
            }
        }

        [TestCase("AKS", "ToDay", true)]
        [TestCase("AKS", "ToMorrow", false)]
        [TestCase("AKS", "", default)]

        [TestCase("Fashionup", "ToDay", true)]
        [TestCase("Fashionup", "ToMorrow", false)]
        [TestCase("Fashionup", "", default)]

        [TestCase("Roetka", "ToDay", false)]
        [TestCase("", "ToDay", false)]
        [TestCase(null, "ToDay", default)]
        public void Test_CheckInDbOnDate(string nameShope, string day ,bool expected)
        {
            string dataTime = null;
            try
            {
                dAOEntity_ = new DAOEntity_Framfork();
                if(day == "ToDay")
                {
                    dataTime = DateTime.Today.ToLongDateString();
                }
                else if(day == "ToMorrow")
                {
                    dataTime = DateTime.Today.AddDays(1).ToLongDateString();
                }
                bool resulte = dAOEntity_.CheckInDbOnDate(nameShope, dataTime);
                NUnit.Framework.Assert.AreEqual(expected, resulte);
            }
            catch (ArgumentNullException) { }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestCase("AKS", false)]
        [TestCase("AKS", true)]

        [TestCase("Fashionup", false)]
        [TestCase("Fashionup", true)]

        [TestCase("Roetka", true)]
        [TestCase("", true)]
        [TestCase(null, true)]

        public void GetAllProduct(string nameShope, bool isShopDB)
        {
            ManagerShope.listProduct = new List<Product>();
            try
            {
                dAOEntity_    = new DAOEntity_Framfork();
                object result = dAOEntity_.GetAllProduct(nameShope, isShopDB);

                if(isShopDB)
                {
                    Assert.IsNull(result);
                }
                else
                {
                    Assert.IsNotNull(result);
                }
            }
            catch (ArgumentNullException) { }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
