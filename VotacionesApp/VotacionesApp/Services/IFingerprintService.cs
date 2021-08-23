using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Fingerprint.Abstractions;

namespace VotacionesApp.Services
{
    public interface IFingerprintService
    {
        Task<bool> IsAvailableAsync(
            bool allowAlternativeAuthentication = true);

        Task<FingerprintAuthenticationResult> AuthenticateAsync(
            string reason, CancellationToken cancellationToken = default(CancellationToken));

    }
}
