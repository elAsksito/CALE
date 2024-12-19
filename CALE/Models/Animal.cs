using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CALE.Models
{
    public partial class Animal
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La especie es obligatoria.")]
        [StringLength(50, ErrorMessage = "La especie no puede tener más de 50 caracteres.")]
        public string Especie { get; set; } = null!;

        [Required(ErrorMessage = "El raza es obligatoria.")]
        [StringLength(50, ErrorMessage = "La raza no puede tener más de 50 caracteres.")]
        public string? Raza { get; set; }

        [Required(ErrorMessage = "El edad es obligatoria.")]
        [Range(0, 15, ErrorMessage = "La edad debe estar entre 0 y 15 años.")]
        public int? Edad { get; set; }

        [Required(ErrorMessage = "El sexo es obligatorio.")]
        [StringLength(20, ErrorMessage = "El sexo no puede tener más de 20 caracteres.")]
        public string Sexo { get; set; } = null!;

        public string? Estado { get; set; }

        public string? ImageUrl { get; set; }

        public Guid DuenoId { get; set; }

        public virtual ICollection<Adopcion> Adopcions { get; set; } = new List<Adopcion>();

        public virtual Usuario Dueno { get; set; } = null!;
    }
}
