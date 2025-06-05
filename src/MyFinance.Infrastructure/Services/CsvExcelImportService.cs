using ClosedXML.Excel;
using CsvHelper;
using DocumentFormat.OpenXml.Spreadsheet;
using MyFinance.Domain.Interfaces;
using MyFinance.Domain.ValueObjects;
using MyFinance.Shared.DTOs;
using System.Globalization;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyFinance.Infrastructure.Services
{
    // Infrastructure/Services/CsvExcelImportService.cs
    public class CsvExcelImportService : ICsvExcelImportService
    {
        //public Task<IList<TransactionDto>> ParseTransactionsAsync(Stream fileStream, string extension)
        //{
        //    var transactions = new List<TransactionDto>();
        //    if (extension.ToLower() == ".csv")
        //    {
        //        //using var reader = new StreamReader(fileStream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
        //        using var reader = new StreamReader(fileStream, Encoding.GetEncoding("Windows-1252"));
        //        string? line;
        //        bool dataStarted = false;

        //        // Formatos de fecha permitidos
        //        string[] formats = {
        //                    "dd/MM/yyyy HH:mm:ss",
        //                    "dd/MM/yyyy H:mm:ss",
        //                    "dd/MM/yyyy",
        //                    "d/M/yyyy HH:mm:ss",
        //                    "d/M/yyyy H:mm:ss",
        //                    "d/M/yyyy H:mm",
        //                    "d/M/yyyy"
        //                };

        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            // Salta filas vacías
        //            if (string.IsNullOrWhiteSpace(line))
        //                continue;

        //            // Detecta la fila de encabezado real
        //            if (!dataStarted)
        //            {
        //                if (line.Contains("Tipo de Transacción") || line.Contains("Monto") || line.Contains("Fecha de operación"))
        //                {
        //                    dataStarted = true;
        //                }
        //                continue;
        //            }

        //            // Procesa solo las filas de datos
        //            var columns = line.Split(';');
        //            if (columns.Length < 6)
        //                continue; // Salta filas incompletas

        //            // Parseo de campos
        //            string tipo = columns[0].Trim().ToString();
        //            string transactionType = tipo.Equals("TE PAGÓ") ? "Income" : "Expense";

        //            decimal amount = 0;
        //            decimal.TryParse(columns[3].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out amount);

        //            string description = string.IsNullOrWhiteSpace(columns[4].ToString()) ? "Importado desde Yape." : columns[4].Trim().ToString();

        //            DateTime date = DateTime.MinValue;
        //            string dateString = columns[5].Trim();
        //            if (!string.IsNullOrWhiteSpace(dateString))
        //            {
        //                DateTime.TryParseExact(
        //                    dateString,
        //                    formats,
        //                    CultureInfo.InvariantCulture,
        //                    DateTimeStyles.None,
        //                    out date
        //                );
        //            }

        //            // Solo agrega si hay datos válidos
        //            if (!string.IsNullOrWhiteSpace(transactionType) && amount > 0 && date != DateTime.MinValue)
        //            {
        //                transactions.Add(new TransactionDto(
        //                    Guid.Empty,
        //                    date,
        //                    transactionType,
        //                    amount,
        //                    description
        //                ));
        //            }
        //        }
        //    }
        //    else if (extension.ToLower() == ".xlsx")
        //    {
        //        using var workbook = new XLWorkbook(fileStream);
        //        var worksheet = workbook.Worksheet(1);
        //        foreach (var row in worksheet.RowsUsed().Skip(2)) // Suponiendo encabezado
        //        {
        //            //para tipo de transaccion Yape
        //            string transactionType = string.Empty;
        //            var cell1 = row.Cell(1);
        //            if (!string.IsNullOrWhiteSpace(cell1.GetString()))
        //            {
        //                transactionType = cell1.GetString().Equals("TE PAGÓ") ? "Income" : "Expense";
        //            }
        //            // Para decimal
        //            decimal amount = 0;
        //            var cell4 = row.Cell(4);
        //            if (!string.IsNullOrWhiteSpace(cell4.GetString()))
        //            {
        //                decimal.TryParse(cell4.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out amount);
        //            }
        //            //para la descripcion
        //            string description = string.Empty;
        //            var cell5 = row.Cell(5);
        //            if (!string.IsNullOrWhiteSpace(cell5.GetString()))
        //            {
        //                description = cell5.GetString();
        //            }
        //            else
        //            {
        //                description = "Importado desde Yape.";
        //            }
        //            // Para DateTime
        //            DateTime date = DateTime.MinValue;
        //            var cell6 = row.Cell(6);
        //            var dateString = cell6.GetString();
        //            if (!string.IsNullOrWhiteSpace(dateString))
        //            {
        //                string[] formats = {
        //                    "dd/MM/yyyy HH:mm:ss",
        //                    "dd/MM/yyyy H:mm:ss",
        //                    "dd/MM/yyyy",
        //                    "d/M/yyyy HH:mm:ss",
        //                    "d/M/yyyy H:mm:ss",
        //                    "d/M/yyyy"
        //                };
        //                DateTime.TryParseExact(
        //                    dateString,
        //                    formats,
        //                    CultureInfo.InvariantCulture,
        //                    DateTimeStyles.None,
        //                    out date
        //                );
        //            }
        //            // Solo agrega si hay datos válidos
        //            if (!string.IsNullOrWhiteSpace(transactionType) && amount > 0 && date != DateTime.MinValue)
        //            {
        //                transactions.Add(new TransactionDto(
        //                    Guid.Empty,
        //                    date,
        //                    transactionType,
        //                    amount,
        //                    description
        //                ));
        //            }
        //        }
        //    }
        //    return Task.FromResult((IList<TransactionDto>)transactions);
        //}

        public Task<IList<TransactionDto>> ParseTransactionsAsync(Stream fileStream, string extension)
        {
            if (extension.ToLower() == ".csv")
                return Task.FromResult(ParseCsv(fileStream));
            else if (extension.ToLower() == ".xlsx")
                return Task.FromResult(ParseXlsx(fileStream));
            else
                throw new NotSupportedException("Formato de archivo no soportado.");
        }
        private IList<TransactionDto> ParseCsv(Stream fileStream)
        {
            var transactions = new List<TransactionDto>();
            using var reader = new StreamReader(fileStream, Encoding.GetEncoding("Windows-1252"));
            string? line;
            bool dataStarted = false;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (!dataStarted)
                {
                    if (line.Contains("Tipo de Transacción") || line.Contains("Monto") || line.Contains("Fecha de operación"))
                        dataStarted = true;
                    continue;
                }

                var columns = line.Split(';');
                if (columns.Length < 6)
                    continue;

                var dto = MapCsvColumnsToDto(columns);
                if (dto != null)
                    transactions.Add(dto);
            }
            return transactions;
        }
        private IList<TransactionDto> ParseXlsx(Stream fileStream)
        {
            var transactions = new List<TransactionDto>();
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet(1);
            foreach (var row in worksheet.RowsUsed().Skip(2))
            {
                var dto = MapXlsxRowToDto(row);
                if (dto != null)
                    transactions.Add(dto);
            }
            return transactions;
        }

        private TransactionDto? MapCsvColumnsToDto(string[] columns)
        {
            string tipo = columns[0].Trim().ToString();
            string transactionType = tipo.Equals("TE PAGÓ") ? "Income" : "Expense";
            decimal amount = ParseDecimal(columns[3]);
            string description = string.IsNullOrWhiteSpace(columns[4].ToString()) ? "Importado desde Yape." : columns[4].Trim();
            DateTime date = ParseDate(columns[5]);

            if (!string.IsNullOrWhiteSpace(transactionType) && amount > 0 && date != DateTime.MinValue)
            {
                return new TransactionDto(Guid.Empty, date, transactionType, amount, description);
            }
            return null;
        }

        private TransactionDto? MapXlsxRowToDto(IXLRow row)
        {
            string tipo = row.Cell(1).GetString().Trim();
            string transactionType = tipo.Equals("TE PAGÓ") ? "Income" : "Expense";
            decimal amount = ParseDecimal(row.Cell(4).GetString());
            string description = string.IsNullOrWhiteSpace(row.Cell(5).GetString()) ? "Importado desde Yape." : row.Cell(5).GetString().Trim();
            DateTime date = ParseDate(row.Cell(6).GetString());

            if (!string.IsNullOrWhiteSpace(transactionType) && amount > 0 && date != DateTime.MinValue)
            {
                return new TransactionDto(Guid.Empty, date, transactionType, amount, description);
            }
            return null;
        }

        private decimal ParseDecimal(string? value)
        {
            decimal.TryParse(value?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
            return result;
        }

        private DateTime ParseDate(string? value)
        {
            string[] formats = {
                "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy H:mm:ss", "dd/MM/yyyy",
                "d/M/yyyy HH:mm:ss", "d/M/yyyy H:mm:ss", "d/M/yyyy H:mm", "d/M/yyyy"
            };
            DateTime.TryParseExact(value?.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
            return date;
        }
    }

}