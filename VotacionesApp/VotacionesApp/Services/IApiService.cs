using System;
using VotacionesApp.Models;
using System.Threading.Tasks;

namespace VotacionesApp.Services
{
    public interface IApiService
    {
        Task<Response<object>> CheckConnection();

        Task<Response<TokenResponse>> GetTokenAsync(
            string urlBase,
            string username,
            string password);

        Task<Response<UserResponse>> GetUserAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            TokenRequest tokenRequest);

        Task<Response<object>> GetVotingsAsync<VotingResponse>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);

        Task<Response<object>> PutAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken);

        Task<Response<object>> GetResultsAsync<VotingResponse>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);

        Task<Response<object>> PostAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken);

        Task<Response<object>> GetListAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);
        Task<Response<CertificateResponse>> GetCertificatesAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);
    }
}
