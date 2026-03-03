using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Necesario para [Table]

namespace WebAppNomina.Models
{
    [Table("Usuarios")] // Mapeo exacto a tu tabla en SQL
    public class Usuario
    {
        [Key]
        public int emp_no { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        public string clave { get; set; }

        public string rol { get; set; }
    }
}