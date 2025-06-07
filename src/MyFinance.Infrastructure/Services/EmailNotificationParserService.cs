using MyFinance.Domain.Interfaces;
using MyFinance.Shared.DTOs;
using Microsoft.Extensions.Options;
using MyFinance.Shared.Config;
using System.Text.RegularExpressions;
using MailKit;
using System.Globalization;

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

            //Configura el timeout a 5 minutos (300000 ms)
            client.Timeout = 300000; // 5 minutos en milisegundos
            await client.ConnectAsync(_settings.ImapServer, _settings.ImapPort, _settings.UseSsl);
            await client.AuthenticateAsync(_settings.Email, _settings.Password);
            // Abre la bandeja de entrada
            var inbox = client.Inbox;
            await inbox.OpenAsync(MailKit.FolderAccess.ReadWrite);
            // Buscar emails NO leídos (Unread) de Yape/Plin
            // Filtra emails no leídos con posible asunto de Interbank
            var uids = inbox.Search(MailKit.Search.SearchQuery.NotSeen);// TO-DO: cambiar a Seen/NotSeen para leer solo los no leídos

            foreach (var uid in uids)
            {
                var message = await inbox.GetMessageAsync(uid);

                //aqui filtrar correos por asunto
                if (message.Subject != null && message.Subject.ToUpper().Contains("COMPARTIR MOVIMIENTOS DE MI CUENTA INTERBANK"))
                {
                    var body = message.TextBody ?? message.HtmlBody ?? "";
                    var extraidas = ParseMovimientosInterbank(body);

                    parsedList.AddRange(extraidas);

                    // Opcional: Marcar como leído
                    await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                }
            }

            await client.DisconnectAsync(true);
            return parsedList;
        }
        // El método de parsing específico
        public List<ParsedEmailTransactionDto> ParseMovimientosInterbank(string emailBody)
        {
            var movimientos = new List<ParsedEmailTransactionDto>();

            // Encuentra la sección de MOVIMIENTOS
            var movPattern = @"MOVIMIENTOS:(?<tabla>.+)";
            var matchMov = Regex.Match(emailBody, movPattern, RegexOptions.Singleline);
            if (!matchMov.Success) return movimientos;

            var tabla = matchMov.Groups["tabla"].Value.Trim();

            // Extrae las filas de la tabla
            var filaPattern = @"<tr.*?>(.*?)</tr>";
            var filas = Regex.Matches(tabla, filaPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            //for (int i = startIndex; i < lineas.Count; i++)
            foreach (Match fila in filas)
            {
                //var linea = lineas[i];
                var filaHtml = fila.Groups[1].Value;

                // Extrae las celdas de la fila
                var celdaPattern = @"<td.*?>(.*?)</td>";
                var celdas = Regex.Matches(filaHtml, celdaPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                if (celdas.Count != 3) continue; // Saltar filas no válidas

                var cargo = ExtraerTextoPlano(celdas[0].Groups[1].Value);
                var fechaHora = ExtraerTextoPlano(celdas[1].Groups[1].Value);
                var montoStr = ExtraerTextoPlano(celdas[2].Groups[1].Value);

                // Extrae monto
                var montoMatch = Regex.Match(montoStr, @"S/\s*([+-]?[\d,\.]+)");
                if (!montoMatch.Success) continue;

                var montoClean = montoMatch.Groups[1].Value.Replace(",", "");
                if (!decimal.TryParse(montoClean, NumberStyles.Any, CultureInfo.InvariantCulture, out var monto))
                    continue;

                // Extrae fecha y hora
                DateTime fecha;
                if (!TryParseFechaHora(fechaHora, out fecha))
                    fecha = DateTime.Now; // Si no se puede parsear, usar fecha actual                    

                movimientos.Add(new ParsedEmailTransactionDto
                {
                    Date = fecha,
                    Amount = monto,
                    Description = "Interbank",//cargo,
                    Sender = "Interbank"
                });
            }
            return movimientos;
        }

        // Método auxiliar para parsear la fecha y hora
        public static bool TryParseFechaHora(string texto, out DateTime resultado)
        {
            resultado = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            // Detecta si ya hay año, si no hay, inserta el año actual después del mes
            var regex = new Regex(@"^(?<diaSemana>\w{3,}\.\s)?(?<dia>\d{1,2}) (?<mes>\w{3}) (?<hora>\d{2}:\d{2})$");
            var match = regex.Match(texto.Trim());

            string fechaFormateada;
            if (match.Success)
            {
                // Ejemplo: "dom. 11 may 18:53" -> "11 may 2025 18:53"
                var dia = match.Groups["dia"].Value;
                var mes = match.Groups["mes"].Value.ToLower();
                var hora = match.Groups["hora"].Value;
                var año = DateTime.Now.Year;
                fechaFormateada = $"{dia} {mes} {año} {hora}";
            }
            else
            {
                // Usa el texto tal cual, por si ya incluye el año
                fechaFormateada = texto.Trim();
            }            
            // Formatos válidos
            var formatos = new[] {
                "dd MMM yyyy HH:mm",
                "d MMM yyyy HH:mm",
                "dd MMM yyyy H:mm",
                "d MMM yyyy H:mm"
            };
            if (DateTime.TryParseExact(fechaFormateada, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out resultado))
            {
                return true;
            }
            return false;
        }
        // Método auxiliar para limpiar HTML y extraer solo el texto plano
        private static string ExtraerTextoPlano(string html)
        {
            // Extrae el texto dentro de <span> si existe, si no, quita etiquetas HTML
            var spanMatch = Regex.Match(html, @"<span.*?>(.*?)</span>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var text = spanMatch.Success ? spanMatch.Groups[1].Value : html;
            // Quita cualquier etiqueta HTML restante
            return Regex.Replace(text, "<.*?>", "").Trim();
        }
    }
}