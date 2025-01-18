using System;

namespace RealStateApp.Core.Application.ViewModels.Agents
{
    public class AgentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string UserId { get; set; }
    }
}
