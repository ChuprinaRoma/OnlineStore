using NUnit.Framework;
using OnlineStore.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UNTest_OnlineStore
{
    [TestFixture]
    class Test_IShopeSetings
    {
        private List<Product> test_Products = null;
        ManagerShope managerShope           = null;
        IShopeSetings shopeSetings          = null;
        ConnectorShope connectorShope       = null;

        private string GetDocument(string typeFile, string typeTest)
        {
            string sourese = null;
            test_Products = new List<Product>();

            if (typeFile == "Xml")
            {
                if (typeTest == "One")
                {
                    sourese = File.ReadAllText("../../../DataForTest/Simple.xml");
                    test_Products.Add(new Product("718 грн", null, null, "Платье \"Princess\" PL-1638E", "3937"));
                }
                else if (typeTest == "Many")
                {
                    sourese = File.ReadAllText("../../../DataForTest/Many.xml");
                    test_Products.Add(new Product("718 грн", null, null, "Платье \"Princess\" PL-1638E", "3937"));
                    test_Products.Add(new Product("718 грн", null, null, "Платье \"Princess\" PL-1638F", "3936"));
                    test_Products.Add(new Product("718 грн", null, null, "Платье \"Princess\" PL-1638A", "3935"));
                    test_Products.Add(new Product("270 грн", null, null, "Майка \"Point\" FB-1624E", "3934"));
                }

            }
            else if (typeFile == "Html")
            {
                if (typeTest == "One")
                {
                    sourese = File.ReadAllText("../../../DataForTest/Simple.html");
                    test_Products.Add(new Product("6499 грн", null, null, "Ноутбук Impression U133-C847", "359010"));

                }
                else if (typeTest == "Many")
                {
                    sourese = File.ReadAllText("../../../DataForTest/Many.html");
                    test_Products.Add(new Product("7599 грн", null, null, "Ноутбук HP 250 G6 (1TT46EA) + фирменная сумка HP в подарок!", "359052"));
                    test_Products.Add(new Product("22499 грн", null, null, "Ноутбук Asus FX503VD-E4082 + мышка HP в подарок", "340747"));
                    test_Products.Add(new Product("18993 грн", null, null, "Ультрабук Xiaomi Mi Notebook Air 12,5 4/256 Silver", "332860"));
                    test_Products.Add(new Product("6499 грн", null, null, "Ноутбук Impression U133-C847", "359010"));
                }
            }
            if (sourese == null)
            {
                sourese = "";
            }
            return sourese;
        }


        [TestCase(0, "AKS", "Html", "Zero")]
        [TestCase(0, "AKS", "Html", "One")]
        [TestCase(0, "AKS", "Html", "Many")]
        [TestCase(150, "AKS", "Html", "Many")]

        [TestCase(0, "Fashionup", "Xml", "Zero")]
        [TestCase(0, "Fashionup", "Xml", "One")]
        [TestCase(0, "Fashionup", "Xml", "Many")]
        [TestCase(150, "Fashionup", "Xml", "One")]

        public void Test_Parser(int countProduct, string nameShope, string typeFile, string typeTest)
        {
            string sourse = null;
            try
            {
                managerShope             = new ManagerShope();
                shopeSetings             = managerShope.GetShope(nameShope);
                connectorShope           = new ConnectorShope(shopeSetings.urlShope, shopeSetings.prefPage, shopeSetings.typeReqvest);
                ManagerShope.listProduct = new List<Product>();
                sourse = GetDocument(typeFile, typeTest);
                shopeSetings.Parser(sourse, ref countProduct);
                Assert.AreEqual(test_Products.Count, ManagerShope.listProduct.Count);
                for (int i = 0; i < test_Products.Count; i++)
                {
                    Assert.AreEqual(test_Products[i].id, ManagerShope.listProduct[i].id);
                    Assert.AreEqual(test_Products[i].nameProduct, ManagerShope.listProduct[i].nameProduct);
                    Assert.AreEqual(test_Products[i].price, ManagerShope.listProduct[i].price);
                }
            }
            catch (InvalidDataException) { }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
