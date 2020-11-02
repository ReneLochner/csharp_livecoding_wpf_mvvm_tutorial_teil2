using ActReport.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ActReport.UI {
    public class MainController : IController {
        private Dictionary<BaseViewModel, Window> _windows;

        public MainController()
        {
            this._windows = new Dictionary<BaseViewModel, Window>();
        }

        public void ShowWindow(BaseViewModel viewModel)
        {
            Window window = viewModel switch
            {
                null => throw new ArgumentNullException(nameof(viewModel)),
                EmployeeViewModel _ => new MainWindow(),
                ActivityViewModel _ => new ActivityWindow(),
                AddActivityViewModel _ => new AddActivityWindow(),
                _ => throw new InvalidOperationException($"Unknown ViewModel of type '{viewModel}'"),
            };

            _windows[viewModel] = window;
            window.DataContext = viewModel;
            window.ShowDialog();
        }

        public void CloseWindow(BaseViewModel viewModel)
        {
            if (_windows.ContainsKey(viewModel))
            {
                _windows[viewModel].Close();
                _windows.Remove(viewModel);
            }
        }
    }
}
