using LibraryManagementSystemAPI.Data;
using LibraryManagementSystemAPI.DTOModels;
using LibraryManagementSystemAPI.ValidationClasses;
using Stripe;

namespace LibraryManagementSystemAPI.Services;

public class BorrowingService
{
    private readonly ApplicationDbContext _context;
    private readonly int LEAST_NUMBER_OF_COPIES_ALLOWED = 1;
    private readonly PaymentService _paymentService;
    private readonly ValidateStripeEmailAccount _validateStripeEmailAccount;
    private readonly IConfiguration _configuration;
    private const long DELIVERY_COST_IN_CENT = 1000;
    private const long AUTHOR_PROFIT_IN_CENT = 200;

    public BorrowingService(ApplicationDbContext context, PaymentService paymentService, ValidateStripeEmailAccount validateStripeEmailAccount, IConfiguration configuration) {
        _context = context;
        _paymentService = paymentService;
        _validateStripeEmailAccount = validateStripeEmailAccount;
        _configuration = configuration;
    }

    

    public bool BorrowBook(int currentMemberId, string ISBN) {
        var existingMember = _context.Members.SingleOrDefault(member => member.Id == currentMemberId);
        if (existingMember == null) {
            Console.WriteLine("current user object is null");
            return false;
        }

        // check if customer & our website stripe emails are valid or not
        var customerObject = _validateStripeEmailAccount.IsStripeEmailFound(existingMember.StripeEmail);
        if (customerObject == null) {
            Console.WriteLine("you don't have stripe account");
            return false;
        }

        var OurWebsiteCustomerObject = _validateStripeEmailAccount.IsStripeEmailFound(_configuration["Stripe:Email"]);
        if (OurWebsiteCustomerObject == null) {
            Console.WriteLine("Problem in the server side");
            return false;
        }

        var existingBook = _context.Books.SingleOrDefault(book => book.ISBN == ISBN);

        if (existingBook == null) {
            Console.WriteLine("book not found");
            return false;   // not found
        }
        if (existingBook.CopiesAvailable < LEAST_NUMBER_OF_COPIES_ALLOWED) {
            Console.WriteLine("no copies found from this book!");
            return false;   // not found
        }

        var existingBorrowing = _context.Borrowings
            .SingleOrDefault(borrow => borrow.BookId == existingBook.Id && borrow.MemberId == currentMemberId);
        
        if (existingBorrowing != null && existingBorrowing.ReturnDate == null) {
            Console.WriteLine("you still borrow this book! you can't borrow again before you return it");
            return false;   // not allowed
        }

        // payment & payout process
        try
        {
            // step 1
            var customerId = "";
            if (string.IsNullOrEmpty(existingMember.CustomerId)) {
                customerId = _paymentService.CreateCustomer(existingMember.StripeEmail, existingMember.FirstName + " " + existingMember.LastName);
                if (string.IsNullOrEmpty(customerId)) {
                    Console.WriteLine("invalid inputs! create stripe customer id failed");
                    return false;
                }
            } 
            else 
                customerId = existingMember.CustomerId;

            // step 2
            var paymentIntent = _paymentService.CreatePaymentIntent(DELIVERY_COST_IN_CENT);
            if (paymentIntent == null || string.IsNullOrEmpty(paymentIntent.PaymentIntentId) || string.IsNullOrEmpty(paymentIntent.PaymentIntentClientSecret)) {
                Console.WriteLine("invalid inputs! create Payment Intent failed");
                return false;
            }
            // front end receive values it needs
            // then 
            // step 3 comes
            var paymentMethodId = "comes_from_front_end";
            var isDone = _paymentService.ConfirmPayment(paymentIntent.PaymentIntentId, paymentMethodId);
            if (!isDone) {
                Console.WriteLine("payment confirmation failed");
                return false;
            }

            var transaction = new Transaction
            {
                Date = DateTime.Now,
                bookId = existingBook.Id,
                MemberId = existingMember.Id,
                CustomerId = existingMember.CustomerId,
                AuthorId = existingBook.AuthorId,
                DeliveryCost = DELIVERY_COST_IN_CENT,
                AuthorPayout = AUTHOR_PROFIT_IN_CENT,
                PaymentIntentId = paymentIntent.PaymentIntentId
            };
            _context.Add(transaction);
            _context.SaveChanges();
            Console.WriteLine("payment process done!");

            ++existingBook.CopiesAvailable;
            _context.Update(existingBook);
            _context.SaveChanges();
            Console.WriteLine("borrowing process done!");

            var existingAuthor = _context.Authors.FirstOrDefault(a => a.Id == existingBook.AuthorId);
            if (existingAuthor == null) {
                Console.WriteLine("author not found! adding author's due failed");
                return false;
            }

            var authorDue = new AuthorsDue
            {
                AuthorId = existingAuthor.Id,
                PaypalEmail = existingAuthor.PaypalEmail,
                DueAuthorPayout = AUTHOR_PROFIT_IN_CENT,
                TransactionId = transaction.Id,
                IsDuePaid = false
            };
            _context.Add(authorDue);
            _context.SaveChanges();
            Console.WriteLine("adding author's due done");
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occured: {ex.Message} --- on source: {ex.Source}");
        }

        return true;   // added successfully
    }

}