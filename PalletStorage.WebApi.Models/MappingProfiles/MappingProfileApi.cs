using AutoMapper;
using PalletStorage.BusinessModels;
using PalletStorage.WebApi.Models.Models.Box;
using PalletStorage.WebApi.Models.Models.Pallet;

namespace PalletStorage.WebApi.Models.MappingProfiles;

public class MappingProfileApi : Profile
{
    public MappingProfileApi()
    {
        // Boxes
        CreateMap<BoxModel, BoxResponse>();
        CreateMap<BoxResponse, BoxModel>();

        CreateMap<BoxModel, BoxCreateRequest>();
        CreateMap<BoxCreateRequest, BoxModel>();

        CreateMap<BoxModel, BoxUpdateRequest>();
        CreateMap<BoxUpdateRequest, BoxModel>();

        // Pallets
        CreateMap<PalletModel, PalletResponse>()
            .ForMember(p => p.Boxes, options => options.Condition(p => p.Boxes.Count > 0));
        CreateMap<PalletResponse, PalletModel>();

        CreateMap<PalletModel, PalletCreateRequest>();
        CreateMap<PalletCreateRequest, PalletModel>();

        CreateMap<PalletModel, PalletUpdateRequest>();
        CreateMap<PalletUpdateRequest, PalletModel>();
    }
}
