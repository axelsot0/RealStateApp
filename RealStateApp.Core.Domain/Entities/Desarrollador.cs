using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class Desarrollador : AuditableBaseEntity
    {
        public string Cedula { get; set; }
        public string UserId { get; set; }
    }
}
