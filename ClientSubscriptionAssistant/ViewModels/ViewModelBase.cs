using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        private bool _isBusy;
        private string _title;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        protected Command CreateCommand(Action execute, Func<bool> canExecute = null)
        {
            return new Command(execute, canExecute);
        }

        protected Command<T> CreateCommand<T>(Action<T> execute, Func<T, bool> canExecute = null)
        {
            return new Command<T>(execute, canExecute);
        }
    }
}
