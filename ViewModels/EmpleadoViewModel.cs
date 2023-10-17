
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;

using Microsoft.EntityFrameworkCore;
using MauiAppEjercicio1_2.DataAccess;
using MauiAppEjercicio1_2.DTOs;
using MauiAppEjercicio1_2.Utilidades;
using MauiAppEjercicio1_2.Modelos;

namespace MauiAppEjercicio1_2.ViewModels
{
    public partial class EmpleadoViewModel : ObservableObject, IQueryAttributable
    {
        private readonly EmpleadoDbContext _dbContext;

        [ObservableProperty]
        private EmpleadoDTO empleadoDto = new EmpleadoDTO();

        [ObservableProperty]
        private string tituloPagina;

        private int IdEmpleado;

        [ObservableProperty]
        private bool loadingEsVisible = false;

        public EmpleadoViewModel(EmpleadoDbContext context)
        {
            _dbContext = context;
            EmpleadoDto.FechaNacimiento = DateTime.Now;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query) 
        {
            var id = int.Parse(query["id"].ToString());
            IdEmpleado = id;

            if (IdEmpleado == 0)
            {
                TituloPagina = "Nuevo Empleado";
            }
            else
            {
                TituloPagina = "Editar Empleado";
                LoadingEsVisible = true;
                await Task.Run(async () =>
                {
                    var encontrado = await _dbContext.Empleados.FirstAsync(e => e.IdEmpleado == IdEmpleado);
                    EmpleadoDto.IdEmpleado = encontrado.IdEmpleado;
                    EmpleadoDto.Nombre = encontrado.Nombre;
                    EmpleadoDto.Apellido = encontrado.Apellido;
                    EmpleadoDto.FechaNacimiento = encontrado.FechaNacimiento;
                    EmpleadoDto.Correo = encontrado.Correo;

                    MainThread.BeginInvokeOnMainThread(() => { LoadingEsVisible = false; });
                });
            }
        }

        [RelayCommand]
        private async Task Agregar_Informacion()
        {
            LoadingEsVisible = true;
            EmpleadoMensaje mensaje = new EmpleadoMensaje();

            await Task.Run(async () =>
            {
                if (IdEmpleado == 0)
                {
                    var tbEmpleado = new Empleado
                    {
                        Nombre = EmpleadoDto.Nombre,
                        Apellido = EmpleadoDto.Apellido,
                        FechaNacimiento = EmpleadoDto.FechaNacimiento,
                        Correo = EmpleadoDto.Correo,
                    };

                    _dbContext.Empleados.Add(tbEmpleado);
                    await _dbContext.SaveChangesAsync();

                    EmpleadoDto.IdEmpleado = tbEmpleado.IdEmpleado;
                    mensaje = new EmpleadoMensaje()
                    {
                        EsCrear = true,
                        EmpleadoDto = EmpleadoDto
                    };

                }
                else
                {
                    var encontrado = await _dbContext.Empleados.FirstAsync(e => e.IdEmpleado == IdEmpleado);
                    encontrado.Nombre = EmpleadoDto.Nombre;
                    encontrado.Apellido = EmpleadoDto.Apellido;
                    encontrado.FechaNacimiento = EmpleadoDto.FechaNacimiento;
                    encontrado.Correo = EmpleadoDto.Correo;

                    await _dbContext.SaveChangesAsync();

                    mensaje = new EmpleadoMensaje()
                    {
                        EsCrear = false,
                        EmpleadoDto = EmpleadoDto
                    };

                }

                MainThread.BeginInvokeOnMainThread(async () => 
                {
                    LoadingEsVisible = false;
                    WeakReferenceMessenger.Default.Send(new EmpleadoMensajeria(mensaje));
                    await Shell.Current.Navigation.PopAsync();
                });
            });
        }

    }
}
