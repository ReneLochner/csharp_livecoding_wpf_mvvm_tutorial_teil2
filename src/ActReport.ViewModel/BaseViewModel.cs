using System.ComponentModel;

namespace ActReport.ViewModel {
    public class BaseViewModel : INotifyPropertyChanged {
        protected IController _controller;

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel(IController controller)
        {
            this._controller = controller;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
