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
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server;

namespace MITSWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        [HttpPost("~/connect/token"), Produces("application/json")]
        public IActionResult Exchange(OpenIdConnectRequest request)
        {
            if (!request.IsPasswordGrantType())
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    ErrorDescription = "The specified grant type is not supported."
                });
            }

            if (request.Username != "enderjs@gmail.com" ||
                request.Password != "password")
            {
                return Forbid(OpenIddictServerDefaults.AuthenticationScheme);
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
            identity.AddClaim(OpenIdConnectConstants.Claims.Role, "Admin", OpenIdConnectConstants.Destinations.AccessToken);

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(
                 principal,
                 new AuthenticationProperties(),
                 OpenIdConnectServerDefaults.AuthenticationScheme);

            // ... add other claims, if necessary.
            
            // Ask OpenIddict to generate a new token and return an OAuth2 token response.
            return SignIn(principal, OpenIddictServerDefaults.AuthenticationScheme);
        }
    }
}