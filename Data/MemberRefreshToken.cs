using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystemAPI.Data;

public class MemberRefreshToken
{
    public int Id { get; set; }
    public Member Member { get; set; }
    public int MemberId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    public DateTime CreatedOn { get; set; }
    public DateTime RevokedOn { get; set; }
    public bool IsActive => RevokedOn == null && !IsExpired;
}