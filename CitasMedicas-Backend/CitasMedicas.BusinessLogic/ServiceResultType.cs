using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.BusinessLogic
{
    public enum ServiceResultType
    {
        Info = 100,
        Success = 200,
        Created = 201,
        Warning = 202,
        Forbidden = 203,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        NotAcceptable = 406,
        Conflict = 409,
        Disabled = 410,
        Error = 500,
        DatabaseError = 501,
        ValidationError = 422,
        Informational = 503
    }
}
