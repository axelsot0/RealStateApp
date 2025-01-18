using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.DbConfiguration;

namespace RealStateApp.Infraestructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Propiedad> Propiedades { get; set; }
        public DbSet<TipoPropiedad> TiposPropiedades { get; set; }
        public DbSet<TipoVenta> TiposVentas { get; set; }
        public DbSet<Mejora> Mejoras { get; set; }
        public DbSet<PropiedadMejora> PropiedadMejoras { get; set; }
        public DbSet<PropiedadImagen> PropiedadImagenes { get; set; }
        public DbSet<PropiedadCliente> PropiedadesClientes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Agente> Agentes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Desarrollador> Desarrolladores { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigDb();
        }
    }
}
