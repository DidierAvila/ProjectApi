using System.ComponentModel;

namespace Domain.Enums
{
    public enum Category
    {
        [Description("Futbol")]
        Futbol,
        [Description("Politica")]
        Politica,
        [Description("Farandula")]
        Farandula,
        [Description("Tecnologia")]
        Tecnologia,
        [Description("Ciencia")]
        Ciencia,
        [Description("Salud")]
        Salud,
        [Description("Economia")]
        Economia,
        [Description("Moda")]
        Moda,
        [Description("Otro")]
        Otro
    }
}
