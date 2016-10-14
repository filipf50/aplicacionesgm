using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AplicacionesGM_MVC.Models
{

    #region Models
    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "La nueva contraseña y su confirmación no coincide.")]
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña actual")]
        public string OldPassword { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Nueva contraseña")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirmar nueva contraseña")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [DisplayName("Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [DisplayName("Recordarme?")]
        public bool RememberMe { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
    public class RegisterModel
    {
        [Required]
        [DisplayName("Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirmación de contraseña")]
        public string ConfirmPassword { get; set; }
    }

    public class AccountIndexViewModel
    {
        public MembershipUserCollection  Users { get; set; }
    }

    public class AccountEditViewModel
    {
        #region StatusEnum enum

        public enum StatusEnum
        {
            Offline,
            Online,
            LockedOut,
            Unapproved
        }

        #endregion

        public string DisplayName { get; set; }
        public StatusEnum Status { get; set; }
        public MembershipUser User { get; set; }
        public IDictionary<string, bool> Roles { get; set; }
        public CheckBoxModel Subordinados { get; set; }
        public CheckBoxModel Origenes { get; set; }
        public CheckBoxModel Aplicaciones { get; set; }
        public CheckBoxModel Delegaciones { get; set; }
    }
    #endregion

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El valor no puede ser nulo o vacío.", "Usuario");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("El valor no puede ser nulo o vacío.", "Contraseña");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El valor no puede ser nulo o vacío.", "Usuario");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("El valor no puede ser nulo o vacío.", "Contraseña");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("El valor no puede ser nulo o vacío.", "Email");

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El valor no puede ser nulo o vacío.", "Usuario");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("El valor no puede ser nulo o vacío.", "Antigua contraseña");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("El valor no puede ser nulo o vacío.", "Nueva contraseña");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El valor no puede ser nulo o vacío.", "Usuario");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion

    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "El usuario ya existe. Por favor introduzca uno diferente.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Ya existe un usario relacionado con esta dirección de mail. Por favor introduzca otra dirección de mail.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La contraseña no es correcta. Por favor, vuelva a intentarlo.";

                case MembershipCreateStatus.InvalidEmail:
                    return "La dirección de correo no es correcta. Por favor, revisela y vuelva a intentarlo";

                case MembershipCreateStatus.InvalidAnswer:
                    return "La respuesta proporcionada no es valida. Por favor, revise el valor y vuelva a intentarlo.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "La pregunta proporciona no es valida. Por favor, revise el valor y vuelva a intentarlo.";

                case MembershipCreateStatus.InvalidUserName:
                    return "El usuario introducido es incorrecto. Por favor, vuelva a intentarlo.";

                case MembershipCreateStatus.ProviderError:
                    return "La autentifcación ha sido errónea. Por favor, vuelva a intentarlo y si el problema persiste contacte con su administrador.";

                case MembershipCreateStatus.UserRejected:
                    return "La creación del usuairo ha sido cancelada. Por favor, vuelva intentarlo y si el problema persiste contacte con su administrador.";

                default:
                    return "Se ha producido un error desconocido. Por favor, vuelva a intentarlo y si el problema persiste contacte con su administrador.";
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' y '{1}' no coinciden.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' debe de tener al menos {1} caracteres.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    #endregion

}
