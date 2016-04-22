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
    private readonly HashSet<BaseViewModel> mWaiting;
    private readonly MainViewModel mViewModel;

    public AlwaysRunAsAdminProvider()
    {
      mWaiting = new HashSet<BaseViewModel>();
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
      mWaiting.Remove(mViewModel);
    }

    private async void ShowView(Window window, BaseViewModel viewModel)
    {
      await Task.Yield();
      WaitForCompleted(viewModel);

      window.DataContext = viewModel;
      window.ShowDialog();

      if (mWaiting.Contains(viewModel))
      {
        viewModel.ForceCompleted();
      }
    }

    bool IViewModelReceiver.Receive(BaseViewModel viewModel)
    {
      var editor = viewModel as AppPathEditorViewModel;
      if (editor != null)
      {
        ShowView(new AppPathEditorView(), editor);
        return true;
      }

      return false;
    }
  }
}
