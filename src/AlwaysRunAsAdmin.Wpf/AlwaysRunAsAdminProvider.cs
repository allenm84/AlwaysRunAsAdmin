using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelFramework;

namespace AlwaysRunAsAdmin.Wpf
{
  public class AlwaysRunAsAdminProvider : IViewModelReceiver
  {
    private readonly List<BaseViewModel> mWaiting;
    private readonly MainViewModel mViewModel;

    public AlwaysRunAsAdminProvider()
    {
      mWaiting = new List<BaseViewModel>();
      mViewModel = new MainViewModel();
      ViewModelBroadcaster.Instance.Add(this);
    }

    public MainViewModel ViewModel
    {
      get { return mViewModel; }
    }

    private async void WaitForCompleted(BaseViewModel viewModel)
    {
      mWaiting.Add(viewModel);
      await viewModel.Completed;
      mWaiting.Remove(viewModel);
    }

    private async void Cleanup(BaseViewModel viewModel)
    {
      await Task.Yield();
      if (mWaiting.Contains(viewModel))
      {
        viewModel.Dispose();
      }
    }

    private async void ShowView(Window window, BaseViewModel viewModel)
    {
      await Task.Yield();
      WaitForCompleted(viewModel);

      window.DataContext = viewModel;
      window.ShowDialog();

      Cleanup(viewModel);
    }

    private async void ShowConfirmation(ConfirmViewModel confirm)
    {
      await Task.Yield();
      var result = MessageBox.Show(confirm.Message, confirm.Caption, 
        MessageBoxButton.YesNo, MessageBoxImage.Question);
      var command = (result == MessageBoxResult.Yes)
        ? confirm.YesCommand
        : confirm.NoCommand;
      command.Invoke(this);
    }

    private async void ShowAlert(AlertViewModel alert)
    {
      await Task.Yield();
      MessageBox.Show(alert.Message, alert.Caption,
        MessageBoxButton.OK, MessageBoxImage.Information);
      alert.AcceptCommand.Invoke(this);
    }

    bool IViewModelReceiver.Receive(BaseViewModel viewModel)
    {
      var editor = viewModel as AppPathEditorViewModel;
      if (editor != null)
      {
        ShowView(new AppPathEditorView(), editor);
        return true;
      }

      var confirmRefresh = viewModel as ConfirmRefreshViewModel;
      if (confirmRefresh != null)
      {
        ShowView(new ConfirmRefreshView(), confirmRefresh);
        return true;
      }

      var confirm = viewModel as ConfirmViewModel;
      if (confirm != null)
      {
        ShowConfirmation(confirm);
        return true;
      }

      var alert = viewModel as AlertViewModel;
      if (alert != null)
      {
        ShowAlert(alert);
        return true;
      }

      return false;
    }
  }
}
