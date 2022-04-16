namespace locationRecordeapi.TokenAuthentication
{
    public interface ITokenManager
    {
        bool Authenticate(string name, string password);
        bool checkPermission(string[] permissions, int? role);
        Token NewToken();
        bool VerifyToken(string token);

    }
}