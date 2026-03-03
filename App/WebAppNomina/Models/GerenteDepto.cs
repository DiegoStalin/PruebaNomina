using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppNomina.Models
{
    [Table("dept_manager")] // Nombre exacto de la tabla en el MER
    public class GerenteDepto
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Seleccione el empleado que será manager")]
        public int emp_no { get; set; }

        [Required(ErrorMessage = "Seleccione el departamento")]
        public int dept_no { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Inicio")]
        public DateTime from_date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Fin")]
        public DateTime to_date { get; set; }

        // Propiedades de navegación para mostrar nombres
        [ForeignKey("emp_no")]
        public virtual Empleado Empleado { get; set; }

        [ForeignKey("dept_no")]
        public virtual Departamento Departamento { get; set; }
    }
}