using System;

namespace OpenWebTech.Infrastructure.Exceptions
{
    public class BusinnessDomainException : ApplicationException
    {
        public BusinnessDomainException(): base() {}

        public BusinnessDomainException(string Contact) : base(Contact) { }

        public BusinnessDomainException(string Contact, Exception ex): base(Contact, ex) { }
    }
}