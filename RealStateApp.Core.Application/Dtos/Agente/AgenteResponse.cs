namespace RealStateApp.Core.Application.Dtos.Agente
{
    public class AgenteResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Propiedades { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
