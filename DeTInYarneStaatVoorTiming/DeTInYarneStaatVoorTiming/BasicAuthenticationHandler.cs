using DeTInYarneStaatVoorTiming.Data;
using DeTInYarneStaatVoorTiming.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace DeTInYarneStaatVoorTiming
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private IDataContext _data;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IDataContext data
            ) : base(options, logger, encoder, clock)
            
        {
            _data = data;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
            }

            // Get authorization key
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var authHeaderRegex = new Regex(@"Basic (.*)");

            if (!authHeaderRegex.IsMatch(authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted properly."));
            }

            
            var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
            var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
            var authUsername = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

            //-----------
            Account acc = _data.GetAccountByRole(authUsername);
            if (acc == null) return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));



            if (authPassword == acc.pass)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, authUsername));

                if (authUsername == "ThiboVanderkam") claims.Add(new Claim(ClaimTypes.Role, "admin"));
                if (authUsername == "YarneTimingAssistent") claims.Add(new Claim(ClaimTypes.Role, "user"));

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom"));

                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
            }
            return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));
        }
    }
}