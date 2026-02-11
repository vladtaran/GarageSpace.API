using GarageSpaceAPI.Contracts.Dto;
using GarageSpaceAPI.Contracts.Request;

namespace GarageSpace.API.Mapping
{
    public interface IUserObjectsMapper
    {
        public UserDto MapCreateUserRequestToUser(UserCreateRequest request);
    }
}