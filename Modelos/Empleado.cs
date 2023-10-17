using System.ComponentModel.DataAnnotations;

namespace MauiAppEjercicio1_2.Modelos
{
    public class Empleado
    {
        [Key] 
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo {  get; set; }
    }
}
