using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using AccessControl.Models;

namespace AccessControl.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            //Recebe a url que o usuário tentou acessar
            ViewBag.ReturnUrl = returnUrl;
            return View(new Access());
        }


        /// <param name="login"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccessControl.Models.Access login, string returnUrl)
        {
            if (!login.Email.IsEmpty() && !login.Password.IsEmpty())
            {
                using (AccessControl.Models.DbZeusEntities db = new AccessControl.Models.DbZeusEntities())
                {
                    var getLogin = db.Access.FirstOrDefault(p => p.Email.Equals(login.Email));
                    // Verificar se a variavel getLogin está vazia. Isso pode ocorrer caso o usuário não existe. 
                    if (getLogin != null)
                    {
                        // Verifica se o usuário que retornou na variavel tem está ativo
                        if (getLogin.Active)
                        {
                            // Valida se a senha digitada est[a correta
                            if (getLogin.ValidatePassword(login.Password))
                            {
                                FormsAuthentication.SetAuthCookie(getLogin.Email, false);
                                if (Url.IsLocalUrl(returnUrl)
                                && returnUrl.Length > 1
                                && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//")
                                && returnUrl.StartsWith("/\\"))
                                {
                                    return Redirect(returnUrl);
                                }
                                //cria uma session para armazenar o nome do usuário
                                Session["Name"] = getLogin.Name;
                                //cria uma session para armazenar o sobrenome do usuário
                                Session["LastName"] = getLogin.LastName;
                                //retorna para a tela inicial do Home
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                //Escreve na tela a mensagem de erro informada
                                ModelState.AddModelError("Password", "Invalid Password. =/");
                                //Retorna a tela de login
                                return View(new Access());
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Usuário sem acesso para usar o sistema!!!");
                            return View(new Access());
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Email", @"Invalid Email. ");
                        return View(new Access());
                    }
                }
            }
            //Caso os campos não esteja de acordo com a solicitação retorna a tela de login com as mensagem dos campos
            return View(login);
        }

    }
}