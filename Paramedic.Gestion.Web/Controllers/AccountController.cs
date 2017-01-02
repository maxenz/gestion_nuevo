using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using PagedList;
using System.Data;
using System.Net.Mail;
using System.Data.Entity;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Service;
using LinqKit;

namespace Gestion.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class AccountController : Controller
    {
        #region Properties

        IAccountService _AccountService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors 
        public AccountController(IAccountService AccountService)
        {
            _AccountService = AccountService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {

            var predicate = PredicateBuilder.New<UserProfile>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.UserName.Contains(searchName)) : null;

            IEnumerable<UserProfile> userProfiles = _AccountService.FindByPage(predicate, "UserName ASC", controllersPageSize, page);
            int count = _AccountService.GetCount(predicate);
            var resultAsPagedList = new StaticPagedList<UserProfile>(userProfiles, page, controllersPageSize, count);     

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Usuarios", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Edit(int id = 0)
        {

            UserProfile user = _AccountService.FindBy(x => x.Id == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }

            RegisterModel regModel = new RegisterModel
            {
                Id = user.Id,
                Apellido = user.Apellido,
                Nombre = user.Nombre,
                UserName = user.UserName,
                Email = user.Email
            };

            setRoles(_AccountService.getSelectedRole(user.UserName));

            return View(regModel);
        }

        //private string GeneratePassword()
        //{
        //    string strPwdchar = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    string strPwd = "";
        //    Random rnd = new Random();
        //    for (int i = 0; i <= 7; i++)
        //    {
        //        int iRandom = rnd.Next(0, strPwdchar.Length - 1);
        //        strPwd += strPwdchar.Substring(iRandom, 1);
        //    }
        //    return strPwd;
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //public int RecoverPassword(string email)
        //{
        //    int retV = 0;
        //    if (Request.IsAjaxRequest())
        //    {
        //        int cant = db.UserProfiles.Where(a => a.Email == email).Count();
        //        string userName = db.UserProfiles.Where(a => a.Email == email).Select(a => a.UserName).FirstOrDefault();

        //        if (cant > 0)
        //        {
        //            retV = 1;
        //            string newPassword = GeneratePassword();
        //            string loginShaman = "http://200.49.156.125:57771/Account/Login";
        //            var token = WebSecurity.GeneratePasswordResetToken(userName);
        //            WebSecurity.ResetPassword(token, newPassword);
        //            string msgMail = String.Format("Estimado/a: Su nuevo password para ingresar a Shaman Gestión es {1}.{0}" +
        //                                           "Recuerde que puede modificarlo desde su panel de control. {0}{0} " +
        //                                           "Login Shaman Gestión: {2}", Environment.NewLine, newPassword, loginShaman);

        //            sendEmail(email, "Shaman Gestión - Recuperación de Password", msgMail);
        //        }


        //    }
        //    return retV;


        //}

        //private void sendEmail(string to, string subject, string body)
        //{
        //    MailMessage mailMsg = new MailMessage();
        //    mailMsg.To.Add(to);
        //    MailAddress mailAddress = new MailAddress("sistemas@paramedic.com.ar");
        //    mailMsg.From = mailAddress;
        //    mailMsg.Subject = subject;
        //    mailMsg.Body = body;
        //    mailMsg.IsBodyHtml = true;

        //    SmtpClient smtpClient = new SmtpClient("smtp.fibertel.com.ar", 25);
        //    System.Net.NetworkCredential credentials =
        //       new System.Net.NetworkCredential("sistemas.paramedic.com.ar", "pwsi01");
        //    smtpClient.Credentials = credentials;
        //    smtpClient.Send(mailMsg);

        //}

        [HttpPost]
        public ActionResult Edit(RegisterModel model, string selectedRole)
        {

            if (ModelState.IsValid)
            {
                UserProfile user = _AccountService.FindBy(x => x.Id == model.Id).FirstOrDefault();
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Apellido = model.Apellido;
                user.Nombre = model.Nombre;

                if (!model.Password.Equals("") && !model.ConfirmPassword.Equals(""))
                {

                    var token = WebSecurity.GeneratePasswordResetToken(model.UserName);
                    WebSecurity.ResetPassword(token, model.Password);
                }

                _AccountService.Update(user);
                _AccountService.UpdateRole(selectedRole, user.UserName);

                return RedirectToAction("Index");
            }

            setRoles("Administrador");
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("Login", "_TemplateGestion_Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                //int userID = _AccountService.FindBy(x => x.UserName == model.UserName).FirstOrDefault().Id;
                //LogRegistroSistema log = new LogRegistroSistema("LOGUEO SISTEMA", userID);
                //db.LogsRegistroSistema.Add(log);
                //db.SaveChanges();
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "Usuario y/o contraseña incorrecto/s.");
            return View("Login", "_TemplateGestion_Login", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Login", "Account");
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, string selectedRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { Email = model.Email, Nombre = model.Nombre, Apellido = model.Apellido }, false);
                    Roles.AddUserToRoles(model.UserName, new[] { selectedRole });
                    return RedirectToAction("Index", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));

                }
            }

            setRoles("Administrador");
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile user = _AccountService.DeleteUser(id);
            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(user.UserName); // deletes record from webpages_Membership table
            ((SimpleMembershipProvider)Membership.Provider).DeleteUser(user.UserName, true); // deletes record from UserProfile table
            return RedirectToAction("Index");
        }

        //public ActionResult Manage(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
        //        : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
        //        : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //        : "";
        //    ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Manage(LocalPasswordModel model)
        //{
        //    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //    ViewBag.HasLocalPassword = hasLocalAccount;
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    if (hasLocalAccount)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // ChangePassword will throw an exception rather than return false in certain failure scenarios.
        //            bool changePasswordSucceeded;
        //            try
        //            {
        //                changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
        //            }
        //            catch (Exception)
        //            {
        //                changePasswordSucceeded = false;
        //            }

        //            if (changePasswordSucceeded)
        //            {
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // User does not have a local password so remove any validation errors caused by a missing
        //        // OldPassword field
        //        ModelState state = ModelState["OldPassword"];
        //        if (state != null)
        //        {
        //            state.Errors.Clear();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //            }
        //            catch (Exception e)
        //            {
        //                ModelState.AddModelError("", e);
        //            }
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //}

        //public ActionResult ExternalLoginCallback(string returnUrl)
        //{
        //    AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //    if (!result.IsSuccessful)
        //    {
        //        return RedirectToAction("ExternalLoginFailure");
        //    }

        //    if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
        //    {
        //        return RedirectToLocal(returnUrl);
        //    }

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        // If the current user is logged in add the new account
        //        OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else
        //    {
        //        // User is new, ask for their desired membership name
        //        string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
        //        ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
        //        ViewBag.ReturnUrl = returnUrl;
        //        return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        //{
        //    string provider = null;
        //    string providerUserId = null;

        //    if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
        //    {
        //        return RedirectToAction("Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Insert a new user into the database
        //        using (var db = new GestionDb())
        //        {
        //            UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
        //            // Check if user already exists
        //            if (user == null)
        //            {
        //                // Insert name into the profile table
        //                db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
        //                db.SaveChanges();

        //                OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
        //                OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

        //                return RedirectToLocal(returnUrl);
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
        //            }
        //        }
        //    }

        //    ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        //[ChildActionOnly]
        //public ActionResult ExternalLoginsList(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        //}

        //[ChildActionOnly]
        //public ActionResult RemoveExternalLogins()
        //{
        //    ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
        //    List<ExternalLogin> externalLogins = new List<ExternalLogin>();
        //    foreach (OAuthAccount account in accounts)
        //    {
        //        AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

        //        externalLogins.Add(new ExternalLogin
        //        {
        //            Provider = account.Provider,
        //            ProviderDisplayName = clientData.DisplayName,
        //            ProviderUserId = account.ProviderUserId,
        //        });
        //    }

        //    ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //    return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        //}

        #endregion

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
