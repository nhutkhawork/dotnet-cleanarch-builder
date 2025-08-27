namespace Temp.Domain.Common;

public static class EnvironmentHelpers
{
    public static string GetJwtKey()
    {
        return Environment.GetEnvironmentVariable("JWT_KEY") ?? "h2vmn7C0fmVJP425";
    }

    public static string GetSalt()
    {
        return Environment.GetEnvironmentVariable("SALT") ?? "IvCpw9TkBJFYDqyM";
    }
}
