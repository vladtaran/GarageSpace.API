using GarageSpace.API.Contracts.Dto;
using GarageSpace.API.Contracts.Request;

namespace GarageSpace.API.Mapping
{
    public interface IUserObjectsMapper
    {
        public UserDto MapCreateUserRequestToUser(UserCreateRequest request);
    }
}