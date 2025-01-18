using RealStateApp.Core.Application.ViewModels.Upgrades;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IMejoraService : IGenericService<UpgradeHandlerViewModel, UpgradeViewModel, Mejora>
    {
    }
}
