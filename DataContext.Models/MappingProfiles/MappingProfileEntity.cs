using AutoMapper;
using DataContext.Models.Entities;
using PalletStorage.Business.Models;

namespace DataContext.Models.MappingProfiles;

public class MappingProfileEntity : Profile
{
    public MappingProfileEntity()
    {
        // Box
        CreateMap<BoxModel, Box>()
            .ForMember(b => b.PalletId, opt => opt.Ignore())
            .ForMember(b => b.Pallet, opt => opt.Ignore());
        CreateMap<Box, BoxModel>();

        // Pallet
        CreateMap<PalletModel, Pallet>();
        CreateMap<Pallet, PalletModel>();
    }
}
