using CaixaINteligenteWeb.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class UserManager
{
    private const string SessionKey = "UserSession";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SaveUser(UsuarioModel user)
    {
        var serializedUser = JsonConvert.SerializeObject(user);
        _httpContextAccessor.HttpContext.Session.SetString(SessionKey, serializedUser);
    }

    public UsuarioModel GetLoggedInUser()
    {
        var serializedUser = _httpContextAccessor.HttpContext.Session.GetString(SessionKey);
        if (serializedUser != null)
        {
            return JsonConvert.DeserializeObject<UsuarioModel>(serializedUser);
        }
        return null;
    }

    public void Logout()
    {
        _httpContextAccessor.HttpContext.Session.Remove(SessionKey);
    }
}