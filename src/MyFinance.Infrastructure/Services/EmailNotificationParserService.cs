using MyFinance.Domain.Interfaces;
using MyFinance.Shared.DTOs;
using Microsoft.Extensions.Options;
using MyFinance.Shared.Config;
using System.Text.RegularExpressions;

namespace MyFinance.Infrastructure.Services
{
    public class EmailNotificationParserService : IEmailNotificationParserService
    {
        private readonly EmailImapSettings _settings;
        public EmailNotificationParserService(IOptions<EmailImapSettings> options)
        {
            _settings = options.Value;
        }
        public async Task<IList<ParsedEmailTransactionDto>> ParseNewTransactionsAsync()
        {
            var parsedList = new List<ParsedEmailTransactionDto>();
            using var client = new MailKit.Net.Imap.ImapClient();
            await client.ConnectAsync(_settings.ImapServer, _settings.ImapPort, _settings.UseSsl);
            await client.AuthenticateAsync(_settings.Email, _settings.Password);
            var inbox = client.Inbox;
            await inbox.OpenAsync(MailKit.FolderAccess.ReadWrite);
            // Buscar emails NO leídos (Unread) de Yape/Plin
            var uids = inbox.Search(MailKit.Search.SearchQuery.NotSeen);

            foreach (var uid in uids)
            {
                var message = await inbox.GetMessageAsync(uid);

                // Aquí puedes filtrar por remitente/asunto
                if (message.From.ToString().Contains("yape") || message.From.ToString().Contains("plin"))
                {
                    // Aquí va el parsing real
                    var parsed = ParseTransactionFromEmail(message.TextBody, message.Date.UtcDateTime);
                    if (parsed != null)
                    {
                        parsed.RawEmailId = uid.ToString();
                        parsed.Sender = message.From.ToString();
                        parsedList.Add(parsed);
                    }
                    // Opcional: Marcar como leído o mover a otra carpeta
                    // await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                }
            }

            await client.DisconnectAsync(true);
            return parsedList;
        }
        private ParsedEmailTransactionDto? ParseTransactionFromEmail(string text, DateTime date)
        {
            // Aquí va la lógica de extracción según el formato del email
            // Por ejemplo, usando Regex para encontrar monto y descripción

            var match = Regex.Match(text, @"Monto: S\.\s*(\d+(\.\d{1,2})?)");
            if (!match.Success) return null;

            var amount = decimal.Parse(match.Groups[1].Value);
            return new ParsedEmailTransactionDto
            {
                Date = date,
                Amount = amount,
                Description = text // O una mejor extracción según tu email
            };
        }
    }
}
