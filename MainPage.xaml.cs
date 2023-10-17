using MauiAppEjercicio1_2.ViewModels;

namespace MauiAppEjercicio1_2
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

    }
}