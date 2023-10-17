using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiAppEjercicio1_2.DTOs
{
    public partial class EmpleadoDTO : ObservableObject
    {
        [ObservableProperty]
        public int idEmpleado;
        [ObservableProperty]
        public string nombre;
        [ObservableProperty]
        public string apellido;
        [ObservableProperty]
        public DateTime fechaNacimiento;
        [ObservableProperty]
        public string correo;
    }
}
