using Microsoft.JSInterop;

namespace clientApp.Services
{
    public class AddressService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string ActiveAddressKey = "activeAddressId";

        public AddressService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetActiveAddressId(int addressId)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", ActiveAddressKey, addressId.ToString());
        }

        public async Task<int?> GetActiveAddressId()
        {
            var activeAddressId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", ActiveAddressKey);
            if (int.TryParse(activeAddressId, out int id))
            {
                return id;
            }
            return null;
        }

        public async Task ClearActiveAddress()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", ActiveAddressKey);
        }
    }
} 