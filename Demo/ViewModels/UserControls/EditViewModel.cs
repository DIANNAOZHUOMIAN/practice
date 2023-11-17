using Demo.Helpers;
using Demo.Models;
using DryIoc;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Demo.ViewModels.UserControls
{
    // INavigationAware
    public class EditViewModel : BindableBase, IConfirmNavigationRequest,IDialogAware
    {
        private readonly IRegionManager regionManager;
        public EditViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(); }
        }

        private UserInfo _editUser;

        public UserInfo EditUser
        {
            get { return _editUser; }
            set { _editUser = value; RaisePropertyChanged(); }
        }

        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; RaisePropertyChanged(); }
        }

        private BitmapImage _userLogo;

        public event Action<IDialogResult> RequestClose;

        public BitmapImage UserLogo
        {
            get { return _userLogo; }
            set { _userLogo = value; RaisePropertyChanged(); }
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Id"))
            {
                Id = navigationContext.Parameters.GetValue<string>("Id");

                using (var db = new Db002Entities())
                {
                    EditUser = db.UserInfo.Where(new Func<UserInfo, bool>((user) => user.Id == Convert.ToInt32(Id))).FirstOrDefault();

                    if (EditUser != null)
                    {
                        // 把byte[]字节数组（数据库中二进制）转换成Image对象或BitmapImage
                        if (EditUser.Logo != null)
                        {
                            UserLogo = ImageHelper.ByteToImg(EditUser.Logo);
                        }
                    }
                }
            }
        }

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

        public DelegateCommand SelectImgCommand
        {
            get => new DelegateCommand(() =>
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                bool? result = openFileDialog.ShowDialog();
                if (result == true)
                {
                    ImagePath = openFileDialog.FileName;
                    byte[] imgByte = ImageHelper.ImgPathToByte(ImagePath);
                    UserLogo = ImageHelper.ByteToImg(imgByte);
                }
            });
        }

        public DelegateCommand SaveCommand
        {
            get => new DelegateCommand(() =>
            {
                using (var db = new Db002Entities())
                {
                    if (EditUser != null)
                    {
                        if (!string.IsNullOrEmpty(ImagePath)) {
                            EditUser.Logo = ImageHelper.ImgPathToByte(ImagePath);
                        }

                        db.UserInfo.Attach(EditUser);
                        db.Entry(EditUser).State = System.Data.Entity.EntityState.Modified;//添加修改标记
                        db.SaveChanges();

                        regionManager.Regions["ContentRegion"].NavigationService.RequestNavigate("List");
                    }
                }
                DialogParameters parameters = new DialogParameters { { "EditUser.Id",EditUser.Id },
                    { "EditUser.UserName",EditUser.UserName },
                    { "EditUser.Nickname",EditUser.Nickname },
                    { "EditUser.Password",EditUser.Password },
                    { "EditUser.Logo",EditUser.Logo },
                };
                RequestClose.Invoke(new DialogResult(ButtonResult.OK,parameters));

            });
        }

        public DelegateCommand BackCommand
        {
            get => new DelegateCommand(() =>
            {
                regionManager.Regions["ContentRegion"].NavigationService.RequestNavigate("List");
                RequestClose.Invoke(new DialogResult(ButtonResult.Cancel));
            });
        }

        public string Title => "添加";
    }
}
