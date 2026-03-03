using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Necesario para [Key] y [Display]
using System.ComponentModel.DataAnnotations.Schema; // Necesario para [Table] y [ForeignKey]

namespace WebAppNomina.Models
{
    // Asegura que coincida con la tabla donde el Trigger inserta los datos
    [Table("LogAuditoriaSalarios")]
    public class LogAuditoriaSalario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Display(Name = "Usuario Responsable")]
        public string usuario { get; set; }

        [Display(Name = "Fecha y Hora")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime fechaActualizacion { get; set; }

        [Display(Name = "Descripción del Cambio")]
        public string DetalleCambio { get; set; }

        [Display(Name = "Monto")]
        [DataType(DataType.Currency)] // Ayuda a mostrar el símbolo de moneda (€) en la vista
        public long salario { get; set; }

        [Display(Name = "Código Emp.")]
        public int emp_no { get; set; }

        // Relación con Empleado para poder mostrar nombres en los reportes
        [ForeignKey("emp_no")]
        public virtual Empleado Empleado { get; set; }
    }
}