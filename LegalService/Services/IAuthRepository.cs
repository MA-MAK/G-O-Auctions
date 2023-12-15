using LegalService.Models;
using System.Net.Http;

namespace LegalService.Services
{
    public interface IAuthRepository
    {
        public Task<HttpResponseMessage> Login(LoginModel login);
    }
}
