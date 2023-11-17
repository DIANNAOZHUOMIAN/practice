using Demo.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Demo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        private string _title = "练习区域/导航/传参等";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public DelegateCommand LoadedCommand
        {
            get => new DelegateCommand(() =>
            {
                regionManager.Regions["ContentRegion"].NavigationService.RequestNavigate("List");
            });
        }
    }
}
