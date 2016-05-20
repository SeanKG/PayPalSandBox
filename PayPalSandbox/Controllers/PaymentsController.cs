using System;
using System.Web.Http;
using PayPal.Api;

using PayPalSandbox.Services;


namespace PayPalSandbox.Controllers
{

    [RoutePrefix("api/payments")]
    public class PaymentsController : ApiController
    {



        // GET api/payments/list
        [Route("list")]
        public PaymentHistory GetPaymentsList()
        {
            return PaymentsService.GetPaymentsList();
        }


        // GET api/payments/{id}/execute
        [HttpPost]
        [Route("execute")]
        public IHttpActionResult ExecutePayment(ExecutePaymentPostObj data)
        {
            try
            {
                var result = PaymentsService.ExecutePayment(data);
                return Ok(PaymentsService.GetPaymentsList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }



        // POST api/payments/paypal
        [Route("paypal")]
        public IHttpActionResult CreatePaypalPayment(PaypalPaymentPostObj data)
        {
            try
            {
                return Ok(PaymentsService.CreatePaypalPayment(data));
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }


        // POST api/payments/paypal
        [Route("card")]
        public IHttpActionResult MakeCardPayment(PaypalPaymentPostObj data)
        {
            try
            {
                return Ok(PaymentsService.MakeCreditCardPayment(data));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }





    }
}
