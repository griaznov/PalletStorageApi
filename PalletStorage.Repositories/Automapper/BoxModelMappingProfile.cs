using AutoMapper;
using DataContext.Entities;
using PalletStorage.BusinessModels;

namespace PalletStorage.Repositories.Automapper;

public class BoxModelMappingProfile : Profile
{
    public BoxModelMappingProfile()
    {
        CreateMap<BoxModel, Box>()
            .ForMember(b => b.PalletId, opt => opt.Ignore())
            .ForMember(b => b.Pallet, opt => opt.Ignore());
        CreateMap<Box, BoxModel>();
    }
}
