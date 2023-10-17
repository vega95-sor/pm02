using MauiAppEjercicio1_2.ViewModels;

namespace MauiAppEjercicio1_2.Views;

public partial class EmpleadoPage : ContentPage
{
	public EmpleadoPage(EmpleadoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}