using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using RoReader.Infrastructure.Core;

namespace RoReader.Infrastructure.Claims
{
    public class ClaimsInfo : IClaimsInfo
    {
        public string GetCurrentUser()
        {
            return ClaimsPrincipal.Current.Identity.Name;
        }

        public string GetCurrentUserId()
        {
            return ClaimsPrincipal.Current.Claims.FirstOrDefault(q => q.Type == ClaimTypes.NameIdentifier).Value;
        }

        public IEnumerable<Claim> GetClaimsCurrentUser()
        {
            return ClaimsPrincipal.Current.Claims;
        }
    }
}
