using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Sales;
using RealStateApp.Core.Application.ViewModels.Upgrades;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Services
{
    public class MejoraService : GenericService<UpgradeHandlerViewModel, UpgradeViewModel, Mejora>, IMejoraService
    {
        private readonly IMejoraRepository _repository;
        private readonly IMapper _mapper;

        public MejoraService(IMejoraRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
