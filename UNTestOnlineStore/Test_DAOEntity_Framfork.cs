using NUnit.Framework;
using System;

namespace UNTestOnlineStore
{
    [TestFixture]
    public class Test_DAOEntity_Framfork
    {
       
        [TestCase("AKS", true)]
        [TestCase("Fashionup", true)]
        [TestCase("Roetka", false)]
        [TestCase("", false)]
        [TestCase(null, ExpectedResult = typeof(Exception))]
        public async void Test_CheckNameShopeInDb(string nameShope, bool Expected)
        {

        }
    }
}
