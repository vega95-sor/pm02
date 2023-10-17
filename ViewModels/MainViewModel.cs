using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;

using Microsoft.EntityFrameworkCore;
using MauiAppEjercicio1_2.DataAccess;
using MauiAppEjercicio1_2.DTOs;
using MauiAppEjercicio1_2.Utilidades;
using MauiAppEjercicio1_2.Modelos;
using System.Collections.ObjectModel;
using MauiAppEjercicio1_2.Views;

namespace MauiAppEjercicio1_2.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly EmpleadoDbContext _dbContext;
        [ObservableProperty]
        private ObservableCollection<EmpleadoDTO> listaEmpleado = new ObservableCollection<EmpleadoDTO>();

        public MainViewModel(EmpleadoDbContext context) 
        {
            _dbContext = context;

            MainThread.BeginInvokeOnMainThread(new Action(async () => await Obtener()));

            WeakReferenceMessenger.Default.Register<EmpleadoMensajeria>(this, (r,m) => 
            {
                EmpleadoMensajeRecibido(m.Value);
            });

        }

        public async Task Obtener()
        {
            var lista = await _dbContext.Empleados.ToListAsync();
            if (lista.Any())
            {
                foreach(var item in lista)
                {
                    ListaEmpleado.Add(new EmpleadoDTO 
                    {
                        IdEmpleado = item.IdEmpleado,
                        Nombre = item.Nombre,
                        Apellido = item.Apellido,
                        FechaNacimiento = item.FechaNacimiento,
                        Correo = item.Correo,
                    });
                }
            }
        }

        private void EmpleadoMensajeRecibido(EmpleadoMensaje empleadoMensaje)
        {
            var empleadoDto = empleadoMensaje.EmpleadoDto;

            if (empleadoMensaje.EsCrear)
            {
                ListaEmpleado.Add(empleadoDto);
            }
            else
            {
                var encontrado = ListaEmpleado
                    .First(e => e.IdEmpleado == empleadoDto.IdEmpleado);

                encontrado.Nombre = empleadoDto.Nombre;
                encontrado.Apellido = empleadoDto.Apellido;
                encontrado.FechaNacimiento = empleadoDto.FechaNacimiento;
                encontrado.Correo = empleadoDto.Correo;

            }

        }

        [RelayCommand]
        private async Task Crear()
        {
            var uri = $"{nameof(EmpleadoPage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Editar(EmpleadoDTO empleadoDto)
        {
            var uri = $"{nameof(EmpleadoPage)}?id={empleadoDto.IdEmpleado}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Eliminar(EmpleadoDTO empleadoDto)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "Desea eliminar el empleado?", "Si", "No");

            if (answer)
            {
                var encontrado = await _dbContext.Empleados
                    .FirstAsync(e => e.IdEmpleado == empleadoDto.IdEmpleado);

                _dbContext.Empleados.Remove(encontrado);
                await _dbContext.SaveChangesAsync();
                ListaEmpleado.Remove(empleadoDto);

            }

        }
    }
}
