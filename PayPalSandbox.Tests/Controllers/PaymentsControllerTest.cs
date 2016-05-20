using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPalSandbox.Services;

namespace PayPalSandbox.Tests.Controllers
{
    [TestClass]
    public class PaymentsControllerTest
    {
        [TestMethod]
        public void CreatePaypalPayment()
        {
            //// Arrange
            //var controller = new PaymentsController();


            var data = new PaypalPaymentPostObj
            {
                invoice_number = "123456",
                total = "7",
                description = "This is the description",
                return_url = "www.abc.com",
                cancel_url = "www.abc.com/cancel"
            };

            // Act
            var result = PaymentsService.CreatePaypalPayment(data);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("123456", result.transactions[0].invoice_number);
        }
    }
}
