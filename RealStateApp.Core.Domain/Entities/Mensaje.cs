using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class Mensaje : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public int ChatId { get; set; }
        public string Contenido { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public Chat Chat { get; set; }
    }
}
