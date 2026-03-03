using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; // Necesario para [Key] y [Required]

namespace WebAppNomina.Models
{
    public class Usuario
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50)]
        public string username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [StringLength(20)]
        public string rol { get; set; }
    }
}