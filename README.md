<h2>📚 Library Management System API</h2>

LibraryManagementSystemAPI is a C# ASP.NET Core backend API for managing book borrowing with JWT authentication, role-based authorization, and Stripe payment integration. This system enables authors to manage their books and members to borrow and return books. It also includes automated email notifications and background services for handling overdue returns and payment reconciliation.

<h2>🌟 Features</h2>

    JWT Authentication, Refresh JWT Tokens & Role-Based Authorization:
        Authors and members have different roles and permissions.

    Book Management (Authors):
        Add, update, and delete books.

    Borrowing and Payment Process (Members):
        Borrow a book and pay via Stripe.
        Revenue split: 80% for the platform, 20% for the author.
        Automated handling of failed author Stripe accounts with background reconciliation.

    Overdue Return Handling:
        If a book is not returned within 30 days, a late fee is applied, and an email notification is sent.

    Background Services:
        Monitor overdue books and apply fees.
        Handle failed Stripe account payments for authors.

    Email Notifications:
        Welcome emails for new members and authors.
        Detailed emails for both authors and members regarding borrowing transactions.

    Logging:
        Logs critical events and system activities.

 <h2>🛠️ Technologies Used</h2>

    Backend: ASP.NET Core
    Authentication: JWT Tokens
    Authorization: Role-Based Access Control
    Payments: Stripe API Integration
    Background Services: Hosted Services in ASP.NET Core
    Email Handling: SMTP

 <h2>🚀 Getting Started</h2>
Prerequisites

Make sure you have the following installed:

    .NET SDK (version 8.0 or later)
    SQL Server
    Stripe Account for payment handling
    SMTP Server for sending emails
    some packages: Stripe, Jwt bearer, Identity, Entity Framework Core (Tooles, SQL server)
    Postman (optional, for API testing)

Installation

    Clone the repository:
    git clone https://github.com/your-username/LibraryManagementSystemAPI.git
    cd LibraryManagementSystemAPI

Set up the database:

    Update the connection string in appsettings.json.

    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=yourDb;Trusted_Connection=True;"
    }

Configure Stripe API keys and SMTP:

    Add your Stripe and SMTP credentials in appsettings.json.

        "Stripe": {
          "SecretKey": "your-stripe-secret-key",
          "PublishableKey": "your-stripe-publishable-key"
        },
        "EmailSettings": {
          "SMTPHost": "smtp.your-email.com",
          "SMTPPort": 25,
          "SenderEmail": "your-email@example.com",
          "SenderPassword": "your-email-password"
        }

Apply migrations:

Delete all migrations.
Then, Create a new one.
After that, Update datebase

Then, run the application.

📈 <h2>Future Improvements</h2>

    Add unit tests and integration tests.
    Implement pagination, search and filter functionality for books.
    Enable members and authors to update their profiles.
