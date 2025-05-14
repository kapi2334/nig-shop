using UserService.Models;

namespace userService.Models.Interfaces
{
    internal interface IUserType
    {
        public int id { get; set; }
        public string login { get; set; }
        public string passwordHash { get; set; }
        public IEnumerable<IEntity> address { get; set; }
        public long? nip { get; set; }


    }
}
