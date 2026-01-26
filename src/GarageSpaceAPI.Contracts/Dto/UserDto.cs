namespace GarageSpaceAPI.Contracts.Dto
{
    public class UserDto : BaseAuditableEntityDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
    }
}
