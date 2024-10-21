
using PayPalCheckoutSdk.Core;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System;
using System.Threading.Tasks;
using PayPalCheckoutSdk;
using PayPalCheckoutSdk.Orders;
using PayPalCheckoutSdk.Payments;
using PayPalHttp;
using LibraryManagementSystemAPI.DTOModels;


namespace LibraryManagementSystemAPI.Services;

            // src: https://chatgpt.com/c/67060fec-92a8-800e-b57e-1a024854f751
public class PaymentService 
{

    // step 1
    // if CustomerId == null => create customer id 
    public string CreateCustomer(string memberEmail, string name)
    {
        var options = new CustomerCreateOptions
        {
            Email = memberEmail,
            Description = "Library Member",
            Name = name
        };
        var service = new CustomerService();
        var customer = service.Create(options);

        // Store customer.Id in your database for future transactions
        return customer.Id;
    }

    // charge customer (from customer to platform)
    // step 2
    public DtoPaymentIntent CreatePaymentIntent(long amountInCent)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = amountInCent,
            Currency = "usd",
        };
        var service = new PaymentIntentService();
        var paymentIntent = service.Create(options);

        return new DtoPaymentIntent 
        { 
            PaymentIntentId = paymentIntent.Id,
            PaymentIntentClientSecret = paymentIntent.ClientSecret
        };
    }

    // step 3
    // then front-end role appears
    // front-end will use paymentIntent.ClientSecret + collect members's credit card info
    // then send payment method Id to the server again after calling stripe server and receive the 
    // payment method id from it

    // use payment method id to confirm 
    // step 4

    public bool ConfirmPayment(string paymentIntentId, string paymentMethodId)
    {
        var service = new PaymentIntentService();
        var options = new PaymentIntentConfirmOptions
        {
            PaymentMethod = paymentMethodId,
        };
        var paymentIntent = service.Confirm(paymentIntentId, options);

        if (paymentIntent.Status == "succeeded")
        {
            // Payment was successful
            return true;
        }

        // Handle payment failure
        // log the error
        return false;
    }

}

