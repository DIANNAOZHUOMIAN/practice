using Demo.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Demo.ViewModels.UserControls
{
    public class ListViewModel : BindableBase, INavigationAware
    {
        private List<UserInfo> _userList;
        private readonly IRegionManager regionManager;
        private readonly IDialogService dialogService;
        private readonly IEventAggregator eventAggregator;

        public List<UserInfo> UserList
        {
            get { return _userList; }
            set { SetProperty(ref _userList, value); }
        }

        public ListViewModel(IRegionManager regionManager, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            InitialUserList();
            this.regionManager = regionManager;
            this.dialogService = dialogService;
            this.eventAggregator = eventAggregator;
        }

        private void InitialUserList(string userName = "", string nickName = "")
        {
            using (var db = new Db002Entities())
            {
                IEnumerable<UserInfo> list = db.UserInfo.AsEnumerable();
                if (!string.IsNullOrEmpty(userName))
                {
                    list = list.Where(u => u.UserName.Contains(userName));
                }

                if (!string.IsNullOrEmpty(nickName))
                {
                    list = list.Where(u => u.Nickname.Contains(nickName));
                }

                UserList = list.ToList();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public DelegateCommand<int?> EditCommand
        {
            get
            {
                return new DelegateCommand<int?>((id) =>
                {
                    var parameters = new NavigationParameters() {
                        { "Id",id.ToString()}
                    };
                    regionManager.Regions["ContentRegion"].NavigationService.RequestNavigate("Edit", parameters);
                });
            }
        }
        public DelegateCommand AddCommand { get => new DelegateCommand(() => {
            DialogParameters keyValuePairs = new DialogParameters { { "Title", "添加" } };
            dialogService.ShowDialog("Edit", keyValuePairs, new Action<IDialogResult>((s) =>
            {
                if(s.Result==ButtonResult.OK)
                {
                    using (var context = new Db002Entities()) {
                        UserInfo userInfo = new UserInfo {
                            UserName = s.Parameters.GetValue<string>("EditUser.UserName"),
                            Nickname = s.Parameters.GetValue<string>("EditUser.Nickname"),
                            Id = s.Parameters.GetValue<int>("EditUser.Id"),
                            Password = s.Parameters.GetValue<string>("EditUser.Password"),
                            Logo = Helpers.ImageHelper.ImgPathToByte(s.Parameters.GetValue<BitmapImage>("EditUser.Logo"))
                        };
                    }
                }
            }));
        }); }

        public DelegateCommand<object> SearchCommand
        {
            get
            {
                return new DelegateCommand<object>((obj) =>
                {
                    object[] arr = obj as object[];
                    InitialUserList(arr[0].ToString(), arr[1].ToString());
                });
            }
        }
        public DelegateCommand<int?> OpenDeletDialogCommand
        {
            get => new DelegateCommand<int?>(s =>
            {
                DialogParameters keyValuePairs = new DialogParameters { { "Id", s } };
                dialogService.ShowDialog("DeletDialog", keyValuePairs, new System.Action<IDialogResult>(d =>
                {
                    if (d.Result == ButtonResult.OK)
                    {
                        using (var context = new Db002Entities())
                        {
                            var entity = context.UserInfo.Find(s);
                            if (entity != null)
                            {
                                context.UserInfo.Remove(entity);
                                context.SaveChanges();
                                InitialUserList();
                            }
                        }
                    }
                }));
            });
        }

    }
}
