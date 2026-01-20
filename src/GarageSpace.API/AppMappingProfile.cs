using AutoMapper;
using GarageSpaceAPI.Contracts.Dto;
using GarageSpaceAPI.Contracts.Dto.Vehicle;
using GarageSpace.Controllers.Request;
using GarageSpace.Data.Models.EF.CarJournal;
using GarageSpace.Data.Models.EF.Vehicles;
using GarageSpaceAPI.Contracts.Dto;
using GarageSpaceAPI.Contracts.Dto.Vehicle;

namespace GarageSpace;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<UpdateUserRequest, UserDto>();
        CreateMap<GarageSpace.Data.Models.EF.User, UserDto>().ReverseMap();

        
        // Vehicle mappings
        CreateMap<Car, CarDto>().ReverseMap();
        CreateMap<Motorcycle, MotocycleDto>().ReverseMap();
        CreateMap<Trailer, TrailerDto>().ReverseMap();

        CreateMap<Journal, JournalDto>().ReverseMap();
        CreateMap<JournalRecord, JournalRecordDto>().ReverseMap();

        CreateMap<GarageSpace.Data.Models.EF.UserGarage, GarageDto>().ReverseMap();
        CreateMap<GarageSpace.Data.Models.EF.Manufacturer, ManufacturerDto>().ReverseMap();
        CreateMap<GarageSpace.Data.Models.EF.Country, CountryDto>().ReverseMap();
    }
}