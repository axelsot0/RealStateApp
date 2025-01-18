using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Infraestructure.Persistence.DbConfiguration
{
    public static class DbConfig
    {
        public static void ConfigDb(this ModelBuilder modelBuilder)
        {
            #region Propiedad
            modelBuilder.Entity<Propiedad>().ToTable("Propiedades");
            modelBuilder.Entity<Propiedad>().HasKey(p => p.Id);
            modelBuilder.Entity<Propiedad>().Property(p => p.Codigo).HasMaxLength(6).IsRequired();
            modelBuilder.Entity<Propiedad>().Property(p => p.TipoPropiedadId).IsRequired();
            modelBuilder.Entity<Propiedad>().Property(p => p.TipoVentaId).IsRequired();
            modelBuilder.Entity<Propiedad>().Property(p => p.AgenteId).IsRequired();
            modelBuilder.Entity<Propiedad>().Property(p => p.Habitaciones).IsRequired();
            modelBuilder.Entity<Propiedad>().Property(p => p.Banios).IsRequired();
            modelBuilder.Entity<Propiedad>().Property(p => p.Descripcion).IsRequired();
            modelBuilder.Entity<Propiedad>().Property(p => p.Estado).IsRequired();
            modelBuilder.Entity<Propiedad>().Property(p => p.Precio).HasColumnType("decimal(12,4)").IsRequired();
            modelBuilder.Entity<Propiedad>().Property(p => p.Terreno).HasColumnType("decimal(12,2)").IsRequired();

            modelBuilder.Entity<Propiedad>()
                        .HasMany(p => p.Imagenes)
                        .WithOne(i => i.Propiedad)
                        .HasForeignKey(i => i.PropiedadId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Propiedad>()
                       .HasMany(p => p.Ofertas)
                       .WithOne(o => o.Propiedad)
                       .HasForeignKey(o => o.PropiedadId)
                       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Propiedad>()
                       .HasMany(p => p.Chats)
                       .WithOne(c => c.Propiedad)
                       .HasForeignKey(c => c.PropiedadId)
                       .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region TipoPropiedad
            modelBuilder.Entity<TipoPropiedad>().ToTable("TiposPropiedades");
            modelBuilder.Entity<TipoPropiedad>().HasKey(tp => tp.Id);
            modelBuilder.Entity<TipoPropiedad>().Property(tp => tp.Nombre).IsRequired();
            modelBuilder.Entity<TipoPropiedad>().Property(tp => tp.Descripcion).IsRequired();

            modelBuilder.Entity<TipoPropiedad>()
                        .HasMany(tp => tp.Propiedades)
                        .WithOne(p => p.TipoPropiedad)
                        .HasForeignKey(p => p.TipoPropiedadId)
                        .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region TipoVenta
            modelBuilder.Entity<TipoVenta>().ToTable("TiposVentas");
            modelBuilder.Entity<TipoVenta>().HasKey(tv => tv.Id);
            modelBuilder.Entity<TipoVenta>().Property(tv => tv.Nombre).IsRequired();
            modelBuilder.Entity<TipoVenta>().Property(tv => tv.Descripcion).IsRequired();

            modelBuilder.Entity<TipoVenta>()
                        .HasMany(tv => tv.Propiedades)
                        .WithOne(p => p.TipoVenta)
                        .HasForeignKey(p => p.TipoVentaId)
                        .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Mejora
            modelBuilder.Entity<Mejora>().ToTable("Mejoras");
            modelBuilder.Entity<Mejora>().HasKey(m => m.Id);
            modelBuilder.Entity<Mejora>().Property(m => m.Nombre).IsRequired();
            modelBuilder.Entity<Mejora>().Property(m => m.Descripcion).IsRequired();
            #endregion

            #region Imagen
            modelBuilder.Entity<PropiedadImagen>().ToTable("PropiedadImagenes");
            modelBuilder.Entity<PropiedadImagen>().HasKey(pi => pi.Id);
            modelBuilder.Entity<PropiedadImagen>().Property(pi => pi.PropiedadId).IsRequired();
            modelBuilder.Entity<PropiedadImagen>().Property(pi => pi.UrlImagen).IsRequired();
            #endregion

            #region Oferta
            modelBuilder.Entity<Oferta>().ToTable("Ofertas");
            modelBuilder.Entity<Oferta>().HasKey(o => o.Id);
            modelBuilder.Entity<Oferta>().Property(o => o.ClienteId).IsRequired();
            modelBuilder.Entity<Oferta>().Property(o => o.PropiedadId).IsRequired();
            modelBuilder.Entity<Oferta>().Property(o => o.Created).IsRequired();
            modelBuilder.Entity<Oferta>().Property(o => o.Estado).IsRequired();
            modelBuilder.Entity<Oferta>().Property(o => o.Cifra).HasColumnType("decimal(12,4)").IsRequired();
            #endregion

            #region Chat
            modelBuilder.Entity<Chat>().ToTable("Chats");
            modelBuilder.Entity<Chat>().HasKey(c => c.Id);
            modelBuilder.Entity<Chat>().Property(c => c.ClienteId).IsRequired();
            modelBuilder.Entity<Chat>().Property(c => c.AgenteId).IsRequired();
            modelBuilder.Entity<Chat>().Property(c => c.PropiedadId).IsRequired();

            modelBuilder.Entity<Chat>()
                        .HasMany(c => c.Mensajes)
                        .WithOne(m => m.Chat)
                        .HasForeignKey(m => m.ChatId)
                        .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Mensaje
            modelBuilder.Entity<Mensaje>().ToTable("Mensajes");
            modelBuilder.Entity<Mensaje>().HasKey(m => m.Id);
            modelBuilder.Entity<Mensaje>().Property(m => m.Contenido).IsRequired();
            modelBuilder.Entity<Mensaje>().Property(m => m.UserId).IsRequired();
            modelBuilder.Entity<Mensaje>().Property(m => m.Created).IsRequired();
            modelBuilder.Entity<Mensaje>().Property(m => m.ChatId).IsRequired();
            #endregion

            #region Many To Many Relantionships
            modelBuilder.Entity<PropiedadMejora>().ToTable("PropiedadMejoras");
            modelBuilder.Entity<PropiedadMejora>().HasKey(pm => new { pm.PropiedadId, pm.MejoraId });
            modelBuilder.Entity<PropiedadMejora>().Property(pm => pm.PropiedadId).IsRequired();
            modelBuilder.Entity<PropiedadMejora>().Property(pm => pm.MejoraId).IsRequired();

            modelBuilder.Entity<PropiedadMejora>()
                        .HasOne(pm => pm.Propiedad)
                        .WithMany(p => p.Mejoras)
                        .HasForeignKey(pm => pm.PropiedadId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PropiedadMejora>()
                        .HasOne(pm => pm.Mejora)
                        .WithMany(m => m.Propiedades)
                        .HasForeignKey(pm => pm.MejoraId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PropiedadCliente>().ToTable("PropiedadesClientes");
            modelBuilder.Entity<PropiedadCliente>().HasKey(pc => new { pc.PropiedadId, pc.ClienteId });
            modelBuilder.Entity<PropiedadCliente>().Property(pc => pc.PropiedadId).IsRequired();
            modelBuilder.Entity<PropiedadCliente>().Property(pc => pc.ClienteId).IsRequired();

            modelBuilder.Entity<PropiedadCliente>()
                        .HasOne(pc => pc.Propiedad)
                        .WithMany(p => p.PropiedadClientes)
                        .HasForeignKey(pc => pc.PropiedadId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PropiedadCliente>()
                        .HasOne(pm => pm.Cliente)
                        .WithMany(m => m.Favoritas)
                        .HasForeignKey(pm => pm.ClienteId)
                        .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Cliente
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Cliente>().HasKey(c => c.Id);
            modelBuilder.Entity<Cliente>().Property(c => c.UserId).IsRequired();
            modelBuilder.Entity<Cliente>().Property(c => c.ProfilePhotoURL).IsRequired();
            #endregion

            #region Agente
            modelBuilder.Entity<Agente>().ToTable("Agentes");
            modelBuilder.Entity<Agente>().HasKey(a => a.Id);
            modelBuilder.Entity<Agente>().Property(a => a.UserId).IsRequired();
            modelBuilder.Entity<Agente>().Property(a => a.ProfilePhotoURL).IsRequired();

            modelBuilder.Entity<Agente>()
                        .HasMany(a => a.Propiedades)
                        .WithOne(p => p.Agente)
                        .HasForeignKey(p => p.AgenteId)
                        .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Admin
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Admin>().HasKey(a => a.Id);
            modelBuilder.Entity<Admin>().Property(a => a.UserId).IsRequired();
            modelBuilder.Entity<Admin>().Property(a => a.Cedula).IsRequired();
            #endregion

            #region Desarrollador
            modelBuilder.Entity<Desarrollador>().ToTable("Desarrolladores");
            modelBuilder.Entity<Desarrollador>().HasKey(d => d.Id);
            modelBuilder.Entity<Desarrollador>().Property(d => d.UserId).IsRequired();
            modelBuilder.Entity<Desarrollador>().Property(d => d.Cedula).IsRequired();
            #endregion
        }
    }
}
