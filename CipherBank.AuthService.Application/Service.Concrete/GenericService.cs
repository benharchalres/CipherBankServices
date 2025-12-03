using CipherBank.AuthService.Application.DTOs;
using CipherBank.AuthService.Application.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Service.Concrete
{
    public class GenericService:IGenericService
    {
        public ResponseDto<T> BuildResponseModel<T>(string message, T result, bool? isSuccess = true, string responseCode = "200")
        {
            return new ResponseDto<T>
            {
                Data = result,
                Code = !string.IsNullOrEmpty(responseCode) ? responseCode : "200",
                IsSuccess = isSuccess.GetValueOrDefault(true),
                Message = message,
            };
        }
    }
}
