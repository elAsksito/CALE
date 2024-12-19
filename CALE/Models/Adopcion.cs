using System;
using System.Collections.Generic;

namespace CALE.Models;

public partial class Adopcion
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public Guid AnimalId { get; set; }

    public DateTime? FechaAdopcion { get; set; }

    public virtual Animal Animal { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
