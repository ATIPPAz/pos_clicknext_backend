using Microsoft.AspNetCore.Mvc;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;
using PosApi.Models;
using System.Security.AccessControl;

namespace PosApi.Helpers
{
    public class ResponseHelper
    {
        public JsonResult JsonError()
        {
            return new JsonResult(new ResponseObject
            {
                data = null,
                statusCode = Models.StatusCode.error
            });
        }
        public JsonResult JsonCreate()
        {
            return new JsonResult(new ResponseObject
            {
                data = null,
                statusCode = Models.StatusCode.created
            });
        }
        public JsonResult JsonUpdate()
        {
            return new JsonResult(new ResponseObject
            {
                data = null,
                statusCode = Models.StatusCode.successNoReturn
            });
        }
        public JsonResult JsonDelete()
        {
            return new JsonResult(new ResponseObject
            {
                data = null,
                statusCode = Models.StatusCode.successNoReturn
            });
        }
        public JsonResult JsonGet<T>(T data)
        {
            return new JsonResult(new ResponseObject<T>
            {
                data = data,
                statusCode = Models.StatusCode.successReturn
            });
        }
    }
}
