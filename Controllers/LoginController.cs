using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TesteMazzaFC.WebApi.Models;

namespace TesteMazzaFC.WebApi.Controllers
{
    public class LoginController : Controller
    {
        const string URL_BASE = "https://localhost:44306/api/";

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(UserViewModel login)
        {
            IEnumerable<UserViewModel> usuario = null;

            using (var user = new HttpClient())
            {
                user.BaseAddress = new Uri(URL_BASE);

                var result = await user.PostAsJsonAsync("entrar", login);

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Produto/Index");
                }
                else
                {
                    var error = await result.Content.ReadAsAsync<ErroViewModel>();
                    ModelState.AddModelError(string.Empty, error.Errors.FirstOrDefault());
                }
            }

            return View(usuario);
        }
    }
}