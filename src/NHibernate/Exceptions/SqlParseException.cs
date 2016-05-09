using System;

namespace NHibernate.Exceptions
{
    public partial class SqlParseException : Exception 
    {

        public SqlParseException(string Message) : base(Message)
        {
        }

    }
}
