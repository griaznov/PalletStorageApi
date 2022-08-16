using AutoMapper;
using PalletStorage.BusinessModels;
using PalletStorage.WebApi.Models.Pallet;

namespace PalletStorage.WebApi.Automapper;

public class PalletApiMappingProfile : Profile
{
    public PalletApiMappingProfile()
    {
        CreateMap<PalletModel, PalletResponse>()
            .ForMember(p => p.Boxes, options => options.Condition(p => p.Boxes.Count > 0));
        CreateMap<PalletResponse, PalletModel>();

        CreateMap<PalletModel, PalletCreateRequest>();
        CreateMap<PalletCreateRequest, PalletModel>()
            .ForMember(p => p.Boxes, opt => opt.Ignore());

        CreateMap<PalletModel, PalletUpdateRequest>();
        CreateMap<PalletUpdateRequest, PalletModel>()
            .ForMember(p => p.Boxes, opt => opt.Ignore());
        ;
    }
}
