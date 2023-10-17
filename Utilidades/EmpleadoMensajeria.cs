using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiAppEjercicio1_2.Utilidades
{
    public class EmpleadoMensajeria : ValueChangedMessage<EmpleadoMensaje>
    {
        public EmpleadoMensajeria(EmpleadoMensaje value) : base(value)
        {

        }
    }
}
