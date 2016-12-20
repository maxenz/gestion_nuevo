using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Gestion.Models;
using PagedList;
using System.Data;
using System.Net.Mail;
using System.Data.Entity;

namespace Gestion.Controllers
{
  [Authorize(Roles = "Administrador")]
    public class AccountController : Controller
    {

         private GestionDb db = new GestionDb();

        //
        // GET: /ClientesUsuarios/
           
         public ActionResult Index(string searchName = null, int page = 1)
         {
             var qUsr = from p in db.UserProfiles select p;

             if (!String.IsNullOrEmpty(searchName))
             {

                 qUsr = qUsr.Where(p => p.UserName.ToUpper().Contains(searchName.ToUpper()));

             }

             qUsr = qUsr.OrderBy(p => p.UserName);

             if (Request.IsAjaxRequest())
             {
                 return PartialView("_Usuarios", qUsr.ToPagedList(page, 5));
             }

             return View(qUsr.ToPagedList(page, 5));
         }

         public ActionResult Edit(int id = 0)
         {
             UserProfile user = db.UserProfiles.Find(id);
             if (user == null)
             {
                 return HttpNotFound();
             }

             RegisterModel regModel = new RegisterModel
                                        {
                                            ID = user.UserId,
                                            Apellido = user.Apellido,
                                            Nombre = user.Nombre,
                                            UserName = user.UserName,
                                            Email = user.Email
                                        };

             if (HasRole("Cliente",user.UserName))
             {
                 setRoles("Cliente");
             }
             else if (HasRole("Administrador",user.UserName))
             {
                 setRoles("Administrador");
             }
             else
             {
                 setRoles("Cliente ticket");
             }
                      
             return View(regModel);
         }

         private bool HasRole(string role, string userName)
         {
             return Roles.FindUsersInRole(role, userName).Length > 0;
         }

         private string GeneratePassword()
         {
             string strPwdchar = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";
             string strPwd = "";
             Random rnd = new Random();
             for (int i = 0; i <= 7; i++)
             {
                 int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                 strPwd += strPwdchar.Substring(iRandom, 1);
             }
             return strPwd;
         }
        
         [AllowAnonymous]
         [HttpPost]
         public int RecoverPassword(string email)
         {
             int retV = 0;
             if (Request.IsAjaxRequest())
             {
                 int cant = db.UserProfiles.Where(a => a.Email == email).Count();
                 string userName = db.UserProfiles.Where(a => a.Email == email).Select(a => a.UserName).FirstOrDefault();

                 if (cant > 0)
                 {
                     retV = 1;
                     string newPassword = GeneratePassword();
                     string loginShaman = "http://200.49.156.125:57771/Account/Login";
                     var token = WebSecurity.GeneratePasswordResetToken(userName);
                     WebSecurity.ResetPassword(token, newPassword);
                     string msgMail = String.Format("Estimado/a: Su nuevo password para ingresar a Shaman Gestión es {1}.{0}" +
                                                    "Recuerde que puede modificarlo desde su panel de control. {0}{0} " +
                                                    "Login Shaman Gestión: {2}", Environment.NewLine, newPassword, loginShaman);

                     sendEmail(email, "Shaman Gestión - Recuperación de Password", msgMail);
                 }


             }
             return retV;


         }

         private void sendEmail(string to, string subject, string body)
         {
             MailMessage mailMsg = new MailMessage();
             mailMsg.To.Add(to);
             MailAddress mailAddress = new MailAddress("sistemas@paramedic.com.ar");
             mailMsg.From = mailAddress;
             mailMsg.Subject = subject;
             mailMsg.Body = body;
             mailMsg.IsBodyHtml = true;

             SmtpClient smtpClient = new SmtpClient("smtp.fibertel.com.ar", 25);
             System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("sistemas.paramedic.com.ar", "pwsi01");
             smtpClient.Credentials = credentials;
             smtpClient.Send(mailMsg);

         }


         [HttpPost]
         public ActionResult Edit(RegisterModel model, string selectedRole)
         {
          
             if (ModelState.IsValid)
             {
                 var usr = db.UserProfiles.Find(model.ID);
                 usr.UserName = model.UserName;
                 usr.Email = model.Email;
                 usr.Apellido = model.Apellido;
                 usr.Nombre = model.Nombre;

                 if (!model.Password.Equals("") && !model.ConfirmPassword.Equals(""))
                 {
                   
                     var token = WebSecurity.GeneratePasswordResetToken(model.UserName);
                     WebSecurity.ResetPassword(token, model.Password);
                 }

                 db.Entry(usr).State = EntityState.Modified;
                 db.SaveChanges();

                 var rol = Roles.FindUsersInRole( "Administrador",model.UserName);
                 if (HasRole("Administrador",model.UserName))
                 {
                     Roles.RemoveUserFromRole(model.UserName, "Administrador");
                 }
                 else if (HasRole("Cliente", model.UserName))
                 {
                     Roles.RemoveUserFromRole(model.UserName, "Cliente");
                 }
                 else
                 {
                     Roles.RemoveUserFromRole(model.UserName, "Cliente ticket");
                 }
                               
                 if (selectedRole == "Administrador")
                 {
                     Roles.AddUserToRole(model.UserName, "Administrador");
                 }
                 else if (selectedRole == "Cliente")
                 {
                     Roles.AddUserToRole(model.UserName, "Cliente");
                 }
                 else
                 {
                     Roles.AddUserToRole(model.UserName, "Cliente ticket");
                 }


                 return RedirectToAction("Index");
             }

             setRoles("Administrador");
             return View();
         }

        //
        // GET: /Account/Login

      [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            //return View("Login", "_LayoutLogin");
            return View("Login", "_TemplateGestion_Login");
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                int userID = db.UserProfiles.Where(x => x.UserName == model.UserName).Select(x => x.UserId).FirstOrDefault();
                LogRegistroSistema log = new LogRegistroSistema("LOGUEO SISTEMA", userID);
                db.LogsRegistroSistema.Add(log);
                db.SaveChanges();
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Usuario y/o contraseña incorrecto/s.");
            return View("Login","_TemplateGestion_Login",model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Login","Account");
        }

        //
        // GET: /Account/Register

        private void setRoles(string selValue)
        {
            var rolesCollection = new List<string> { "Administrador", "Cliente", "Cliente ticket" };
            ViewBag.Roles = new SelectList(rolesCollection, selValue);
        }

        public ActionResult Register()
        {
            setRoles("Administrador");
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model,string selectedRole)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { Email = model.Email, Nombre = model.Nombre, Apellido = model.Apellido }, false);
                    Roles.AddUserToRoles(model.UserName, new[] { selectedRole });
                    //WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));

                }
            }

            // If we got this far, something failed, redisplay form
            setRoles("Administrador");
            return View(model);
        }


        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (var db = new GestionDb())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        //
        // POST: /Licencias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var usr = db.UserProfiles.Find(id);
            var userName = usr.UserName;
            var rolAsignado = Roles.FindUsersInRole("Administrador", userName);

            if (HasRole("Administrador",userName))
            {
                Roles.RemoveUserFromRole(userName, "Administrador");
            }
            else if (HasRole("Cliente",userName))
            {
                Roles.RemoveUserFromRole(userName, "Cliente");
            }
            else
            {
                Roles.RemoveUserFromRole(userName, "Cliente ticket");
            }

            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(userName); // deletes record from webpages_Membership table
            ((SimpleMembershipProvider)Membership.Provider).DeleteUser(userName, true); // deletes record from UserProfile table
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
