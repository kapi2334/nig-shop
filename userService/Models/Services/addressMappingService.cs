using UserService.Models;
using UserService.Data;
using Microsoft.EntityFrameworkCore;
namespace UserService.Models.Services{
	internal class AddressMapperService{
		
		public async Task<List<Address>> MapoutAddressAsync(List<AddressDto> addressDtos, AppDbContext db){
			var outAddress = new List<Address>();

			foreach (var dto in addressDtos){
				// Check if address already is in database
				var existingAddress = await db.Address.FirstOrDefaultAsync(a =>
					a.street == dto.street &&
					a.buildingNo == dto.buildingNo &&
					a.localeNo == dto.localeNo &&
					a.postCode == dto.postCode &&
					a.city == dto.city &&
					a.country == dto.country); 
				
				if (existingAddress != null){
					outAddress.Add(existingAddress);
				}
				//If not- add it 
				else{
					var newAddress = new Address{
						street = dto.street,
						buildingNo = dto.buildingNo,
						localeNo = dto.localeNo,
						postCode = dto.postCode,
						city = dto.city,
						country = dto.country
					};

					db.Address.Add(newAddress);
					outAddress.Add(newAddress); // dodaj do listy, nawet przed SaveChanges
				}
			}

			return outAddress;
		}
		
		
	}
	
}	
