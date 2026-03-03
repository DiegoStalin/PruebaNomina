using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppNomina.Models
{
    [Table("dept_emp")] // Mapeo exacto a la tabla que creamos en SQL
    public class AsignacionDepto
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un empleado")]
        public int emp_no { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un departamento")]
        public int dept_no { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [Display(Name = "Fecha Desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime from_date { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [Display(Name = "Fecha Hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime to_date { get; set; }

        // --- PROPIEDADES DE NAVEGACIÓN (Corrigen CS1061) ---
        // Estas permiten que el Index muestre Nombres en lugar de IDs

        [ForeignKey("emp_no")]
        public virtual Empleado Empleado { get; set; }

        [ForeignKey("dept_no")]
        public virtual Departamento Departamento { get; set; }
    }
}