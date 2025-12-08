namespace MyFinance.Domain.ValueObjects
{
    public class Categoria
    {
        public string Nombre { get; private set; } // Nombre de la categoría (e.g., Alimentación, Transporte)
        public string NivelNecesidad { get; private set; } // Esencial o NoEsencial
        public string NaturalezaGasto { get; private set; } // Consumo o Financiero
        public bool EsFijo { get; private set; } // Indica si el gasto es fijo o variable

        public Categoria(string nombre, string nivelNecesidad = "Esencial", string naturalezaGasto = "Consumo", bool esFijo = false)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la categoría no puede estar vacío.", nameof(nombre));

            Nombre = nombre;
            NivelNecesidad = nivelNecesidad;
            NaturalezaGasto = naturalezaGasto;
            EsFijo = esFijo;
        }
    }
}