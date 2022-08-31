using AutoMapper;
using PalletStorage.BusinessModels;
using PalletStorage.WebApi.Models.Box;

namespace PalletStorage.WebApi.Infrastructure.Automapper;

public class BoxApiMappingProfile : Profile
{
    public BoxApiMappingProfile()
    {
        CreateMap<BoxModel, BoxResponse>();
        CreateMap<BoxResponse, BoxModel>();

        CreateMap<BoxModel, BoxCreateRequest>();
        CreateMap<BoxCreateRequest, BoxModel>();

        CreateMap<BoxModel, BoxUpdateRequest>();
        CreateMap<BoxUpdateRequest, BoxModel>();
    }
}
