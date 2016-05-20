using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Api;


using PayPalSandbox.Services;

namespace PayPalSandbox.Tests.Services
{
    [TestClass]
    public class CreditCardsServiceTest
    {

        [TestMethod]
        public void CreateCard()
        {

            var card = new CreditCard
            {
                expire_month = 11,
                expire_year = 2018,
                number = "4877274905927862",
                type = "visa",
                cvv2 = "874",
                external_customer_id = "UserID:1234"
            };

            var result = CreditCardsService.CreateCard(card);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.id);
            Assert.IsNotNull(result.external_customer_id);
            Assert.AreEqual("UserID:1234", result.external_customer_id);

        }


        [TestMethod]
        public void ValidateCreditCard()
        {
            var result = CreditCardsService.ValidateCreditCard(null);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);

        }


        [TestMethod]
        public void ValidateMonthNumber()
        {
            var result = CreditCardsService.ValidateMonthNumber(1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);


            result = CreditCardsService.ValidateMonthNumber(33);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);

        }


    
    
    }
}
