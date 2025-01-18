using AutoMapper;
using RealStateApp.Core.Application.Dtos.Mejoras;
using RealStateApp.Core.Application.Dtos.Propiedad;
using RealStateApp.Core.Application.Dtos.TipoPropiedades;
using RealStateApp.Core.Application.Dtos.TipoVentas;
using RealStateApp.Core.Application.Features.Mejoras.Commands.CreateMejora;
using RealStateApp.Core.Application.Features.Mejoras.Commands.UpdateMejora;
using RealStateApp.Core.Application.Features.TipoPropiedades.Commands.CreateTipoPropiedad;
using RealStateApp.Core.Application.Features.TipoPropiedades.Commands.UpdateTipoPropiedad;
using RealStateApp.Core.Application.Features.TipoVentas.Commands.CreateTipoVenta;
using RealStateApp.Core.Application.Features.TipoVentas.Commands.UpdateTipoVenta;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Chat;
using RealStateApp.Core.Application.ViewModels.Favorites;
using RealStateApp.Core.Application.ViewModels.Offers;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Application.ViewModels.Sales;
using RealStateApp.Core.Application.ViewModels.Upgrades;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        private string GetFirstImageOrDefault(Propiedad propiedad)
        {
            return propiedad.Imagenes != null && propiedad.Imagenes.Any()
                ? propiedad.Imagenes.First().UrlImagen
                : "https://via.placeholder.com/300";
        }
        public GeneralProfile()
        {
            #region Agente
            CreateMap<Agente, PropertyAgentViewModel>()
                .ReverseMap();



            CreateMap<Agente, EditAgentViewModel>()
                .ForMember(dest => dest.Nombre, opt => opt.Ignore()) 
                .ForMember(dest => dest.Apellido, opt => opt.Ignore()) 
                .ForMember(dest => dest.Telefono, opt => opt.Ignore()) 
                .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.ProfilePhotoURL))
                .ReverseMap();



            #endregion
            #region Offerta

            CreateMap<Oferta, OfferViewModel>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()));

            
            CreateMap<CreateOfferViewModel, Oferta>();
            #endregion
            #region TipoPropiedad
            CreateMap<TipoPropiedad, TipoPropiedadResponse>()
                .ReverseMap();

            CreateMap<TipoPropiedad, CreateTipoPropiedadCommand>()
               .ReverseMap()
               .ForMember(tp => tp.Id, opt => opt.Ignore());

            CreateMap<TipoPropiedad, UpdateTipoPropiedadCommand>()
               .ReverseMap();

            CreateMap<TipoPropiedad, TipoPropiedadUpdateResponse>()
               .ReverseMap();

            CreateMap<TipoPropiedad, PropertyTypeViewModel>()
                .ReverseMap();
              
            #endregion

            #region TipoVenta
            CreateMap<TipoVenta, TipoVentaResponse>()
                .ReverseMap();

            CreateMap<TipoVenta, CreateTipoVentaCommand>()
               .ReverseMap()
               .ForMember(tv => tv.Id, opt => opt.Ignore());

            CreateMap<TipoVenta, UpdateTipoVentaCommand>()
               .ReverseMap();

            CreateMap<TipoVenta, TipoVentaUpdateResponse>()
               .ReverseMap();

            CreateMap<TipoVenta, SaleTypeViewModel>()
                .ReverseMap()
                .ForMember(x => x.Propiedades, opt => opt.Ignore());

            CreateMap<TipoVenta, SaleTypeHandlerViewModel>()
                .ReverseMap()
                .ForMember(x => x.Propiedades, opt => opt.Ignore());
            #endregion
            #region Chat
            #region Chat
            CreateMap<Chat, ChatViewModel>().ReverseMap();
            CreateMap<Mensaje, MensajeViewModel>().ReverseMap();
            CreateMap<CreateMessageViewModel, Mensaje>()
    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.AgenteId) ? src.AgenteId : src.UserId))
    .ReverseMap();


            #endregion

            #endregion
            #region Mejora
            CreateMap<Mejora, MejoraResponse>()
                .ReverseMap();

            CreateMap<Mejora, CreateMejoraCommand>()
               .ReverseMap()
               .ForMember(m => m.Id, opt => opt.Ignore());

            CreateMap<Mejora, UpdateMejoraCommand>()
               .ReverseMap();

            CreateMap<Mejora, MejoraUpdateResponse>()
               .ReverseMap();

            CreateMap<Mejora, UpgradeViewModel>()
                .ReverseMap()
                .ForMember(x => x.Propiedades, opt => opt.Ignore());

            CreateMap<Mejora, UpgradeViewModel>()
                .ReverseMap()
                .ForMember(x => x.Propiedades, opt => opt.Ignore());

            CreateMap<Mejora, UpgradeHandlerViewModel>()
                .ReverseMap()
                .ForMember(x => x.Propiedades, opt => opt.Ignore());
            #endregion

            #region Propiedad
            CreateMap<Propiedad, PropiedadResponse>()

               .ForMember(dest => dest.TipoPropiedad, opt => opt.MapFrom(src => src.TipoPropiedad))
               .ForMember(dest => dest.TipoVenta, opt => opt.MapFrom(src => src.TipoVenta))
               .ForMember(dest => dest.Mejoras, opt => opt.MapFrom(src => src.Mejoras.Select(m => m.Mejora)))
               .ReverseMap();


               
            CreateMap<Propiedad, PropertyDetailsViewModel>()
                .ForMember(dest => dest.TipoPropiedad, opt => opt.MapFrom(src => src.TipoPropiedad.Nombre))
                .ForMember(dest => dest.TipoVenta, opt => opt.MapFrom(src => src.TipoVenta.Nombre))
                .ForMember(dest => dest.Agente, opt => opt.MapFrom(src => src.Agente))
                .ForMember(dest => dest.Imagenes, opt => opt.MapFrom(src => src.Imagenes.Select(img => img.UrlImagen)))
                .ReverseMap();




            CreateMap<Propiedad, PropiedadViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CodigoPropiedad, opt => opt.MapFrom(src => src.Codigo)) 
                .ForMember(dest => dest.TipoPropiedad, opt => opt.MapFrom(src => src.TipoPropiedad.Nombre ?? "N/A"))
                .ForMember(dest => dest.TipoVenta, opt => opt.MapFrom(src => src.TipoVenta.Nombre ?? "N/A"))
                .ForMember(dest => dest.ValorPropiedad, opt => opt.MapFrom(src => src.Precio))
                .ForMember(dest => dest.CantidadHabitaciones, opt => opt.MapFrom(src => src.Habitaciones))
                .ForMember(dest => dest.CantidadBaños, opt => opt.MapFrom(src => src.Banios))
                .ForMember(dest => dest.TamañoPropiedad, opt => opt.MapFrom(src => (float)src.Terreno))
                .ForMember(dest => dest.Imagenes, opt => opt.MapFrom(src => src.Imagenes.Select(img => img.UrlImagen).ToList()))
                .ForMember(dest => dest.IsFavorite, opt => opt.MapFrom(src => false))
                .ReverseMap();

            CreateMap<CreatePropertyViewModel, Propiedad>()
                .ForMember(dest => dest.Codigo, opt => opt.Ignore()) 
                .ForMember(dest => dest.TipoPropiedadId, opt => opt.MapFrom(src => src.TipoPropiedadId))
                .ForMember(dest => dest.TipoVentaId, opt => opt.MapFrom(src => src.TipoVentaId))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Precio))
                .ForMember(dest => dest.Terreno, opt => opt.MapFrom(src => src.TamañoPropiedad))
                .ForMember(dest => dest.Habitaciones, opt => opt.MapFrom(src => src.Habitaciones))
                .ForMember(dest => dest.Banios, opt => opt.MapFrom(src => src.Banios))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Mejoras, opt => opt.Ignore()) 
                .ForMember(dest => dest.Imagenes, opt => opt.Ignore()) 
                .ReverseMap();








            #endregion

            #region Favorite
            CreateMap<Propiedad, FavoritePropertyViewModel>()
            .ForMember(dest => dest.ImagenUrl, opt => opt.MapFrom(src => GetFirstImageOrDefault(src)))
            .ForMember(dest => dest.NombrePropiedad, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Codigo) ? src.Codigo : "Nombre no disponible"))
            .ForMember(dest => dest.EsFavorita, opt => opt.MapFrom(src => true));
            #endregion

            #region Chat

            CreateMap<Chat, ChatViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.ClienteId))
                .ForMember(dest => dest.AgenteId, opt => opt.MapFrom(src => src.AgenteId))
                .ForMember(dest => dest.PropiedadId, opt => opt.MapFrom(src => src.PropiedadId))
                .ForMember(dest => dest.Mensajes, opt => opt.MapFrom(src => src.Mensajes))
                .ReverseMap();

            CreateMap<CreateChatViewModel, Chat>()
                .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.ClienteId))
                .ForMember(dest => dest.AgenteId, opt => opt.MapFrom(src => src.AgenteId))
                .ForMember(dest => dest.PropiedadId, opt => opt.MapFrom(src => src.PropiedadId))
                .ReverseMap();

            CreateMap<Mensaje, MensajeViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.ChatId))
                .ForMember(dest => dest.Contenido, opt => opt.MapFrom(src => src.Contenido))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created))
                .ReverseMap();

            CreateMap<CreateMessageViewModel, Mensaje>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.ChatId))
                .ForMember(dest => dest.Contenido, opt => opt.MapFrom(src => src.Contenido))
                .ReverseMap();

            #endregion



        }
    }
}
