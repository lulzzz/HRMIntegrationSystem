using Microsoft.Extensions.Configuration;
using Shared.Interfaces;
using Shared.Services.Extensions;
using Sticos.Utilities.Security;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Shared.Services.Services
{
    public class CertificateLoader : ICertificateLoader
    {
        public const string CertificateLoadMethod_Key = "Authentication:CertificateLoadMethod";
        public const string SigningCertificateThumbprint_Key = "Authentication:SigningCertificateThumbprint";
        public const string SigningCertificateFile_Key = "Authentication:SigningCertificateFile";
        public const string SigningStoreName_Key = "Authentication:SigningCertificateStoreName";
        public const string SigningStoreLocation_Key = "Authentication:SigningCertificateStoreLocation";

        private readonly IConfiguration _configuration;
        readonly ConcurrentDictionary<string, X509Certificate2> _certCache = new ConcurrentDictionary<string, X509Certificate2>();

        public CertificateLoader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public X509Certificate2 GetSigningCertificate()
        {
            var loadMethod = _configuration.GetValue<CertificateLoadMethod>(CertificateLoadMethod_Key);
            switch (loadMethod)
            {
                case CertificateLoadMethod.Thumbprint:
                    return LoadFromThumbprint();
                case CertificateLoadMethod.File:
                    return LoadFromFile();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private X509Certificate2 LoadFromFile()
        {
            var filename = _configuration.GetValueNotNull<string>(SigningCertificateFile_Key);

            if (!File.Exists(filename))
            {
                throw new ApplicationException($"Can not find certificate. File: {filename}");
            }

            try
            {
                return new X509Certificate2(filename);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Can not find create certificate from file: {filename}", e);
            }
        }

        private X509Certificate2 LoadFromThumbprint()
        {
            var thumbprint = _configuration.GetValueNotNull<string>(SigningCertificateThumbprint_Key);

            if (!_certCache.TryGetValue(thumbprint, out X509Certificate2 signingCertificate))
            {
                var store = _configuration.GetValueNotNull<StoreName>(SigningStoreName_Key);
                var location = _configuration.GetValueNotNull<StoreLocation>(SigningStoreLocation_Key);
                signingCertificate = CertificateUtil.GetCertificateByThumbprint(store, location, thumbprint);
                if (signingCertificate == null)
                {
                    throw new ApplicationException($"Can not find sertificate. Store={store} Location={location} Thumbprint={thumbprint}");
                }

                _certCache.TryAdd(thumbprint, signingCertificate);
            }

            return signingCertificate;
        }

    }
    public enum CertificateLoadMethod
    {
        Thumbprint,
        File,
    }
}
