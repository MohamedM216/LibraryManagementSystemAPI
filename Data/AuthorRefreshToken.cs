using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystemAPI.Data;

public class AuthorRefreshToken
{
    public int Id { get; set; }
    public Author Author { get; set; }
    public int AuthorId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    public DateTime CreatedOn { get; set; }
    public DateTime RevokedOn { get; set; }
    public bool IsActive => RevokedOn == null && !IsExpired;
}