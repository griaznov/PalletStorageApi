using AutoMapper;
using DataContext.Entities;
using PalletStorage.BusinessModels;

namespace PalletStorage.Repositories.Automapper;

public class PalletModelMappingProfile : Profile
{
    public PalletModelMappingProfile()
    {
        CreateMap<PalletModel, Pallet>();
        CreateMap<Pallet, PalletModel>();
    }
}
