using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemAPI.Requests;

public class AuthenticationRequest {
    public string Identifier { get; set; }
    public string Password { get; set; }
}