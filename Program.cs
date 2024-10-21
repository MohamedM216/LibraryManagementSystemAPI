using System.Text;
using LibraryManagementSystemAPI.Data;
using LibraryManagementSystemAPI.Filters;
using LibraryManagementSystemAPI.MiddleWares;
using LibraryManagementSystemAPI.Options;
using LibraryManagementSystemAPI.Services;
using LibraryManagementSystemAPI.Services.BackgroundServices;
using LibraryManagementSystemAPI.ValidationClasses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;

/**** to-dos ****
    * validate isbn format correctly                                                    ( Done )
            src: https://chatgpt.com/c/670238a5-dbbc-800e-ad07-1bbda27949a4

    * validate if this book is already found in reality or not(i.e. author's book claim)
            src: pass it for now for simplicity, but it is a must for production :)

    * delete credit card, we don't need it, change author username to email              ( Done )

    * check if stripe email is found in reality for author(createBook()) and member(borrow()) or not ( Done )
            src: event: https://medium.com/@ravipatel.it/understanding-events-in-c-with-practical-examples-8cb5ad547a35

    * payment process integration & validation                                  ( Done )
            src 1: https://softwareparticles.com/stripe-payment-gateway-dotnet/
            src 2: https://chatgpt.com/c/6703d033-dd44-800e-87da-0db5d3cbeee6

    in future:
    
    * send email for success or failed money transfer                         (  )
    * log money transefer process data in details => use logging                (  )
    * log borrowing process details                                             (  )
    * re-check again and see                                                    (  )
*/

// to now: ~2000   lines of code

// test, git hub repo set up

// src: https://chatgpt.com/c/66e93000-257c-800e-b36f-235a2bb07167

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

var config = builder.Configuration;
builder.Services.AddCors(options => 
{
    options.AddPolicy("ReactOrigin", builder =>
    {
        builder.WithOrigins(config["Origins:ReactOrigin:Host"]) // react host
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dependencies
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<BorrowingService>();
builder.Services.AddScoped<ReturnService>();
builder.Services.AddScoped<MemberAuthService>();
builder.Services.AddScoped<JwtOptions>();
builder.Services.AddScoped<AdminAuthService>();
builder.Services.AddScoped<AuthorAuthService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<WelcomeEmailService>();
builder.Services.AddScoped<LateFeeEmailService>();
builder.Services.AddScoped<ValidateStripeEmailAccount>();


// background services
builder.Services.AddSingleton<IHostedService, LateFeeBackgroundService>();


// authentication
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

// payment options
builder.Services.Configure<StripeInfo>(builder.Configuration.GetSection("Stripe"));

// jwt authentication
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => 
    {
        options.SaveToken = true;   // to allow getting token string from HttpContext
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,  // to expire token directly after finishing its determined duration
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
        };
    });

// connect to db
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// add action filters
builder.Services.AddControllers(options => 
{
    options.Filters.Add<LoggingActionMethodsInfoFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// never allow swagger in production 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactOrigin");

app.UseMiddleware<ProfilingMiddleware>();

// payment integration with stripe
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
