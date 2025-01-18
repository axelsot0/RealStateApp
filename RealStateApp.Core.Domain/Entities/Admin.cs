using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class Admin : AuditableBaseEntity
    {
        public string Cedula { get; set; }
        public string UserId { get; set; }
    }
}
