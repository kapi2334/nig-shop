namespace UserService.Models.Builders{
    internal class UserBuilder{

        private readonly User _user;

        public UserBuilder(){

            _user = new User{

                Addresses = new List<Address>()
            };
        }

        public UserBuilder WithLogin(string login){

            _user.login = login;
            return this;
        }

        public UserBuilder WithPasswordHash(string passwordHash){

            _user.passwordHash = passwordHash;
            return this;
        }

        public UserBuilder WithAddressID(int addressID){

            _user.addressID = addressID;
            return this;
        }

        public UserBuilder WithAddresses(ICollection<Address> addresses){

            _user.Addresses = addresses;
            return this;
        }

        public UserBuilder AddAddress(Address address){

            _user.Addresses.Add(address);
            return this;
        }

        public UserBuilder WithNip(long? nip){

            _user.nip = nip;
            return this;
        }

        public UserBuilder WithType(char type){

            _user.type = type; // setter wykona walidację
            return this;
        }

        public UserBuilder WithName(string name){

            _user.name = name;
            return this;
        }

        public UserBuilder WithSurname(string? surname){

            _user.surname = surname;
            return this;
        }

        public User Build(){

            return _user;
        }
    }
}
