using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Safety
{
	public class Token
	{

		public static string createToken(int userId)
		{
			var secretKey = Encoding.ASCII.GetBytes("rumpelstiltskin");
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

