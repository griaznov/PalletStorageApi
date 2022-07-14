using AutoMapper;
using PalletStorage.Common.Models;
using PalletStorage.WebApi.Models.Models;

namespace PalletStorage.WebApi.Models.Converters
{
    public class MappingProfileApi : Profile
    {
        public MappingProfileApi()
        {
            CreateMap<Box, BoxApiModel>();
            CreateMap<BoxApiModel, Box>()
                .ForMember(b => b.Id, opt => opt.Condition(b => b.Id is not null));

            CreateMap<Pallet, PalletApiModel>()
                .ForMember(p => p.Boxes, options => options.Condition(p => p.Boxes.Count > 0));

            CreateMap<PalletApiModel, Pallet>()
                .ForMember(p => p.Id, options => options.Condition(p => p.Id is not null));
        }
    }
}
