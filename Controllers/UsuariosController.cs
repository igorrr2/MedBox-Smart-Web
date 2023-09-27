using CaixaINteligenteWeb.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;

namespace CaixaINteligenteWeb.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly FirebaseClient _firebaseClient;
        public UsuariosController()
        {
            _firebaseClient = new FirebaseClient("https://caixainteligentedemedicamentos-default-rtdb.firebaseio.com/");
        }
        public async Task<IActionResult> IndexAsync()
        {
            //UsuarioModel user = _userManager.GetLoggedInUser();
            var userId = HttpContext.Session.GetString("UserId");
            var isAdminString = HttpContext.Session.GetString("IsAdmin");
            var nomeUsuario = HttpContext.Session.GetString("NomeUsuario");
            bool.TryParse(isAdminString, out bool isAdmin);
            if (userId != null && isAdmin)
            {
                ViewData["NomeUsuario"] = nomeUsuario;

                var usuarios = (await _firebaseClient.Child("Users").OnceAsync<UsuarioModel>()).ToList();
                List<UsuarioModel> usuariosView = new List<UsuarioModel>();
                foreach (var usuario in usuarios)
                {
                    usuariosView.Add(usuario.Object);
                }
                return View(usuariosView);
            }
            else if (userId != null && isAdmin)
            {
                return RedirectToAction("Index", "Remedios");
            }
            else
            {
                // Trate o caso em que o usuário não está logado
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<IActionResult> EditarAsync(string id)
        {
            var usuarios = (await _firebaseClient.Child("Users").OnceAsync<UsuarioModel>())
                .Where(u => u.Object.Id == id)
                .FirstOrDefault();
            UsuarioModel usuariosView = usuarios.Object;

            var nomeUsuario = HttpContext.Session.GetString("NomeUsuario");
            ViewData["NomeUsuario"] = nomeUsuario;
            ViewData["NomeUsuarioEdicao"] = usuariosView.NomeCompleto;
            ViewData["UsuarioAdministradorEdicao"] = usuariosView.IsAdministrador.ToString();
            ViewData["ChaveEsp32Edicao"] = usuariosView.ChaveEsp32;
            HttpContext.Session.SetString("IdUsuarioEdicao", usuariosView.Id.ToString());
            HttpContext.Session.SetString("UsuarioAdministradorEdicao", usuariosView.IsAdministrador.ToString());
            HttpContext.Session.SetString("ChaveEsp32Edicao", usuariosView.ChaveEsp32.ToString());

            return View(usuariosView);
        }

        public async Task<IActionResult> SalvarAsync(UsuarioModel model)
        {
            string usuarioAdministrador = Request.Form["usuarioAdministrador"];
            string chaveEsp32 = Request.Form["chaveesp32"];

            string id = HttpContext.Session.GetString("IdUsuarioEdicao");
            var usuario = (await _firebaseClient.Child("Users").OnceAsync<UsuarioModel>())
                .Where(u => u.Object.Id == id)
                .FirstOrDefault();
            model.Email = usuario.Object.Email;
            model.Id = usuario.Object.Id;
            model.DataHoraCadastro = usuario.Object.DataHoraCadastro;
            model.NomeCompleto = usuario.Object.NomeCompleto;
            model.Username = usuario.Object.Username;
            model.Password = usuario.Object.Password;
            if(usuarioAdministrador == "on")
            model.IsAdministrador = true;
            else model.IsAdministrador = false;
            model.ChaveEsp32 = chaveEsp32;

            await _firebaseClient.Child("Users").Child(usuario.Key).PutAsync(model);

            HttpContext.Session.Remove("IdUsuarioEdicao");
            HttpContext.Session.Remove("UsuarioAdministradorEdicao");
            HttpContext.Session.Remove("ChaveEsp32Edicaos");

            return RedirectToAction("Index", "Usuarios");
        }
    }

}
