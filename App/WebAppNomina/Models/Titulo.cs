using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppNomina.Models
{
    [Table("titles")] // Nombre exacto en el MER
    public class Titulo
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Seleccione un empleado")]
        public int emp_no { get; set; }

        [Required(ErrorMessage = "El nombre del título/cargo es obligatorio")]
        [StringLength(50)]
        public string title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Inicio")]
        public DateTime from_date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Fin")]
        public DateTime? to_date { get; set; }

        [ForeignKey("emp_no")]
        public virtual Empleado Empleado { get; set; }
    }
}