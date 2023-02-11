using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Safety
{
    // Uses JwtSecurityTokenHandler, which is a built in class in .NET.
    // It is used to create and validate Json Web Tokens.
    // Read: https://learn.microsoft.com/en-us/dotnet/api/system.identitymodel.tokens.jwt.jwtsecuritytokenhandler?view=msal-web-dotnet-latest

	public class Token
	{
		// Method that generates a new token and return it as a string.
		public static string createToken(Guid userId)
		{
			var secretKey = Encoding.UTF8.GetBytes("AFR4D6Y7U8B24FB5M7HJL1L0");
			var securityKey = new SymmetricSecurityKey(secretKey);
			var algorithm = SecurityAlgorithms.HmacSha256Signature;


            var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[] 
				{
					// This can be modified for roles, for example:
					// new Claim("Role", "Admin")
					new Claim(ClaimTypes.NameIdentifier, userId.ToString())
				}),

				Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(securityKey, algorithm)
            };

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

	}

}

