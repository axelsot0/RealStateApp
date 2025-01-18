using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Chat;
using RealStateApp.Core.Application.ViewModels.Offers;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Enums;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RealStateApp.WebApp.Controllers
{

    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IAgentService _agentService;
        private readonly IChatService _chatService;
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Cliente> _clienteRepo;
        private readonly IAccountService _accountService;
        private readonly IGenericRepository<Agente> _agenteRepo;

        public PropertyController(IPropertyService propertyService, IAgentService agentService, IChatService chatService, IOfferService offerService, IMapper mapper, IGenericRepository<Cliente> clienteRepo, IAccountService accountService, IGenericRepository<Agente> agenteRepo)
        {
            _propertyService = propertyService;
            _agentService = agentService;
            _chatService = chatService;
            _offerService = offerService;
            _mapper = mapper;
            _clienteRepo = clienteRepo;
            _accountService = accountService;
            _agenteRepo = agenteRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            
            var propertyDetails = await GetPropertyDetailsAsync(id);

            if (propertyDetails == null)
            {
                Debug.WriteLine("El modelo propertyDetails es null.");
                return NotFound();
            }

            
            var user = HttpContext.User;

            if (user.Identity.IsAuthenticated && user.IsInRole("Cliente"))
            {
                await HandleClientAsync(id, propertyDetails);
                
                foreach (var chat in propertyDetails.Chats)
                {
                    foreach (var mensaje in chat.Mensajes)
                    {
                        var sender = await _accountService.GetAccountByUserIdAsync(mensaje.UserId);
                        if (sender != null)
                        {
                            mensaje.UserName = $"{sender.FirstName} {sender.LastName}";
                        }
                    }
                }

            }
            else if (user.Identity.IsAuthenticated && user.IsInRole("Agente"))
            {
                var userId = User.FindFirst("UserId")?.Value;
                var agente = await _agenteRepo.GetQuery().FirstOrDefaultAsync(c => c.UserId == userId);
                if (agente != null && propertyDetails.Agente.Id == agente.Id)
                {
                    
                    await HandleAgentAsync(id, propertyDetails);
                    ViewBag.IsOwnProperty = true;
                    if (propertyDetails.Offers == null || !propertyDetails.Offers.Any())
                    {
                        var ofertas = await _offerService.GetOffersByPropertyIdAsync(id);
                        if (ofertas != null && ofertas.Any())
                        {
                            propertyDetails.Offers = ofertas;
                        }
                    }
                }
                else
                {
                    
                    ViewBag.IsOwnProperty = false;
                }
                
                
            }
            else
            {
                ViewBag.Rol = "NoAutenticado";
                propertyDetails.Chats = null;
                propertyDetails.Offers = null;
            }

            propertyDetails.Agente ??= new PropertyAgentViewModel();
            propertyDetails.Imagenes ??= new List<string>();
            propertyDetails.Mejoras ??= new List<string>();
            propertyDetails.Chats ??= new List<ChatViewModel>();
            propertyDetails.Offers ??= new List<OfferViewModel>();

            return View(propertyDetails);
        }

        
        private async Task<PropertyDetailsViewModel> GetPropertyDetailsAsync(int id)
        {
            
            var propertyDetails = await _propertyService.GetPropertyDetailsByIdAsync(id);

            
            propertyDetails.Agente ??= new PropertyAgentViewModel();
            propertyDetails.Imagenes ??= new List<string>();
            propertyDetails.Mejoras ??= new List<string>();
            propertyDetails.Offers ??= new List<OfferViewModel>();

            
            propertyDetails.Chats ??= new List<ChatViewModel>();

           
            var clienteId = User.FindFirst("UserId")?.Value;
            if (!string.IsNullOrEmpty(clienteId))
            {
                
                var cliente = await _clienteRepo.GetQuery().FirstOrDefaultAsync(c => c.UserId == clienteId);
                var usuario = await _accountService.GetAccountByUserIdAsync(clienteId);

                if (usuario != null && propertyDetails.Agente != null && propertyDetails.Agente.Id != null)
                {
                    var chat = await _chatService.GetChatByParticipantsAndPropertyAsync(usuario.Id, propertyDetails.Agente.Id.ToString(), id);

                    if (chat != null)
                    {
                        
                        var chatViewModel = _mapper.Map<ChatViewModel>(chat);
                        propertyDetails.Chats.Add(chatViewModel);
                    }
                }
            }


            var ofertas = await _offerService.GetOffersByPropertyIdAsync(id);
            if (ofertas != null && ofertas.Any())
            {
                Debug.WriteLine($"Número de ofertas obtenidas: {ofertas.Count}");
                propertyDetails.Offers = ofertas;
            }
            else
            {
                Debug.WriteLine("No se encontraron ofertas para la propiedad.");
            }


            return propertyDetails;
        }




        private async Task HandleClientAsync(int propertyId, PropertyDetailsViewModel propertyDetails)
        {
            var clienteId = User.FindFirst("UserId")?.Value;
            var cliente = await _clienteRepo.GetQuery().FirstOrDefaultAsync(c => c.UserId == clienteId);
            var usuario = await _accountService.GetAccountByUserIdAsync(clienteId);

            
            if (usuario == null || propertyDetails.Agente == null || propertyDetails.Agente.Id == null)
            {
                Debug.WriteLine("Usuario o Agente no encontrados");
                return;
            }

            
            propertyDetails.Chats ??= new List<ChatViewModel>();

           
            var chat = await _chatService.GetChatByParticipantsAndPropertyAsync(usuario.Id, propertyDetails.Agente.Id.ToString(), propertyId);

            if (chat == null)
            {
                
                var createChatViewModel = new CreateChatViewModel
                {
                    ClienteId = usuario.Id,
                    AgenteId = propertyDetails.Agente.Id,
                    PropiedadId = propertyId
                };
                await _chatService.CreateChatAsync(createChatViewModel);
                chat = await _chatService.GetChatByParticipantsAndPropertyAsync(usuario.Id, propertyDetails.Agente.Id.ToString(), propertyId);
            }

            if (chat != null)
            {
                
                var mensajes = await _chatService.GetMessagesByChatIdAsync(chat.Id);
                chat.Mensajes = mensajes ?? new List<MensajeViewModel>();

                
                var chatViewModel = _mapper.Map<ChatViewModel>(chat);
                propertyDetails.Chats.Add(chatViewModel);  
            }


            
            var offers = await _offerService.GetOffersByPropertyIdAndClientIdAsync(propertyId, cliente.Id) ?? new List<OfferViewModel>();
            propertyDetails.Offers = offers;

           
            ViewBag.DisableNewOffer = offers.Any(o => o.Estado == EstadoOferta.Aceptada) ||
                                      offers.Any(o => o.ClienteId == cliente.Id && o.Estado == EstadoOferta.Pendiente);

            Debug.WriteLine($"PropertyId: {propertyId}");
        }




        private async Task HandleAgentAsync(int propertyId, PropertyDetailsViewModel propertyDetails)
        {
            var Id = User.FindFirst("UserId")?.Value;
            var agente = await _agenteRepo.GetQuery().FirstOrDefaultAsync(c => c.UserId == Id);
            var usuario = await _accountService.GetAccountByUserIdAsync(agente.UserId);

            
            var chats = await _chatService.GetChatsByPropertyIdAsync(propertyId);

            
            propertyDetails.Chats ??= new List<ChatViewModel>();

            foreach (var chat in chats)
            {
                var clienteId = chat.ClienteId;

                if (!string.IsNullOrEmpty(clienteId))
                {
                    try
                    {
                        var cliente = await _accountService.GetAccountByUserIdAsync(clienteId);
                        if (cliente != null)
                        {
                           
                            var mensajes = await _chatService.GetMessagesByChatIdAsync(chat.Id);
                            chat.Mensajes = mensajes ?? new List<MensajeViewModel>();

                            
                            if (chat.Mensajes.Any())
                            {
                                foreach (var mensaje in chat.Mensajes)
                                {
                                    var sender = await _accountService.GetAccountByUserIdAsync(mensaje.UserId);
                                    if (sender != null)
                                    {
                                        mensaje.UserName = $"{sender.FirstName} {sender.LastName}";
                                    }
                                }
                            }

                            var chatViewModel = _mapper.Map<ChatViewModel>(chat);

                            propertyDetails.Chats.Add(chatViewModel);
                        }
                        else
                        {
                            Debug.WriteLine($"Cliente no encontrado para ChatId: {chat.Id}, ClienteId: {clienteId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error al obtener datos del cliente: {ex.Message}");
                    }
                }
            }
            var ofertas = await _offerService.GetOffersByPropertyIdAsync(propertyId);
            if (ofertas != null && ofertas.Any())
            {
                propertyDetails.Offers = ofertas;
            }
            else
            {
                Debug.WriteLine("No se encontraron ofertas para la propiedad.");
            }

        }













        [HttpPost]
        [Authorize(Roles = "Cliente,Agente")]
        public async Task<IActionResult> SendMessage(int chatId, string contenido, int propiedadId)
        {
            if (string.IsNullOrEmpty(contenido))
            {
                TempData["ErrorMessage"] = "El mensaje no puede estar vacío.";
                return RedirectToAction(nameof(Details), new { id = chatId });
            }

            var userId = User.FindFirst("UserId")?.Value;  
            var usuario = await _accountService.GetAccountByUserIdAsync(userId);

            if (usuario == null)
            {
                Debug.WriteLine("Usuario no encontrado");
                return NotFound();
            }

            string agenteId = null;

            if (User.IsInRole("Agente"))
            {
                agenteId = userId; 
            }

            var messageViewModel = new CreateMessageViewModel
            {
                UserId = userId,
                ChatId = chatId,
                Contenido = contenido,
                AgenteId = agenteId 
            };

            await _chatService.AddMessageToChatAsync(messageViewModel);


            
            
            return RedirectToAction(nameof(Details), new { id = propiedadId });
        }





        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> SubmitOffer(int propertyId, decimal offerAmount)
        {
            if (offerAmount <= 0)
            {
                TempData["ErrorMessage"] = "El monto de la oferta debe ser mayor a 0.";
                return RedirectToAction(nameof(Details), new { id = propertyId });
            }

            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cliente = await _clienteRepo.GetQuery().FirstOrDefaultAsync(c => c.UserId == userId);
            if (cliente == null)
            {
                ModelState.AddModelError(string.Empty, "Cliente no encontrado.");
                return RedirectToAction(nameof(Details), new { id = propertyId });
            }

            
            var acceptedOffer = await _offerService.GetAcceptedOfferByPropertyIdAsync(propertyId);
            if (acceptedOffer != null)
            {
                TempData["ErrorMessage"] = "No puedes realizar una nueva oferta ya que existe una oferta aceptada para esta propiedad.";
                return RedirectToAction(nameof(Details), new { id = propertyId });
            }

            
            var newOffer = new OfferViewModel
            {
                ClienteId = cliente.Id,
                PropiedadId = propertyId,
                Cifra = offerAmount,
                Estado = EstadoOferta.Pendiente,
                Created = DateTime.Now
            };

            await _offerService.CreateOfferAsync(newOffer);

            TempData["SuccessMessage"] = "Tu oferta ha sido enviada exitosamente.";
            return RedirectToAction(nameof(Details), new { id = propertyId });
        }

        [HttpPost]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> AcceptOffer(int offerId)
        {
            var offer = await _offerService.GetByIdAsync(offerId);
            if (offer == null)
            {
                return NotFound();
            }

            offer.Estado = EstadoOferta.Aceptada;
            await _offerService.UpdateAsync(offer);

            TempData["SuccessMessage"] = "La oferta ha sido aceptada exitosamente.";
            return RedirectToAction(nameof(Details), new { id = offer.PropiedadId });
        }

        [HttpPost]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> RejectOffer(int offerId)
        {
            var offer = await _offerService.GetByIdAsync(offerId);
            if (offer == null)
            {
                return NotFound();
            }

            offer.Estado = EstadoOferta.Rechazada;
            await _offerService.UpdateAsync(offer);

            TempData["SuccessMessage"] = "La oferta ha sido rechazada exitosamente.";
            return RedirectToAction(nameof(Details), new { id = offer.PropiedadId });
        }







        [HttpGet]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> AgentProperties()
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var agent = await _agentService.GetAgenteByUserIdAsync(userId);

            if (agent == null)
                return NotFound("Agente no encontrado.");

            var properties = await _propertyService.GetPropertiesByAgentIdAsync(agent.Id);

            return View(properties);
        }

        [HttpGet]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> CreateProperty()
        {
            var tiposPropiedades = await _propertyService.GetTiposPropiedadAsync();
            var tiposVentas = await _propertyService.GetTiposVentasAsync(); 
            var mejoras = await _propertyService.GetAllMejorasAsync(); 

            if (!tiposPropiedades.Any() || !tiposVentas.Any() || !mejoras.Any())
            {
                TempData["ErrorMessage"] = "Debe haber tipos de propiedades, tipos de ventas y mejoras creadas antes de crear una propiedad.";
                return RedirectToAction(nameof(AgentProperties));
            }

            ViewBag.TiposPropiedades = tiposPropiedades;
            ViewBag.TiposVentas = tiposVentas;
            ViewBag.Mejoras = mejoras;

            return View(new CreatePropertyViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> CreateProperty(CreatePropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                
                ViewBag.TiposPropiedades = await _propertyService.GetTiposPropiedadAsync();
                ViewBag.TiposVentas = await _propertyService.GetTiposVentasAsync();
                ViewBag.Mejoras = await _propertyService.GetAllMejorasAsync();

                TempData["ErrorMessage"] = "Por favor, corrija los errores del formulario.";
                return View(model); 
            }
            if (model.Imagenes == null || !model.Imagenes.Any())
            {
                ViewBag.TiposPropiedades = await _propertyService.GetTiposPropiedadAsync();
                ViewBag.TiposVentas = await _propertyService.GetTiposVentasAsync();
                ViewBag.Mejoras = await _propertyService.GetAllMejorasAsync();

                ModelState.AddModelError("Imagenes", "Debe subir al menos una imagen.");
                TempData["ErrorMessage"] = "Por favor, corrija los errores del formulario.";
                return View(model);
            }

            var userId = User.FindFirst("UserId")?.Value;
            var agent = await _agentService.GetAgenteByUserIdAsync(userId);

            if (agent == null)
                return Unauthorized();

            var property = _mapper.Map<Propiedad>(model);
            property.AgenteId = agent.Id;
            property.Codigo = GenerateUniqueCode();

            await _propertyService.CreatePropertyAsync(property, model.Imagenes, model.MejoraIds);

            return RedirectToAction(nameof(AgentProperties));
        }

        private string GenerateUniqueCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }


        [HttpGet]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> EditProperty(int id)
        {
            var propertyDetails = await _propertyService.GetPropertyDetailsByIdAsync(id);

            if (propertyDetails == null)
                return NotFound();

            var tiposPropiedades = await _propertyService.GetTiposPropiedadAsync();
            var tiposVentas = await _propertyService.GetTiposVentasAsync();
            var mejoras = await _propertyService.GetAllMejorasAsync();

            if (!tiposPropiedades.Any() || !tiposVentas.Any() || !mejoras.Any())
            {
                TempData["ErrorMessage"] = "Debe haber tipos de propiedades, tipos de ventas y mejoras creadas antes de editar una propiedad.";
                return RedirectToAction(nameof(AgentProperties));
            }

            
            ViewBag.TiposPropiedades = tiposPropiedades.Select(tp => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = tp.Id.ToString(),
                Text = tp.Nombre
            });

            ViewBag.TiposVentas = tiposVentas.Select(tv => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = tv.Id.ToString(),
                Text = tv.Nombre
            });

            ViewBag.Mejoras = mejoras.Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Nombre,
                Selected = propertyDetails.Mejoras.Contains(m.Nombre)
            });

            
            var editViewModel = new EditPropertyViewModel
            {
                Id = propertyDetails.Id,
                TipoPropiedadId = tiposPropiedades.FirstOrDefault(tp => tp.Nombre == propertyDetails.TipoPropiedad)?.Id ?? 0,
                TipoVentaId = tiposVentas.FirstOrDefault(tv => tv.Nombre == propertyDetails.TipoVenta)?.Id ?? 0,
                Precio = propertyDetails.Precio,
                Descripcion = propertyDetails.Descripcion,
                TamañoPropiedad = propertyDetails.Terreno,
                Habitaciones = propertyDetails.Habitaciones,
                Banios = propertyDetails.Banios,
                MejoraIds = mejoras
                    .Where(m => propertyDetails.Mejoras.Contains(m.Nombre))
                    .Select(m => m.Id)
                    .ToList(),
                ImagenesExistentes = propertyDetails.Imagenes
            };

            return View(editViewModel);
        }







        [HttpPost]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> EditProperty(EditPropertyViewModel model)
        {
            if (model.MejoraIds == null)
            {
                model.MejoraIds = new List<int>();
            }

            if (!ModelState.IsValid)
            {
               
                ViewBag.TiposPropiedades = (await _propertyService.GetTiposPropiedadAsync())
                    .Select(tp => new SelectListItem
                    {
                        Value = tp.Id.ToString(),
                        Text = tp.Nombre,
                        Selected = (tp.Id == model.TipoPropiedadId)
                    }).ToList();

                
                ViewBag.TiposVentas = (await _propertyService.GetTiposVentasAsync())
                    .Select(tv => new SelectListItem
                    {
                        Value = tv.Id.ToString(),
                        Text = tv.Nombre,
                        Selected = (tv.Id == model.TipoVentaId)
                    }).ToList();

                
                ViewBag.Mejoras = (await _propertyService.GetAllMejorasAsync())
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nombre,
                        Selected = model.MejoraIds != null && model.MejoraIds.Contains(m.Id)
                    }).ToList();

                TempData["ErrorMessage"] = "Por favor, corrija los errores del formulario.";
                return View(model);
            }

            var userId = User.FindFirst("UserId")?.Value;
            var agent = await _agentService.GetAgenteByUserIdAsync(userId);

            if (agent == null)
                return Unauthorized();

            await _propertyService.UpdatePropertyAsync(model);

            return RedirectToAction(nameof(AgentProperties));
        }
        [HttpGet]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var propiedad = await _propertyService.GetPropertyDetailsByIdAsync(id);

            if (propiedad == null)
            {
                TempData["ErrorMessage"] = "La propiedad no existe.";
                return RedirectToAction(nameof(AgentProperties));
            }

            return View(propiedad);
        }

        [HttpPost]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> ConfirmDeleteProperty(int id)
        {
            try
            {
                await _propertyService.DeletePropertyAsync(id);
                TempData["SuccessMessage"] = "La propiedad fue eliminada exitosamente.";
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(AgentProperties));
        }









    }
}
