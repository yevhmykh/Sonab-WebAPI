namespace Sonab.Auth0Client;

public sealed record Auth0Options(
    string Audience,
    string Authority,
    string Domain,
    string ClientId,
    string ClientSecret
);
