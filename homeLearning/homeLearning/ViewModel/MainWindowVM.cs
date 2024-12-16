using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace homeLearning.viewModel
{
    public partial class MainWindowVM : BaseVM
    {
        static public Action<BaseVM> OnRequestVMChange;

        private BaseVM _currentVM;
        public BaseVM CurrentVM
        {
            get => _currentVM;
            set
            {
                //come from CommunotyToolsKit => Notify binding variables to display
                // informations in the view
                SetProperty(ref _currentVM, value);
                OnPropertyChanged(nameof(_currentVM));
            }
        }
        public MainWindowVM()
        {
            // Subscribe to HandleRequestViewChange
            // => Call the function when event is Invoke.
            MainWindowVM.OnRequestVMChange += HandleRequestViewChange;

            //Invoke the event with the newVM instancied
            MainWindowVM.OnRequestVMChange?.Invoke(new BaseLinkVM());

        }
        public void HandleRequestViewChange(BaseVM a_VMToChange)
        {
            //Notify currentVM will be hide
            CurrentVM?.OnHideVM();

            // Assign the new VM
            CurrentVM = a_VMToChange;

            //Notify currentVM will be shown
            CurrentVM?.OnShowVM();
        }
    }
}
