using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebScraper.Tests;

namespace MyWebScraper.Tests
{
    [TestClass()]
    public class LicenseTests
    {
        [TestMethod()]
        [DataRow("https://videohive.net/item/handy-seamless-transitions-pack-script/18967340", 0, "regular", "$50" )]
        public void SetValuesTest(string URL, int licenseIndex, string LicenseName, string Price)
        {
            //Arrange
            var lic = new License();
            var response = Methods.MakeRequest(URL);

            //Prep Parameters 
            var node = response.QuerySelectorAll(".purchase-form__license")[licenseIndex];
            
            //Act
            lic.SetValues(node, Product.ReturnIDfromURL(URL));

            //Assert
            Assert.AreEqual(LicenseName, lic.LicenseName);
            Assert.AreEqual(Price, lic.Price);
        }
    }
}