using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Security
{
    // Uses JwtSecurityTokenHandler, which is a built in class in .NET.
    // It is used to create and validate Json Web Tokens.
    // Read: https://learn.microsoft.com/en-us/dotnet/api/system.identitymodel.tokens.jwt.jwtsecuritytokenhandler?view=msal-web-dotnet-latest

	public class Token
	{
		// Method that generates a new token and return it as a string.
		public static string CreateToken(int userId)
		{
			var secretKey = Encoding.UTF8.GetBytes(GenerateSecretKey(20));
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

		// Method used to generate a random string containing only
		// alphanumeric values. Can specify length of the string with
		// the parameter size.
		private static string GenerateSecretKey(int size)
		{
			Random generator = new Random();
            StringBuilder key = new StringBuilder();

            string valid = "abcdefghijklmnopqrstuvwxyz0123456789";

			for (int i = 0; i < size; i++)
			{
				int randomIndex = generator.Next(valid.Length);

				key.Append(valid[randomIndex]);
			}
			return key.ToString().ToUpper();
		}

	}

}

