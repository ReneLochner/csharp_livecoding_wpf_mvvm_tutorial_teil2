using ActReport.ViewModel;
using System;
using System.Windows;

namespace ActReport.UI
{
  class MainController : IController
  {
    public void ShowWindow(BaseViewModel viewModel)
    {
      //switch (viewModel)
      //{
      //  case Fall1:
      //    // tue was
      //    break;

      //  default:
      //    break;
      //}


      Window window = viewModel switch
      {
        // Wenn viewModel null ist -> ArgumentNullException
        null => throw new ArgumentNullException(nameof(viewModel)),

        // Wenn view Model vom Type EmployeeViewModel ist -> neues MainWindow instanzieren
        EmployeeViewModel _ => new MainWindow(),
        ActivityViewModel _ => new ActivityWindow(),

        // default -> InvalidOperationException
        _ => throw new InvalidOperationException($"Unknown ViewModel of type '{viewModel}'"),
      };

      window.DataContext = viewModel;
      window.ShowDialog();
    }
  }
}
