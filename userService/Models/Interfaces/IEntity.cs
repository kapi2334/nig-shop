using UserService.Models;
using UserService.Models.Abstract;

namespace UserService.Models.Interfaces
{
    internal interface IEntity
    {
        public int id { get; set; }

        public UserType user {  get; set; }

        public int addressId { get; set; }

        public Address address { get; set; }
    }
}
