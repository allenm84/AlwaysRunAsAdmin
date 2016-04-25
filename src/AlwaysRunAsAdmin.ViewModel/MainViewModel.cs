using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ViewModelFramework;

namespace AlwaysRunAsAdmin
{
  public class MainViewModel : BaseEditableListViewModel<AppPathViewModel>
  {
    const string RegPath = @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags";

    private readonly DelegateCommand mRefreshCommand;
    private readonly DelegateCommand mApplyCommand;

    public MainViewModel()
    {
      mRefreshCommand = new DelegateCommand(DoRefresh);
      mApplyCommand = new DelegateCommand(DoApply, CanApply);

      DoRefresh();
    }

    public BaseCommand RefreshCommand
    {
      get { return mRefreshCommand; }
    }

    public BaseCommand ApplyCommand
    {
      get { return mApplyCommand; }
    }

    private async void DoRefresh()
    {
      if (mItems.Any(v => v.IsDirty))
      {
        var confirm = new ConfirmRefreshViewModel("You have unsaved changes. Are you sure you want to refresh?", "Refresh");
        if (!Send(confirm))
        {
          return;
        }

        var yes = await confirm.Completed;
        if (!yes)
        {
          return;
        }

        if (confirm.SaveChanges)
        {
          DoApply();
        }
      }

      mItems.Clear();

      var flags = Registry.CurrentUser.OpenSubKey(RegPath, true);
      var layers = flags.OpenSubKey("Layers");
      if (layers == null)
      {
        layers = flags.CreateSubKey("Layers");
      }

      var names = layers.GetValueNames();
      foreach (var name in names)
      {
        mItems.Add(new AppPathViewModel(name));
      }
    }

    private bool CanApply()
    {
      return mItems.Any(i => i.IsDirty);
    }

    private void DoApply()
    {
      var programs = mItems
        .Select(p => p.Filepath)
        .ToArray();

      var flags = Registry.CurrentUser.OpenSubKey(RegPath, true);
      var layers = flags.OpenSubKey("Layers", true);
      if (layers == null)
      {
        layers = flags.CreateSubKey("Layers");
      }

      var comp = StringComparer.CurrentCultureIgnoreCase;
      var names = layers.GetValueNames();

      // remove the programs that are no longer valid
      var removed = names.Except(programs, comp);
      foreach (var r in removed)
      {
        layers.DeleteValue(r, false);
      }

      // retrieve the names and set the values
      foreach (var p in programs)
      {
        layers.SetValue(p, "RUNASADMIN", RegistryValueKind.String);
      }
    }

    protected override AppPathViewModel CreateNew()
    {
      return new AppPathViewModel();
    }

    protected override void InternalOnListChanged(ListChangedType type)
    {
      base.InternalOnListChanged(type);
      mApplyCommand.Refresh();
    }
  }
}
