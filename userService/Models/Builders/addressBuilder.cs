namespace UserService.Models.Builders{

internal class AddressBuilder{

    private readonly Address _address;

    public AddressBuilder(){
        _address = new Address();
    }

    public AddressBuilder WithId(int id){
        _address.id = id;
        return this;
    }

    public AddressBuilder WithStreet(string street){
        _address.street = street;
        return this;
    }

    public AddressBuilder WithBuildingNo(int buildingNo){
        _address.buildingNo = buildingNo;
        return this;
    }

    public AddressBuilder WithLocaleNo(int? localeNo){
        _address.localeNo = localeNo;
        return this;
    }

    public AddressBuilder WithPostCode(string postCode){
        _address.postCode = postCode;
        return this;
    }

    public AddressBuilder WithCity(string city){
        _address.city = city;
        return this;
    }

    public AddressBuilder WithCountry(string country){
         _address.country = country;
        return this;
    }

    public AddressBuilder WithUserId(int? userId){
        _address.UserId = userId;
        return this;
    }

    public AddressBuilder WithUser(User user){
        _address.User = user;
        _address.UserId = user.id;
        return this;
    }

    public Address Build(){
        return _address;
    }
}
}
