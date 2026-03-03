using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppNomina.Models
{
    public class Empleado
    {
        [Key]
        public int emp_no { get; set; }

        [Required]
        [StringLength(20)]
        public string ci { get; set; }

        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime birth_date { get; set; }

        [Required]
        [StringLength(50)]
        public string first_name { get; set; }

        [Required]
        [StringLength(50)]
        public string last_name { get; set; }

        // CAMBIO: Se renombra a 'genero' para coincidir con SQL y la Vista
        // Se usa string para facilitar el manejo en los DropDownList
        [Required]
        [StringLength(10)]
        public string genero { get; set; }

        [Required]
        public DateTime hire_date { get; set; } = DateTime.Now;

        [Required]
        [EmailAddress]
        public string correo { get; set; }

        // CAMPO OBLIGATORIO PARA RF-02 (Borrado lógico)
        [Required]
        public bool activo { get; set; } = true;

        // CAMBIO: SE QUITA [NotMapped] para que se guarde en la BD
        [Required]
        [StringLength(100)]
        public string clave { get; set; }

        // Estas propiedades sí pueden seguir como NotMapped si solo se usan para mensajes temporales
        [NotMapped]
        public string mensaje { get; set; }

        [NotMapped]
        public int retorno { get; set; }

        [NotMapped]
        public int Id
        {
            get { return emp_no; }
            set { emp_no = value; }
        }
    }
}