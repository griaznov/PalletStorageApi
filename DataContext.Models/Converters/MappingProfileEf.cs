using AutoMapper;
using DataContext.Models.Models;
using PalletStorage.Common.CommonClasses;

namespace DataContext.Models.Converters
{
    public class MappingProfileEf : Profile
    {
        public MappingProfileEf()
        {
            CreateMap<Box, BoxEfModel>()
                .ForMember(b => b.PalletId, opt => opt.Ignore())
                .ForMember(b => b.Pallet, opt => opt.Ignore());
            CreateMap<BoxEfModel, Box>();

            CreateMap<Pallet, PalletEfModel>();
            CreateMap<PalletEfModel, Pallet>();
        }
    }
}
