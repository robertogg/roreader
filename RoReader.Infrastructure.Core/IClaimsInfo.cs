using System.Collections.Generic;
using System.Security.Claims;

namespace RoReader.Infrastructure.Core
{
    public interface IClaimsInfo
    {
        string GetCurrentUser();
        string GetCurrentUserId();
        IEnumerable<Claim> GetClaimsCurrentUser();
    }
}