using LibraryManagementSystemAPI.DTOModels;
using Stripe;

namespace LibraryManagementSystemAPI.ValidationClasses;

public class ValidateStripeEmailAccount {
    public DtoCustomer IsStripeEmailFound(string stripeEmail) {
        var options = new CustomerListOptions
        {
            Limit = 1,
            Email = stripeEmail,
        };

        var service = new CustomerService();
        var customer = service.List(options);

        if (customer.Data.Count > 0) {
            var customerObject = new DtoCustomer
            {
                Name = customer.Data[0].Name,
                Email = customer.Data[0].Email,
                Balance = customer.Data[0].Balance,
                Id = customer.Data[0].Id,
            };
            return customerObject;
        }
        return null;
    }    
}