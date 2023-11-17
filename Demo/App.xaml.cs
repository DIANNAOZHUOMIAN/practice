using Demo.ViewModels.UserControls;
using Demo.Views;
using Demo.Views.UserControls;
using Prism.Ioc;
using System.Windows;

namespace Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<List>();
            containerRegistry.RegisterForNavigation<Add>();
            containerRegistry.RegisterForNavigation<Edit>();
            containerRegistry.RegisterDialog<Edit, EditViewModel>();
            containerRegistry.RegisterDialog<DeletDialog, DeletDialogViewModel>();
        }
    }
}
