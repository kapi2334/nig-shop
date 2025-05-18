using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace clientApp.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly UserService _userService;

    public CustomAuthStateProvider(UserService userService)
    {
        _userService = userService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await _userService.GetCurrentUserAsync();
        
        if (user != null)
        {
            // Create claims for the authenticated user
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.name),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim("login", user.login)
            };
            var identity = new ClaimsIdentity(claims, "custom");
            var principal = new ClaimsPrincipal(identity);
            
            return new AuthenticationState(principal);
        }

        // Return unauthenticated state
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
} 