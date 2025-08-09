using System.ComponentModel;

namespace Domain.Enums
{
    public enum EntityStatus
    {
        [Description("Activo")]
        Active = 1,
        [Description("Cancelado")]
        Cancelled = 2,
        [Description("Inactivo")]
        Inactive = 3
    }
}