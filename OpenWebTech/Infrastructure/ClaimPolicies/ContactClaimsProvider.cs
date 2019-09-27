using Microsoft.AspNetCore.Authentication;
using OpenWebTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebMVC.Infrastructure.Claims
{
    public class ContactClaimsProvider : IClaimsTransformation
    {
        private IContactsRepository _repository;

        public ContactClaimsProvider(IContactsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null && !principal.HasClaim(c => c.Type == ClaimTypes.PostalCode))
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
                if (identity != null && identity.IsAuthenticated && identity.Name != null)
                {
                    //var contact = _repository.retrieveAllContacts().FirstOrDefault<Contact>
                    
                }
            }
            return Task.FromResult(principal);
        }

        private static Claim CreateClaim(string type, string value) 
            => new Claim(type, value, ClaimValueTypes.String, "RemoteClaims");
    }
}
