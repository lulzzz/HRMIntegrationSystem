using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;

namespace Altinn.Api.IntegrationTests.ClientTests
{
    [TestFixture]
    public class AltinnClientTests : AltinnTestsBase
    {
        [TestCase("r50258273")]
        public async Task WhenFetchingMessage_ThenListShouldBeReturned(string reporteeId)
        {
            //Act
            var messages = await _altinnClient.GetMessages(reporteeId);
            var messagesList = messages.ToList();

            //Assert
            Assert.IsNotNull(messagesList);
            Assert.AreEqual(2, messagesList.Count);
        }

        [TestCase("r50258273")]
        public async Task WhenFetchingMessage_ThenMessageIdShouldBeReturned(string reporteeId)
        {
            //Act
            var messages = await _altinnClient.GetMessages(reporteeId);

            //Assert
            var firstMessage = messages.First();
            Assert.AreEqual("a6330189", firstMessage.MessageId);

            var secondMessage = messages.Last();
            Assert.AreEqual("a6330269", secondMessage.MessageId);
        }

        [TestCase("r50258273", "a6330189")]
        public async Task WhenFetchingMessageAttachment_ThenListShouldBeReturned(string reporteeId, string messageId)
        {
            //Act
            var messagesAttachments = await _altinnClient.GetAttachments(reporteeId, messageId);
            var messagesAttachmentsList = messagesAttachments.ToList();

            //Assert
            Assert.IsNotNull(messagesAttachmentsList);
            Assert.AreEqual(2, messagesAttachmentsList.Count);
        }

        [TestCase("r50258273", "a6330189")]
        public async Task WhenFetchingMessageAttachment_ThenAttachmentIdShouldBeReturned(string reporteeId, string messageId)
        {
            //Act
            var messagesAttachments = await _altinnClient.GetAttachments(reporteeId, messageId);
           
            //Assert
            var firstAttachment = messagesAttachments.First();
            Assert.AreEqual("3090064", firstAttachment.AttachmentId);
            Assert.AreEqual("sykmelding.pdf", firstAttachment.FileName);

            var secondAttachment = messagesAttachments.Last();
            Assert.AreEqual("3090065", secondAttachment.AttachmentId);
            Assert.AreEqual("sykmelding.xml", secondAttachment.FileName);
        }

        [TestCase("r50258273", "a6330189", "3090065")]
        public async Task WhenFetchingMessageAttachmentData_ThenXMLDataShouldBeReturned(string reporteeId, string messageId, string attachmentId)
        {
            //Act
            var xDocument = await _altinnClient.GetAttachmentData(reporteeId, messageId, attachmentId);

            var isValidXml = TryParseXml(xDocument.ToString(), out xDocument);

            //Assert
            Assert.IsNotNull(xDocument);
            Assert.IsTrue(isValidXml);
        }

        [Test]
        public async Task WhenFetchingReportees_ThenListShouldBeReturned()
        {
            var numberOfReportees = 50;
            //Act
            var reportees = await _altinnClient.GetReportees();

            //Assert
            Assert.NotNull(reportees);
            Assert.AreEqual(numberOfReportees, reportees.Count());
        }

        private bool TryParseXml(string xml, out XDocument xmlDocument)
        {
            try
            {
                var tr = new StringReader(xml);
                var doc = XDocument.Load(tr);
                xmlDocument = doc;
                return true;
            }
            catch
            {
                xmlDocument = null;
                return false;
            }
        }
    }
}
