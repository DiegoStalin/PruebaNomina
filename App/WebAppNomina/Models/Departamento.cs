using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Necesario para [Table]

namespace WebAppNomina.Models
{
    [Table("Departamentoes")] // Mapeo exacto a tu tabla en SQL
    public class Departamento
    {
        [Key]
        public int dept_no { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        public string dept_name { get; set; }

        // Agregamos esto para cumplir con el RF-03 (Desactivar)
        public bool activo { get; set; }
    }
}
