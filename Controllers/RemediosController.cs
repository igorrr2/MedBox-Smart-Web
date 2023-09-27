using CaixaINteligenteWeb.Models;
using Firebase.Database;
using Microsoft.AspNetCore.Mvc;

namespace CaixaINteligenteWeb.Controllers
{
    public class RemediosController : Controller
    {
        private readonly FirebaseClient _firebaseClient;
        public RemediosController()
        {
            _firebaseClient = new FirebaseClient("https://caixainteligentedemedicamentos-default-rtdb.firebaseio.com/");
        }
        public async Task<IActionResult> IndexAsync()
        {
            //UsuarioModel user = _userManager.GetLoggedInUser();
            var userId = HttpContext.Session.GetString("UserId");
            var isAdminString = HttpContext.Session.GetString("IsAdmin");
            var nomeUsuario = HttpContext.Session.GetString("NomeUsuario");
            if (userId != null)
            {
                bool.TryParse(isAdminString, out bool isAdmin);
                // Você pode armazenar essa variável no ViewData para acessá-la na visualização
                ViewData["IsAdmin"] = isAdmin;
                ViewData["NomeUsuario"] = nomeUsuario;
                var remedios = (await _firebaseClient.Child("Remedios").OnceAsync<RemedioModel>())
                .Where(u => u.Object.IdUsuario == userId).ToList();
                List<RemedioModel> remediosView = new List<RemedioModel>();
                foreach (var remedio in remedios)
                {
                    remediosView.Add(remedio.Object);
                }

                return View(remediosView);  
            }
            else
            {
                // Trate o caso em que o usuário não está logado
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
