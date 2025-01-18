using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    public IFormFile Photo { get; set; }
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required, Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Role { get; set; }
}
