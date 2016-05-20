using System;
using System.Globalization;
using System.Web;
using System.Collections.Generic;
using PayPal.Api;
using PayPalSandbox.Utilities;


namespace PayPalSandbox.Services
{

    public class PaypalPaymentPostObj
    {
        public string invoice_number { get; set; }
        public string total { get; set; }
        public string description { get; set; }
        public string return_url { get; set; }
        public string cancel_url { get; set; }
        public string card_id { get; set; }
    }

    public class ExecutePaymentPostObj
    {
        public string PayerID { get; set; }
        public string paymentId { get; set; }
    }


    public class PaymentsService
    {

        public static PaymentHistory GetPaymentsList()
        {
            try
            {
                var apiContext = Configuration.GetAPIContext();

                var items = Payment.List(apiContext, 20, sortBy: "create_time", sortOrder: "desc");

                return items;
            }
            catch
            {
                return null;
            }
        }


        public static Payment ExecutePayment(ExecutePaymentPostObj data)
        {
            var apiContext = Configuration.GetAPIContext();

            var execution = new PaymentExecution
            {
                payer_id = data.PayerID
            };

            var payment = new Payment
            {
                id = data.paymentId
            };

            var executedPayment = payment.Execute(apiContext, execution);

            return executedPayment;
        }


        public static Payment CreatePaypalPayment(PaypalPaymentPostObj data)
        {

            var apiContext = Configuration.GetAPIContext();

            // ###Payment
            // A Payment Resource; create one using
            // the above types and intent as `sale` or `authorize`
            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "paypal"
                },
                transactions = new List<Transaction> 
                {
                    new Transaction
                    {
                        amount = new Amount
                        {
                            currency = "USD",
                            total = data.total
                        },
                        description = data.description,
                        invoice_number = data.invoice_number,
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    return_url = data.return_url,
                    cancel_url = data.cancel_url
                }
            };


            // Create a payment using a valid APIContext
            var createdPayment = payment.Create(apiContext);

            return createdPayment;
        }

        public static Payment CreatePaypalPaymentStatic()
        {

            var apiContext = Configuration.GetAPIContext();



            // ###Items
            // Items within a transaction.
            var itemList = new ItemList()
            {
                items = new List<Item>() 
                    {
                        new Item()
                        {
                            name = "Item Name",
                            currency = "USD",
                            price = "15",
                            quantity = "5",
                            sku = "sku"
                        }
                    }
            };

            // ###Payer
            // A resource representing a Payer that funds a payment
            // Payment Method
            // as `paypal`
            var payer = new Payer() { payment_method = "paypal" };

            // ###Redirect URLS
            // These URLs will determine how the user is redirected from PayPal once they have either approved or canceled the payment.
            // var baseURI = Request.Url + "://" + Request.Url.Authority + "/PaymentWithPayPal.aspx?";
            var baseURI = HttpContext.Current.Request.Url + "?";
            var guid = Convert.ToString((new Random()).Next(100000));
            var redirectUrl = baseURI + "guid=" + guid;
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&cancel=true",
                return_url = redirectUrl
            };

            // ###Details
            // Let's you specify details of a payment amount.
            var details = new Details()
            {
                tax = "15",
                shipping = "10",
                subtotal = "75"
            };

            // ###Amount
            // Let's you specify a payment amount.
            var amount = new Amount()
            {
                currency = "USD",
                total = "100.00", // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            // ###Transaction
            // A transaction defines the contract of a
            // payment - what is the payment for and who
            // is fulfilling it. 
            var transactionList = new List<Transaction>();

            // The Payment creation API requires a list of
            // Transaction; add the created `Transaction`
            // to a List
            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = "123456",
                amount = amount,
                item_list = itemList
            });

            // ###Payment
            // A Payment Resource; create one using
            // the above types and intent as `sale` or `authorize`
            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };


            // Create a payment using a valid APIContext
            var createdPayment = payment.Create(apiContext);

            //Session.Add(guid, createdPayment.id);
            //Session.Add("flow-" + guid, this.flow);


            return createdPayment;




        }



        public static Payment MakeCreditCardPayment(PaypalPaymentPostObj data)
        {

            var apiContext = Configuration.GetAPIContext();

            // ###Payment
            // A Payment Resource; create one using
            // the above types and intent as `sale` or `authorize`
            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "credit_card",
                    funding_instruments = new List<FundingInstrument>
                    {
                        new FundingInstrument
                        {
                            credit_card_token = new CreditCardToken
                            {
                                credit_card_id = data.card_id
                            }
                        }
                    }
                },
                transactions = new List<Transaction> 
                {
                    new Transaction
                    {
                        amount = new Amount
                        {
                            currency = "USD",
                            total = data.total
                        },
                        description = data.description,
                        invoice_number = data.invoice_number,
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    return_url = data.return_url,
                    cancel_url = data.cancel_url
                }
            };


            // Create a payment using a valid APIContext
            var createdPayment = payment.Create(apiContext);

            return createdPayment;
        }





        public static Payment MakeCardPayment() 
        {


            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            // See [Configuration.cs](/Source/Configuration.html) to know more about APIContext.
            var apiContext = Configuration.GetAPIContext();

            // A transaction defines the contract of a payment - what is the payment for and who is fulfilling it. 
            var transaction = new Transaction()
            {
                amount = new Amount()
                {
                    currency = "USD",
                    total = "7"//,
                    //details = new Details()
                    //{
                    //    shipping = "1",
                    //    subtotal = "5",
                    //    tax = "1"
                    //}
                },
                description = "This is the payment transaction description."//,
                //item_list = new ItemList()
                //{
                //    items = new List<Item>()
                //    {
                //        new Item()
                //        {
                //            name = "Item Name",
                //            currency = "USD",
                //            price = "1",
                //            quantity = "5",
                //            sku = "sku"
                //        }
                //    },
                //    shipping_address = new ShippingAddress
                //    {
                //        city = "Johnstown",
                //        country_code = "US",
                //        line1 = "52 N Main ST",
                //        postal_code = "43210",
                //        state = "OH",
                //        recipient_name = "Joe Buyer"
                //    }
                //},
                //invoice_number = Common.GetRandomInvoiceNumber()
            };

            // A resource representing a Payer that funds a payment.
            var payer = new Payer()
            {
                payment_method = "credit_card",
                funding_instruments = new List<FundingInstrument>()
                {
                    new FundingInstrument()
                    {
                        credit_card = new CreditCard()
                        {
                            //billing_address = new Address()
                            //{
                            //    city = "Johnstown",
                            //    country_code = "US",
                            //    line1 = "52 N Main ST",
                            //    postal_code = "43210",
                            //    state = "OH"
                            //},
                            cvv2 = "874",
                            expire_month = 11,
                            expire_year = 2018,
                            first_name = "Joe",
                            last_name = "Shopper",
                            number = "4877274905927862",
                            type = "visa"
                        }
                    }
                }//,
                //payer_info = new PayerInfo
                //{
                //    email = "sean-buyer@rideshark.com"
                //}
            };

            // A Payment resource; create one using the above types and intent as `sale` or `authorize`
            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = new List<Transaction>() { transaction }
            };


            // Create a payment using a valid APIContext
            var createdPayment = payment.Create(apiContext);


            // For more information, please visit [PayPal Developer REST API Reference](https://developer.paypal.com/docs/api/).



            return createdPayment;


        }


    }
}