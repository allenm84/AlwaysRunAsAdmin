using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelFramework;

namespace AlwaysRunAsAdmin
{
  public class AppPathEditorViewModel : BaseViewModel
  {
    private readonly AppPathViewModel mSource;

    private readonly DelegateCommand mAcceptCommand;
    private readonly DelegateCommand mRejectCommand;

    public AppPathEditorViewModel(AppPathViewModel source)
    {
      mSource = source;

      mAcceptCommand = new DelegateCommand(DoAccept, CanAccept);
      mRejectCommand = new DelegateCommand(DoReject);

      Filepath = mSource.Filepath;
    }

    public string Filepath
    {
      get { return GetField<string>(); }
      set { SetField(value); }
    }

    public BaseCommand AcceptCommand
    {
      get { return mAcceptCommand; }
    }

    public BaseCommand RejectCommand
    {
      get { return mRejectCommand; }
    }

    private void DoReject()
    {
      SetCompleted(false);
    }

    private bool CanAccept()
    {
      string path = Filepath;
      return
        !string.IsNullOrWhiteSpace(path) &&
        File.Exists(path) &&
        Path.GetExtension(path) == ".exe";
    }

    private void DoAccept()
    {
      string filepath = Filepath;
      if (!string.Equals(mSource.Filepath, filepath))
      {
        mSource.Filepath = filepath;
        mSource.IsDirty = true;
      }

      SetCompleted(true);
    }

    protected override void AfterPropertyChanged(string propertyName)
    {
      base.AfterPropertyChanged(propertyName);
      mAcceptCommand.Refresh();
    }
  }
}
