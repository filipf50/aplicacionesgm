using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AplicacionesGM_MVC.Models;
using AplicacionesGM_MVC.Areas.Clientes.Models.DSas400TableAdapters;
using System.Web.Profile;

namespace AplicacionesGM_MVC.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        private const string ResetPasswordBody = "Su nueva contraseña es: ";
        private const string ResetPasswordFromAddress = "falvarez@grupomora.com";
        private const string ResetPasswordSubject = "Reuniones - Nueva contraseña asignada";

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/Admin
        // **************************************

        public ActionResult Admin()
        {
            Models.AccountIndexViewModel  UsersList = new AccountIndexViewModel
							{
								Users = Membership.GetAllUsers()
							};

            return View(UsersList);
        }

        // **************************************
        // URL: /Account/Admin
        // **************************************

        [Authorize]
        public ViewResult Edit(string userName)
        {
            var user = Membership.GetUser(userName);
            var userRoles = Roles.GetRolesForUser(userName);
            AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
            IEnumerable<aspnet_Users> usuarios = from u in db.aspnet_Users select u;
            IEnumerable<aspnet_Origenes> origenes = from o in db.aspnet_Origenes select o;
            IEnumerable<aspnet_AplicacionesGM> aplicaciones = from a in db.aspnet_AplicacionesGM select a;
            
            ProfileBase profile = ProfileBase.Create(user.UserName);

            //Agentes de Moraval
            Areas.Clientes.Models.DSas400TableAdapters.AGTAG003TableAdapter taMV = new Areas.Clientes.Models.DSas400TableAdapters.AGTAG003TableAdapter();
            var agentesMV = (from a in taMV.GetData() select new { a.AGCDG, NOMBRE = a.AGNBR + " (" + a.AGCDG + ")" }).OrderBy(a=>a.NOMBRE);
            SelectList list = new SelectList(agentesMV.AsEnumerable(), "AGCDG", "NOMBRE", profile["IDAgenteQSMV"]);
            ViewData["AgentesMV"] = list;


            //Agentes de Hierros
            Areas.Clientes.Models.DSas400TableAdapters.AGTAG004TableAdapter taHMA = new Areas.Clientes.Models.DSas400TableAdapters.AGTAG004TableAdapter();
            var agentesHMA = (from a in taHMA.GetData() select new { a.AGCDG, NOMBRE = a.AGNBR + " (" + a.AGCDG + ")" }).OrderBy(a => a.NOMBRE);
            list = new SelectList(agentesHMA.AsEnumerable(), "AGCDG", "NOMBRE", profile["IDAgenteQSHMA"]);
            ViewData["AgentesHMA"] = list;

            //Agentes de ECA
            Areas.Clientes.Models.DSas400TableAdapters.AGTAG006TableAdapter taECA = new Areas.Clientes.Models.DSas400TableAdapters.AGTAG006TableAdapter();
            var agentesECA = (from a in taECA.GetData() select new { a.AGCDG, NOMBRE = a.AGNBR + " (" + a.AGCDG + ")" }).OrderBy(a => a.NOMBRE);
            list = new SelectList(agentesECA.AsEnumerable(), "AGCDG", "NOMBRE", profile["IDAgenteQSECA"]);
            ViewData["AgentesECA"] = list;

            return View(new AccountEditViewModel
            {
                DisplayName = user.UserName,
                User = user,
                Roles = Roles.GetAllRoles().ToDictionary(role=>role,role=>userRoles.Contains(role)),
                Status = user.IsOnline
                            ? AccountEditViewModel.StatusEnum.Online
                            : !user.IsApproved
                                ? AccountEditViewModel.StatusEnum.Unapproved
                                : user.IsLockedOut
                                    ? AccountEditViewModel.StatusEnum.LockedOut
                                    : AccountEditViewModel.StatusEnum.Offline,
                Subordinados=new CheckBoxModel(usuarios.AsEnumerable().OrderBy(u => u.UserName).ToDictionary(u => u.UserId.ToString(), u => u.UserName), new List<string>()),
                Origenes = new CheckBoxModel(origenes.AsEnumerable().OrderBy(o => o.Descripcion).ToDictionary(o => o.OrigenID.ToString(), o => o.Descripcion), new List<string>()),
                Aplicaciones= new CheckBoxModel(aplicaciones.AsEnumerable().OrderBy(a=> a.Nombre).ToDictionary(a => a.AplicacionesGMID.ToString(), a=>a.Nombre), new List<string>())
            });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(string userName,
									[Bind(Prefix = "E-Mail")] string email,
									[Bind(Prefix = "Comentarios")] string comment)
		{
			var user = Membership.GetUser(userName);
			user.Email = email;
			user.Comment = comment;
            Membership.UpdateUser(user);
            return RedirectToAction("Edit", new { userName });
        }

        [Authorize]
        public ViewResult Create()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(model.UserName, "Responsable");
                    return RedirectToAction("Admin");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult ChangeApproval(string userName, bool isApproved)
        {
            var user = Membership.GetUser(userName);
            user.IsApproved = isApproved;
            Membership.UpdateUser(user);
            return RedirectToAction("Edit", new { userName });
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult AddToRole(string userName, string role)
        {
            Roles.AddUserToRole(userName, role);
            return RedirectToAction("Edit", new { userName });
        }
        
        [Authorize]
        [HttpPost]
        public RedirectToRouteResult RemoveFromRole(string userName, string role)
        {
            Roles.RemoveUserFromRole(userName, role);
            return RedirectToAction("Edit", new { userName });
        }
        
        [Authorize]
        [HttpPost]
        public RedirectToRouteResult ResetPassword(string userName, [Bind(Prefix = "nuevaContraseña")] string newPassword)
        {
            var user = Membership.GetUser(userName);
            user.ChangePassword(user.ResetPassword(),newPassword);

            return RedirectToAction("ChangePasswordSuccess");
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult Unlock(string userName)
        {
            var user = Membership.GetUser(userName);
            user.UnlockUser();
            return RedirectToAction("Edit", new { userName });
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult GuardarSubordinados(string userName, FormCollection formData)
        {
            ProfileBase profile = ProfileBase.Create(userName);
            profile["Subordinados"] = formData["arrSubordinados"].Replace("false,", "").Replace(",false", "").Replace("false", "");
            profile.Save();

            return RedirectToAction("Admin");
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult GuardarOrigenes(string userName, FormCollection formData)
        {
            ProfileBase profile = ProfileBase.Create(userName);
            profile["Origenes"] = formData["arrOrigenes"].Replace("false,", "").Replace(",false", "").Replace("false", "");
            profile.Save();

            return RedirectToAction("Admin");
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult GuardarAplicaciones(string userName, FormCollection formData)
        {
            ProfileBase profile = ProfileBase.Create(userName);
            profile["AplicacionesGM"] = formData["arrAplicacionesGM"].Replace("false,", "").Replace(",false", "").Replace("false", "");
            profile.Save();

            return RedirectToAction("Admin");
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult GuardarAgenteQS(string userName, FormCollection formData)
        {
            ProfileBase profile = ProfileBase.Create(userName);
            profile["IDAgenteQSMV"] = formData["IDAgenteQSMV"].ToString();
            profile["IDAgenteQSHMA"] = formData["IDAgenteQSHMA"].ToString();
            profile["IDAgenteQSECA"] = formData["IDAgenteQSECA"].ToString();
            profile.Save();

            return RedirectToAction("Admin");
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        //Limpiamos las variables de sesión
                        Session.Clear();

                        //Cargamos en la sesion las aplicaciones a las que tiene acceso el usuario
                        ProfileBase profile = ProfileBase.Create(model.UserName);
                        Session["AplicacionesGM"] = profile["AplicacionesGM"].ToString().Split(',');


                        if (profile["AplicacionesGM"].ToString().Split(',').Count() > 1)
                        {
                            //Si tiene acceso a más de una aplicación, mostramos la Home para que eliga la aplicación a la que quiere acceder
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }
                        else
                        {
                            //Si sólo tiene acceso a una aplicación accedemos directamente a ella.
                            //NOTA: Hacer dinámico
                            if (profile["AplicacionesGM"].ToString() == "1")
                            {
                                return RedirectToAction("Index", "Acciones", new { area = "Reuniones" });
                            }
                            else
                            {
                                return RedirectToAction("Index", "Clientes", new { area = "Clientes" });
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El usuario o contraseña indicados son incorrectos.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("LogOn", "Account");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);
                
                if (createStatus == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(model.UserName, "Responsable");
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Acciones");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "La contraseña actual indicada no es correcta o la nueva contraseña no es valida.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        // **************************************
        // URL: /Account/UpdateUser
        // **************************************

        [Authorize]
        public ActionResult UpdateUser()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateUser(MembershipUser model)
        {
            if (ModelState.IsValid)
            {
                Membership.UpdateUser(model);
                return RedirectToAction("UpdateUserSuccess");
            }

            return View(model);
        }

        // **************************************
        // URL: /Account/UpdateUserSuccess
        // **************************************

        public ActionResult UpdateUserSuccess()
        {
            return View();
        }
    }
}
