using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Aplicacion.DTOsUser
{
    public class RespuestaAPI
    {
        public RespuestaAPI()
        {
            this.ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool  IsSuccess { get; set; }
        public object Result { get; set; }
    }
}
