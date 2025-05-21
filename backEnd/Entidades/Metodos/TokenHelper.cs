using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace backEnd.Entidades
{
    public static class TokenHelper
    {
        public static int? ExtraerUsuarioId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null) return null;

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Usuario_ID")?.Value;

            return int.TryParse(userIdClaim, out var userId) ? userId : (int?)null;
        }

    }
}
