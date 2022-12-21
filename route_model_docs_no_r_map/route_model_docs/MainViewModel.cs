// Developer Express Code Central Example:
// How to navigate through views by using NavBarControl control and NavigationFrame class
// 
// This example demonstrates how to implement navigation between views by using the
// NavBarControl control and NavigationFrame class.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E5129

using DevExpress.Mvvm;
using route_model_docs.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace route_model_docs {
    public class MainViewModel : ViewModelBase {

        public ObservableCollection<UserControlViewModel> Items {
            get;
            set;
        }


        private UserControlViewModel _SelectedViewModel;
        public UserControlViewModel SelectedViewModel {
            get {
                return _SelectedViewModel;
            }
            set {
                SetProperty(ref _SelectedViewModel, value, () => SelectedViewModel);
                ServiceContainer.GetService<INavigationService>().Navigate(SelectedViewModel.ViewName, null, this);
            }
        }

        public MainViewModel() {
            Items = new ObservableCollection<UserControlViewModel>() {
                new UserControlViewModel() { ViewName = "_3Д_Модель"},
                new UserControlViewModel() { ViewName = "Карта"},
                new UserControlViewModel() { ViewName = "Документы"},
            };

        }
    }
}
