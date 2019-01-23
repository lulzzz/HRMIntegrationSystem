using System.Security.Cryptography.X509Certificates;

namespace Shared.Interfaces
{
    public interface ICertificateLoader
    {
        X509Certificate2 GetSigningCertificate();
    }
}
