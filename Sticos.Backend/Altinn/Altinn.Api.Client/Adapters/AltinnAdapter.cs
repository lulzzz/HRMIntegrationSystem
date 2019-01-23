using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

using Altinn.Api.Client.HttpClients;
using Altinn.Api.Domain.Schemas;
using Altinn.Api.Domain.Entities;
using Altinn.Api.Domain.Interfaces;

namespace Altinn.Api.Client.Adapters
{
    public class AltinnAdapter : IAltinnAdapter
    {
        private readonly IAltinnClient _altinnClient;
        private readonly IXmlSerializer _xmlSerializer;
        private readonly string Sykmelding_Namespace = "http://nav.no/melding/virksomhet/sykmeldingArbeidsgiver/v1/sykmeldingArbeidsgiver";
        private ILogger<AltinnAdapter> _logger { get; set; }

        public AltinnAdapter(IAltinnClient altinnClient, IXmlSerializer xmlSerializer, ILogger<AltinnAdapter> logger)
        {
            _altinnClient = altinnClient;
            _xmlSerializer = xmlSerializer;
            _logger = logger;
        }

        public async Task<IEnumerable<NavMessage>> ReadMessages(string externalCompanyId)
        {
            var navMessagesList = new List<NavMessage>();

            try
            {
                var messages = await _altinnClient.GetMessages(externalCompanyId);

                foreach (var message in messages)
                {
                    var messageId = message.MessageId;
                    var attachments = await _altinnClient.GetAttachments(externalCompanyId, messageId);

                    foreach (var attachment in attachments)
                    {
                        var xmlAttachment = await _altinnClient.GetAttachmentData(externalCompanyId, messageId, attachment.AttachmentId);

                        if (xmlAttachment != null)
                        {
                            var elementNamespace = xmlAttachment?.Root?.Name?.NamespaceName;

                            if (!string.Equals(elementNamespace, Sykmelding_Namespace))
                            {
                                _logger.LogInformation($"Message will be skipped.Namespace: {elementNamespace}");
                            }
                            else
                            {
                                var xmlMessage = _xmlSerializer.Deserialize<SykmeldingArbeidsgiver>(xmlAttachment.ToString());

                                var navMessage = new NavMessage
                                {
                                    Namespace = elementNamespace,
                                    MessageXml = xmlAttachment.ToString(),
                                    IntegrationType = IntegrationType.Import,
                                    WorkState = WorkState.ReadyForProcessing,
                                    ExternalId = xmlMessage.sykmeldingId,
                                    ReporteeId = externalCompanyId,
                                    MesssageId = messageId,
                                    AttachmentId = attachment.AttachmentId,
                                    BusinessOrganizationNumber = xmlMessage.virksomhetsnummer,
                                };
                                navMessagesList.Add(navMessage);
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Attachment is not XML file");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error connecting to Altinn", ex);
            }
            return navMessagesList;
        }

        public async Task<IEnumerable<NavMessage>> WriteMessages(string externalCompanyId, IEnumerable<NavMessage> navMessages)
        {
            throw new NotImplementedException();
        }
    }
}
