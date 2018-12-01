using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MITSDataLib.Models;
using OpenIddict.Server;

namespace MITSWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AuthorizationController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
        {

            

                        
            if (!request.IsPasswordGrantType())
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    ErrorDescription = "The specified grant type is not supported."
                });
            }

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The username/password couple is invalid."
                });
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The username/password couple is invalid."
                });
            }

            var identity = new ClaimsIdentity(
                OpenIddictServerDefaults.AuthenticationScheme,
                OpenIdConnectConstants.Claims.Name,
                OpenIdConnectConstants.Claims.Role);

            // Add a "sub" claim containing the user identifier, and attach
            // the "access_token" destination to allow OpenIddict to store it
            // in the access token, so it can be retrieved from your controllers.
            identity.AddClaim(OpenIdConnectConstants.Claims.Subject,
                "71346D62-9BA5-4B6D-9ECA-755574D628D8",
                OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim("Name", "Alice",
                OpenIdConnectConstants.Destinations.IdentityToken);

            string role = _userManager.GetRolesAsync(user).Result.SingleOrDefault();
            
            identity.AddClaim(OpenIdConnectConstants.Claims.Role, role, OpenIdConnectConstants.Destinations.AccessToken);

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(
                 principal,
                 new AuthenticationProperties(),
                 OpenIdConnectServerDefaults.AuthenticationScheme);

            ticket.SetResources("mits_server");

            // Ask OpenIddict to generate a new token and return an OAuth2 token response.
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }
    }
}