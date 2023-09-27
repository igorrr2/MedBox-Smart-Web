using CaixaINteligenteWeb.Models;
using Firebase.Database;
using Microsoft.AspNetCore.Mvc;

namespace CaixaINteligenteWeb.Controllers
{
    public class AlarmesController : Controller
    {
        private readonly FirebaseClient _firebaseClient;
        public AlarmesController()
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
                //bool isAdmin = user.IsAdministrador;
                bool.TryParse(isAdminString, out bool isAdmin);
                // Você pode armazenar essa variável no ViewData para acessá-la na visualização
                ViewData["IsAdmin"] = isAdmin;
                ViewData["NomeUsuario"] = nomeUsuario;

                var alarmes = (await _firebaseClient.Child("Alarmes").OnceAsync<AlarmeModel>())
               .Where(u => u.Object.IdUsuario == userId).ToList();
                List<AlarmeModel> alarmesView = new List<AlarmeModel>();
                foreach (var alarme in alarmes)
                {
                    var remedio = (await _firebaseClient.Child("Remedios").OnceAsync<RemedioModel>())
                    .Where(u => u.Object.Id == alarme.Object.IdRemedio).FirstOrDefault();
                    alarme.Object.nomeRemedio = remedio.Object.NomeRemedio;
                    alarmesView.Add(alarme.Object);
                }
                return View(alarmesView);
            }
            else
            {
                // Trate o caso em que o usuário não está logado
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}
