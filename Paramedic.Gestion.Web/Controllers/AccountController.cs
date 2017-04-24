using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using PagedList;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Service;
using LinqKit;
using System.Text.RegularExpressions;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AccountController : Controller
    {
        #region Properties

        IAccountService _AccountService;
        IUserProfileService _UserProfileService;
        IUserProfileEmailService _UserProfileEmailService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors 
        public AccountController(IAccountService AccountService, IUserProfileService UserProfileService, IUserProfileEmailService UserEmailProfileService)
        {
            _AccountService = AccountService;
            _UserProfileService = UserProfileService;
            _UserProfileEmailService = UserEmailProfileService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {

            var predicate = PredicateBuilder.New<UserProfile>();
            if (!string.IsNullOrEmpty(searchName))
            {
                var description = searchName.ToUpper();
                predicate = predicate.Or(x => x.Apellido.ToUpper().Contains(description));
                predicate = predicate.Or(x => x.Emails.Any(q => q.Email.ToUpper().Contains(description)));
                predicate = predicate.Or(x => x.Nombre.ToUpper().Contains(description));
                predicate = predicate.Or(x => x.UserName.ToUpper().Contains(description));
            }
            else
            {
                predicate = null;
            }

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

            UserProfile user = _UserProfileService.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            RegisterModel regModel = new RegisterModel
            {
                Id = user.Id,
                Apellido = user.Apellido,
                Nombre = user.Nombre,
                UserName = user.UserName
            };

            ViewBag.Emails = user.Emails.Select(item => item.Email).ToList();
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
        public ActionResult Edit(RegisterModel model, string selectedRole, string[] emails)
        {

            if (string.IsNullOrEmpty(emails[0]) || !IsValidEmail(emails[0]))
            {
                emails[0] = "";
                setRoles("Administrador");
                ModelState.AddModelError("Emails", "Debe ingresar el menos un email");
                return View();
            }

            if (!model.ChangePassword)
            {
                ModelState.Remove("Password");
            }

            if (ModelState.IsValid)
            {
                List<UserProfileEmail> dbMails = _UserProfileEmailService.FindBy(x => x.UserProfileId == model.Id).ToList();
                List<string> mails = emails.ToList();

                foreach (var m in dbMails)
                {
                    var mailInForm = mails.FirstOrDefault(x => x.ToUpper() == m.Email.ToUpper());
                    if (mailInForm == null)
                    {
                        if (!m.EmailPrincipal)
                        {
                            _UserProfileEmailService.Delete(m);
                        }
                    }
                    else
                    {
                        mails.Remove(mailInForm);
                    }
                }

                UserProfile user = _UserProfileService.GetById(model.Id);
                user.UserName = model.UserName;
                user.Apellido = model.Apellido;
                user.Nombre = model.Nombre;

                for (var i = 0; i < mails.Count; i++)
                {
                    if (!string.IsNullOrEmpty(mails[i]))
                    {
                        _UserProfileEmailService.Create(new UserProfileEmail(model.Id, mails[i], false));
                    }
                }

                if (model.ChangePassword)
                {
                    if (!model.Password.Equals("") && !model.ConfirmPassword.Equals(""))
                    {

                        var token = WebSecurity.GeneratePasswordResetToken(model.UserName);
                        WebSecurity.ResetPassword(token, model.Password);
                    }
                }

                _AccountService.Update(user);
                _AccountService.UpdateRole(selectedRole, user.UserName);

                return RedirectToAction("Index");
            }

            ViewBag.Emails = emails;
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

            if (IsValidEmail(model.UserName))
            {
                UserProfileEmail userProfileEmail = _UserProfileEmailService.FindBy(x => x.Email.ToUpper() == model.UserName.ToUpper()).FirstOrDefault();
                if (userProfileEmail != null)
                {
                    UserProfile userProfile = _UserProfileService.FindBy(x => x.Id == userProfileEmail.UserProfileId).FirstOrDefault();
                    if (userProfile != null)
                    {
                        if (ModelState.IsValid && WebSecurity.Login(userProfile.UserName, model.Password, model.RememberMe))
                        {
                            return RedirectToLocal(returnUrl);
                        }
                    }
                }
            }
            else
            {
                if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
                {
                    return RedirectToLocal(returnUrl);
                }
            }

            ModelState.AddModelError("", "Datos incorrectos.");
            return View("Login", "_TemplateGestion_Login", model);
        }

        bool IsValidEmail(string strIn)
        {
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
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
        public ActionResult Register(RegisterModel model, string selectedRole, string[] emails)
        {
            if (string.IsNullOrEmpty(emails[0]) || !IsValidEmail(emails[0]))
            {
                emails[0] = "";
                setRoles("Administrador");
                ModelState.AddModelError("Emails", "Debe ingresar el menos un email");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { Nombre = model.Nombre, Apellido = model.Apellido }, false);
                    Roles.AddUserToRoles(model.UserName, new[] { selectedRole });
                    int id = _UserProfileService.FindBy(x => x.UserName == model.UserName).FirstOrDefault().Id;
                    for (var i = 0; i < emails.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(emails[i]))
                        {
                            _UserProfileEmailService.Create(new UserProfileEmail(id, emails[i], i == 0));
                        }
                    }

                    return RedirectToAction("Index", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));

                }
            }

            setRoles("Administrador");
            ViewBag.Emails = emails;
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
