

using LibraryManagementSystemAPI.Data;

namespace LibraryManagementSystemAPI.Services.BackgroundServices;

public class LateFeeBackgroundService(IServiceProvider serviceProvider) : BackgroundService, IHostedService
{
    private const decimal LATE_FEE_VALUE = 100; // in cent

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var lateFeeEmailService = scope.ServiceProvider.GetRequiredService<LateFeeEmailService>();
                
                var allBorrowingsRecords = dbContext.Borrowings.Where(b => b.ReturnDate == null).ToList();
                foreach(var record in allBorrowingsRecords) {
                    if (record.BorrowDate.AddDays(30) > DateTime.Now) {
                        var member = dbContext.Members.FirstOrDefault(m => m.Id == record.MemberId);
                        if (member.LateFee < LATE_FEE_VALUE) {
                            member.LateFee += LATE_FEE_VALUE;
                            dbContext.Update(member);
                            try {

                                await lateFeeEmailService.SendLateFeeWarningEmailAsync(member.Email, member.FirstName + " " + member.LastName, LATE_FEE_VALUE, DateTime.Now);
                            } catch (Exception ex)
                            {
                                Console.WriteLine($"ERROR: sending email failed! --- {ex.Message}");
                            }
                        }
                    }
                }
            }

            await Task.Delay(TimeSpan.FromHours(6), stoppingToken); // checl every 6 hours
        }
    }
}