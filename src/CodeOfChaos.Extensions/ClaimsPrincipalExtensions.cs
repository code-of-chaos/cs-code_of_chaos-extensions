// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ClaimsPrincipalExtensions {
    public static IEnumerable<string> FindAllValues(this ClaimsPrincipal principal, string type) {
        IEnumerable<Claim> claims = principal.FindAll(type);
        return claims.Select(c => c.Value);
    }
}
