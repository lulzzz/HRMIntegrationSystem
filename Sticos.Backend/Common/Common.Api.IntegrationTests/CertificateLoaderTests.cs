using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Shared.Services.Services;
using TestCommon.Builders;

namespace Common.Api.IntegrationTests
{
    [TestFixture]
    public class CertificateLoaderTests
    {
        [Ignore("only works if sticos-signing certificate is in My Store")]
        [Test]
        public async Task CertificateLoader_ByThumbPrint_Test()
        {
            var thumbPrint = "383530e308a9dedad33956bc753b802194076e65";
         
            var configSettings = new Dictionary<string, string>();
            configSettings.Add(CertificateLoader.SigningCertificateThumbprint_Key, thumbPrint);
            configSettings.Add(CertificateLoader.CertificateLoadMethod_Key, CertificateLoadMethod.Thumbprint.ToString());
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configSettings)
                .Build();
            var loader = new CertificateLoader(configuration);
            var certificate = loader.GetSigningCertificate();

            Assert.IsNotNull(certificate);
            Assert.AreEqual(thumbPrint.ToUpper(), certificate.Thumbprint.ToUpper());
        }

        [Test]
        public async Task CertificateLoader_ByFile_Test()
        {
            var filename = "signingcertificate.pfx";
            var thumbprint = "5A1BFE91FC8B93BD3A2491F99D4D4396D0017AE9";
         
            var configSettings = new Dictionary<string, string>();
            configSettings.Add(CertificateLoader.SigningCertificateFile_Key, filename);
            configSettings.Add(CertificateLoader.CertificateLoadMethod_Key, CertificateLoadMethod.File.ToString());
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configSettings)
                .Build();
            var loader = new CertificateLoader(configuration);
            var certificate = loader.GetSigningCertificate();

            Assert.IsNotNull(certificate);
            Assert.AreEqual(thumbprint.ToUpper(), certificate.Thumbprint.ToUpper());
        }
    }
}
