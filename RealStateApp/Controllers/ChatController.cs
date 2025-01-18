using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Chat;
using System.Threading.Tasks;

namespace RealStateApp.WebApp.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    if (messageViewModel == null || string.IsNullOrEmpty(messageViewModel.UserId))
                    {
                        TempData["Error"] = "Los datos enviados no son válidos.";
                        return RedirectToAction("Details", "Property", new { id = messageViewModel?.ChatId ?? 0 });
                    }

                    await _chatService.AddMessageToChatAsync(messageViewModel);

                    return RedirectToAction("Details", "Property", new { id = messageViewModel.ChatId });
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Hubo un problema al enviar el mensaje: " + ex.Message;
                }
            }

            TempData["Error"] = "No se pudo enviar el mensaje. Por favor, inténtalo de nuevo.";
            return RedirectToAction("Details", "Property", new { id = messageViewModel.ChatId });
        }

    }
}
