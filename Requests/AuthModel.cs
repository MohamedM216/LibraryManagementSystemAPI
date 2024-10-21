using System.Text.Json.Serialization;

namespace LibraryManagementSystemAPI.Requests;

public class AuthModel {
    public string Message { get; set; }
    public bool IsAuthenticated { get; set; } = false;
    public string? AccessToken { get; set; }
    [JsonIgnore]
    public string? RefreshToken { get; set; }   // save it in Cookies
    public DateTime RefreshTokenExpiration { get; set; }
}