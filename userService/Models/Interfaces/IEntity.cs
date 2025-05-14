using UserService.Models;

namespace userService.Models.Interfaces
{
    internal interface IEntity
    {
        public int id { get; set; }

        public IUserType user {  get; set; }

        public int addressId { get; set; }

        public Address address { get; set; }
    }
}
