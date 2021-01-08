using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Gateway
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode Status { get; }

        public HttpResponseException(HttpStatusCode status)
        {
            Status = status;
        }
    }
}
