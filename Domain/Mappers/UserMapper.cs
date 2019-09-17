namespace Domain.Mappers
{
    public static class UserMapper
    {
        public static DTOs.User ToDto(this Entities.User user)
        {
            return new DTOs.User
            {
                Id = user.Id,
                Name = user.Name
            };
        }
    }
}
