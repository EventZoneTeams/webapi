using Microsoft.AspNetCore.Http;
using Repositories.Interfaces;
using Repositories.Utils;
using System.Security.Claims;

namespace Repositories.Commons
{
    public class ClaimsService : IClaimsService
    {

        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            // todo implementation to get the current userId
            var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var extractedId = AuthenTools.GetCurrentUserId(identity);
            GetCurrentUserId = string.IsNullOrEmpty(extractedId) ? -1 : int.Parse(extractedId);
            IpAddress = httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();
        }

        public int GetCurrentUserId { get; }

        public string? IpAddress { get; }
    }
}
