// MyFinance.Client/Models/TransactionDto.cs
namespace MyFinance.Client.Models
{
    using System;

    /// <summary>
    /// Represents a financial transaction.
    /// </summary>
    /// <remarks>
    /// This record is used to transfer transaction data between the client and server.
    /// </remarks>
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string TransactionType { get; set; } = "Income";
        public decimal Amount { get; set; }
        public string TipoIngreso { get; set; } = "Activo"; // Para ingresos
        public string OrigenIngreso { get; set; } = "Es Gasto"; // Para ingresos
        public string? Description { get; set; }

        // Nuevos campos para gastos
        public string ExpenseCategory { get; set; } = string.Empty;
        public bool EsFijo { get; set; } = false;
        public string NaturalezaGasto { get; set; } = "Consumo";
        public string NivelNecesidad { get; set; } = "Esencial";

        public TransactionDto() { }

        public TransactionDto(
            Guid id,
            DateTime date,
            string transactionType,
            decimal amount,
            string tipoIngreso,
            string origenIngreso,
            string description,
            string expenseCategory,
            bool esFijo,
            string naturalezaGasto,
            string nivelNecesidad)
        {
            Id = id;
            Date = date;
            TransactionType = transactionType;
            Amount = amount;
            TipoIngreso = tipoIngreso;
            OrigenIngreso = origenIngreso;
            Description = description;
            ExpenseCategory = expenseCategory;
            EsFijo = esFijo;
            NaturalezaGasto = naturalezaGasto;
            NivelNecesidad = nivelNecesidad;
        }
    }
}