using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ViewModels.UserControls
{
    public class DeletDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "警告";
        private string subTitle="是否确认删除？";
        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value;RaisePropertyChanged(); }
        }


        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
        public DelegateCommand ConfirmCommand { get => new DelegateCommand(() => {
            RequestClose.Invoke(new DialogResult(ButtonResult.OK));
        
        }); }
        public DelegateCommand CancelCommand { get => new DelegateCommand(() => {
            RequestClose.Invoke(new DialogResult(ButtonResult.Cancel));
        }); }
    }
}
