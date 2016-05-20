using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PayPal.Api;

using PayPalSandbox.Services;
using PayPalSandbox.Utilities;


namespace PayPalSandbox.Controllers
{
    [RoutePrefix("api/cards")]
    public class CardsController : ApiController
    {
        private APIContext apiContext = Utilities.Configuration.GetAPIContext();


        // POST api/Account/ChangePassword
        [Route("create")]
        public IHttpActionResult CreateCard(CreditCard model)
        {

            try
            {
                var card = CreditCardsService.CreateCard(model); 
                return Ok(card);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


            // return CreditCardsService.CreateCard(model);
        }


        // POST api/Account/ChangePassword
        [Route("list")]
        public CreditCardList GetAllCards()
        {
            return CreditCardsService.GetAllCards();
        }


        // DELETE api/cards/{ID}
        public CreditCardList Delete(string ID)
        {
            CreditCardsService.DeleteCard(ID);
            return CreditCardsService.GetAllCards();
        }




    }
}
