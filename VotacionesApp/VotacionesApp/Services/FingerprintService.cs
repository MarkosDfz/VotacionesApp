using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace VotacionesApp.Services
{
    public class FingerprintService : IFingerprintService
    {
        public async Task<FingerprintAuthenticationResult> AuthenticateAsync(string reason, CancellationToken cancellationToken = default)
        {
            var isAvailable = await CrossFingerprint.Current.IsAvailableAsync(true);

            var result = new FingerprintAuthenticationResult();

            if (!isAvailable)
            {
                result.Status = FingerprintAuthenticationResultStatus.NotAvailable;
                return result;
            }

            var dialogConfig = new AuthenticationRequestConfiguration(reason);

            return await CrossFingerprint.Current.AuthenticateAsync(dialogConfig, cancellationToken);
        }

        public async Task<bool> IsAvailableAsync(bool allowAlternativeAuthentication = false)
        {
            var isAvailable = await CrossFingerprint.Current.IsAvailableAsync(false);

            var result = new FingerprintAuthenticationResult();

            if (!isAvailable)
            {
                result.Status = FingerprintAuthenticationResultStatus.NotAvailable;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
