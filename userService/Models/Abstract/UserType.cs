using UserService.Models;
using UserService.Models.Interfaces;

namespace UserService.Models.Abstract
{
    internal abstract class UserType 
    {
        public abstract int id { get; set; }
        public abstract string login { get; set; }
        public abstract string passwordHash { get; set; }
        public abstract IEnumerable<IEntity> address { get; set; }
        public abstract long? nip { get; set; }


    }
}
