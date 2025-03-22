// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace CodeOfChaos.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ClaimsIdentityExtensions {

    public static ClaimsIdentity AddClaim(this ClaimsIdentity identity, Claim claim) {
        identity.AddClaim(claim);
        return identity;
    }

    public static ClaimsIdentity AddClaims(this ClaimsIdentity identity, IEnumerable<Claim> claims) {
        identity.AddClaims(claims);
        return identity;
    }

    public static ClaimsIdentity RemoveClaim(this ClaimsIdentity identity, Claim? claim) {
        identity.RemoveClaim(claim);
        return identity;
    }

    public static IEnumerable<string> FindAllValues(this ClaimsIdentity identity, string type) {
        IEnumerable<Claim> claims = identity.FindAll(type);
        return claims.Select(c => c.Value);
    }

    public static ClaimsIdentity AddClaim(this ClaimsIdentity identity, string type, string value) {
        identity.AddClaim(new Claim(type, value));
        return identity;
    }

    public static ClaimsIdentity AddClaim(this ClaimsIdentity identity, string type, string value, string? valueType) {
        identity.AddClaim(new Claim(type, value, valueType));
        return identity;
    }

    public static ClaimsIdentity AddClaim(this ClaimsIdentity identity, string type, string value, string? valueType, string? issuer) {
        identity.AddClaim(new Claim(type, value, valueType, issuer));
        return identity;
    }

    public static ClaimsIdentity AddClaim(this ClaimsIdentity identity, string type, string value, string? valueType, string? issuer, string? originalIssuer) {
        identity.AddClaim(new Claim(type, value, valueType, issuer, originalIssuer));
        return identity;
    }

    public static ClaimsIdentity AddClaim(this ClaimsIdentity identity, string type, string value, string? valueType, string? issuer, string? originalIssuer, ClaimsIdentity? subject) {
        identity.AddClaim(new Claim(type, value, valueType, issuer, originalIssuer, subject));
        return identity;
    }
}
