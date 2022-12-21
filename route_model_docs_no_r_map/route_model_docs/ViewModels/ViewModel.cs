using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;

namespace route_model_docs.ViewModels
{
    public class UserControlViewModel : ViewModelBase
    {
        private string _ViewName;
        public string ViewName
        {
            get
            {
                return _ViewName;
            }
            set
            {
                SetProperty(ref _ViewName, value, () => ViewName);
            }
        }
    }
}
