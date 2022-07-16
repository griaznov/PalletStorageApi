using AutoMapper;
using PalletStorage.Business.Models;
using PalletStorage.WebApi.Models.Models;

namespace PalletStorage.WebApi.Models.MappingProfiles
{
    public class MappingProfileApi : Profile
    {
        public MappingProfileApi()
        {
            CreateMap<BoxModel, BoxApiModel>();
            CreateMap<BoxApiModel, BoxModel>()
                .ForMember(b => b.Id, opt => opt.Condition(b => b.Id is not null));

            CreateMap<PalletModel, PalletApiModel>()
                .ForMember(p => p.Boxes, options => options.Condition(p => p.Boxes.Count > 0));

            CreateMap<PalletApiModel, PalletModel>()
                .ForMember(p => p.Id, options => options.Condition(p => p.Id is not null));
        }
    }
}
