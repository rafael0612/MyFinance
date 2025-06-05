using CsvHelper.Configuration;
using MyFinance.Shared.DTOs;

namespace MyFinance.Infrastructure.Services
{
    public sealed class TransactionDtoMap : ClassMap<TransactionDto>
    {
        public TransactionDtoMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.Date).Name("Fecha"); // Si tu CSV tiene "Fecha" en vez de "Date"
            Map(m => m.TransactionType).Name("Tipo"); // Si tu CSV tiene "Tipo" en vez de "TransactionType"
            Map(m => m.Amount).Name("Monto"); // Si tu CSV tiene "Monto" en vez de "Amount"
            Map(m => m.Description).Name("Descripcion"); // Si tu CSV tiene "Descripcion" en vez de "Description"
        }
    }
}
