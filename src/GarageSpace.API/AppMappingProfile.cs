using AutoMapper;
using GarageSpace.API.Contracts.Dto;
using GarageSpace.API.Contracts.Dto.Vehicle;
using GarageSpace.Models.Repository.EF;
using GarageSpace.Models.Repository.EF.CarJournal;
using GarageSpace.Models.Repository.EF.Vehicles;
using GarageSpace.API.Contracts.Request;

namespace GarageSpace;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<UpdateUserRequest, UserDto>();
        CreateMap<User, UserDto>().ReverseMap();

        
        // Vehicle mappings
        CreateMap<Car, CarDto>().ReverseMap();
        CreateMap<Motorcycle, MotocycleDto>().ReverseMap();
        CreateMap<Trailer, TrailerDto>().ReverseMap();

        CreateMap<Journal, JournalDto>().ReverseMap();
        CreateMap<JournalRecord, JournalRecordDto>().ReverseMap();

        CreateMap<UserGarage, GarageDto>().ReverseMap();
        CreateMap<Manufacturer, ManufacturerDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();
    }
}