using System;
using System.Web;
using System.Web.Security;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;

namespace Nop.Services.Authentication
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public partial class FormsAuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly CustomerSettings _customerSettings;
        private readonly TimeSpan _expirationTimeSpan;

        private Customer _cachedCustomer;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="customerSettings">Customer settings</param>
        public FormsAuthenticationService(HttpContextBase httpContext,
            ICustomerService customerService, CustomerSettings customerSettings)
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
            this._customerSettings = customerSettings;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
        }


        public virtual void SignIn(Customer customer, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                customer.Password,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                _customerSettings.UsernamesEnabled ? customer.Username : customer.Email,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
            _cachedCustomer = customer;
        }

        public virtual void SignOut()
        {
            _cachedCustomer = null;
            FormsAuthentication.SignOut();
        }

        public virtual bool CheckChangePass(Customer customer)
        {
            var _customer = _customerService.GetCustomerById(customer.Id);
            if (_customer.Password == customer.Password && _customer.PasswordSalt == customer.PasswordSalt)
                return true;
            else
                return false;
        }

        public virtual Customer GetAuthenticatedCustomer()
        {
            if (_cachedCustomer != null)
            {
                return _cachedCustomer;
            }

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);
            if (customer != null && customer.Active && !customer.Deleted && customer.IsRegistered())
                _cachedCustomer = customer;

            return _cachedCustomer;
        }

        public virtual Customer GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");
            var password = ticket.Name;
            var usernameOrEmail = ticket.UserData;

            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;
            var customer = _customerSettings.UsernamesEnabled
                ? _customerService.GetCustomerByUsername(usernameOrEmail)
                : _customerService.GetCustomerByEmail(usernameOrEmail);
            if (password == customer.Password)
                return customer;
            else
                return null;
        }
    }
}