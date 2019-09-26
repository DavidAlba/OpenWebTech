using System;

namespace OpenWebTech.Infrastructure.Exceptions
{
    public class NotFoundBusinessEntityException : ApplicationException
    {
        public NotFoundBusinessEntityException(): base() {}

        public NotFoundBusinessEntityException(string Contact) : base(Contact) { }

        public NotFoundBusinessEntityException(string Contact, Exception ex): base(Contact, ex) { }
    }
}