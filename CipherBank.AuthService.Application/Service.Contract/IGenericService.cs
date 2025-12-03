using CipherBank.AuthService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBank.AuthService.Application.Service.Contract
{
    public interface IGenericService
    {
        ResponseDto<T> BuildResponseModel<T>(string message, T result, bool? isSuccess = true, string responseCode = "200");
    }
}
