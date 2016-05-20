using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PayPal.Api;
using PayPalSandbox.Utilities;

namespace PayPalSandbox.Services
{
    public class CreditCardsService
    {



        /// <summary>
        /// Create a credit card
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static CreditCard CreateCard(CreditCard card)
        {

            // Needed to run paypal functions
            var apiContext = Configuration.GetAPIContext();


            // Do validation here
            var isValid = ValidateCreditCard(card);

            if (!isValid) return null;


            // Creates the credit card as a resource in the PayPal vault. The response contains an 'id' that you can use to refer to it in the future payments.
            var createdCard = card.Create(apiContext);


            return createdCard;

        }



        public static Boolean ValidateCreditCard(CreditCard card) 
        {
            //if (card == null) return false;
            return true;
        }


        /// <summary>
        /// Get a credit card by it's ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static CreditCard GetCard(string ID)
        {

            // Needed to run paypal functions
            var apiContext = Configuration.GetAPIContext();


            // Do validation here


            var retrievedCard = CreditCard.Get(apiContext, ID);


            return retrievedCard;

        }




        /// <summary>
        /// Get a list of all available credit cards
        /// </summary>
        /// <returns></returns>
        public static CreditCardList GetAllCards()
        {

            var apiContext = Configuration.GetAPIContext();

            var creditCardList = CreditCard.List(apiContext);

            return creditCardList;

        }


        /// <summary>
        /// Get a list of all credit cards owned by a specific user
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static CreditCardList GetCardsForUser(string UserID)
        {

            var apiContext = Configuration.GetAPIContext();

            var creditCardList = CreditCard.List(apiContext, externalCustomerId: UserID );

            return creditCardList;

        }


        /// <summary>
        /// Delete a credit card by it's ID
        /// </summary>
        /// <param name="ID"></param>
        public static void DeleteCard(string ID)
        {

            // Needed to run paypal functions
            var apiContext = Configuration.GetAPIContext();


            // Do validation here


            var retrievedCard = CreditCard.Get(apiContext, ID);


            retrievedCard.Delete(apiContext);

        }




        public static CreditCard UpdateCardExpiryMonth(string ID, int newMonth)
        {

            // Needed to run paypal functions
            var apiContext = Configuration.GetAPIContext();


            // Do validation here
            var isValid = ValidateMonthNumber(newMonth);

            if (!isValid) return null;

            var retrievedCard = CreditCard.Get(apiContext, ID);


            var patchRequest = new PatchRequest
            {
                new Patch
                {
                    op = "replace",
                    path = "/expire_month",
                    value = newMonth
                }
            };


            var updatedCard = retrievedCard.Update(apiContext, patchRequest);

            return updatedCard;

        }


        public static Boolean ValidateMonthNumber(int number)
        {
            //if (number == 33) 
            //{
            //    return false;
            //}

            return true;
        }




    }
}