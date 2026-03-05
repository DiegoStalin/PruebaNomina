using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppNomina.Models
{
    /// <summary>
    /// Representa la entidad Empleado del sistema de Nómina.
    /// Incluye validaciones de datos y campos requeridos para RNF-02 (Seguridad).
    /// </summary>
    public class Empleado
    {
        /// <summary>
        /// Número identificador único del empleado (Clave Primaria).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int emp_no { get; set; }

        /// <summary>
        /// Cédula de identidad o documento de identificación del empleado.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ci { get; set; }

        /// <summary>
        /// Fecha de nacimiento del empleado para validación de edad.
        /// </summary>
        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime birth_date { get; set; }

        /// <summary>
        /// Nombres del empleado.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string first_name { get; set; }

        /// <summary>
        /// Apellidos del empleado.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string last_name { get; set; }

        /// <summary>
        /// Género del empleado (M/F u otros). Coincide con el DropDownList de la vista.
        /// </summary>
        [Required]
        [StringLength(10)]
        public string genero { get; set; }

        /// <summary>
        /// Fecha en la que el empleado fue contratado. Por defecto es la fecha actual.
        /// </summary>
        [Required]
        public DateTime hire_date { get; set; } = DateTime.Now;

        /// <summary>
        /// Correo electrónico institucional (usado como nombre de usuario para el login).
        /// </summary>
        [Required]
        [EmailAddress]
        public string correo { get; set; }

        /// <summary>
        /// Estado del registro: True (Activo) / False (Inactivo).
        /// Implementa el borrado lógico solicitado en los requerimientos.
        /// </summary>
        [Required]
        public bool activo { get; set; } = true;

        /// <summary>
        /// Contraseña de acceso. Se almacena como un Hash de BCrypt (RNF-02).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string clave { get; set; }

        /// <summary>
        /// Propiedad temporal para capturar mensajes de error de procedimientos almacenados.
        /// </summary>
        [NotMapped]
        public string mensaje { get; set; }

        /// <summary>
        /// Propiedad temporal para capturar el código de retorno de SQL Server.
        /// </summary>
        [NotMapped]
        public int retorno { get; set; }

        /// <summary>
        /// Alias de compatibilidad para el ID del empleado.
        /// </summary>
        [NotMapped]
        public int Id
        {
            get { return emp_no; }
            set { emp_no = value; }
        }
    }
}