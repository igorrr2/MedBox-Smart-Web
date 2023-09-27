using CaixaINteligenteWeb.Models;
using Firebase.Database;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml.Linq;


namespace CaixaINteligenteWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FirebaseClient _firebaseClient;
        [BindProperty]
        public UsuarioModel User { get; set; }
        public bool loginIncorreto = false;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _firebaseClient = new FirebaseClient("https://caixainteligentedemedicamentos-default-rtdb.firebaseio.com/");
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var isAdminString = HttpContext.Session.GetString("IsAdmin");
            var nomeUsuario = HttpContext.Session.GetString("NomeUsuario");
            if (userId != null)
            {
                bool.TryParse(isAdminString, out bool isAdmin);
                if (isAdmin)
                {
                    // Se o usuário é um administrador, redirecione para as 3 telas
                    return RedirectToAction("Index", "Usuarios");
                }
                else
                {
                    // Se o usuário não é um administrador, redirecione para as 2 telas
                    return RedirectToAction("Index", "Remedios");
                }
            }
            return View();
        }
        public IActionResult ErroLogin()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("IsAdmin");
            HttpContext.Session.Remove("NomeUsuario");
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LoginAsync()
        {
            var user = (await _firebaseClient.Child("Users").OnceAsync<UsuarioModel>())
                .Where(u => u.Object.Username == User.Username)
                .Where(u => u.Object.Password == User.Password)
                .FirstOrDefault();

            if (user != null)
            {
                //_userManager.SaveUser(user.Object);
                loginIncorreto = false;
                HttpContext.Session.SetString("UserId", user.Object.Id.ToString());
                HttpContext.Session.SetString("IsAdmin", user.Object.IsAdministrador.ToString());
                HttpContext.Session.SetString("NomeUsuario", user.Object.NomeCompleto.ToString());
                // O usuário foi autenticado com sucesso
                if (user.Object.IsAdministrador)
                {
                    // Se o usuário é um administrador, redirecione para as 3 telas
                    return RedirectToAction("Index", "Usuarios");
                }
                else
                {
                    // Se o usuário não é um administrador, redirecione para as 2 telas
                    return RedirectToAction("Index", "Remedios");
                }
            }
            else
            {
                return RedirectToAction("ErroLogin", "Home");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}