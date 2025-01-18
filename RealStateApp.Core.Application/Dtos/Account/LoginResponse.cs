namespace RealStateApp.Core.Application.Dtos.Account
{
    public class LoginResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
