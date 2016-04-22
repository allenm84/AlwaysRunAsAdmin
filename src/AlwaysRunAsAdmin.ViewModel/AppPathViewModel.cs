using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelFramework;

namespace AlwaysRunAsAdmin
{
  public class AppPathViewModel : BaseEditableItemViewModel
  {
    internal AppPathViewModel(string filepath = null)
    {
      Filepath = filepath;
      IsDirty = false;
    }

    internal bool IsDirty
    {
      get { return GetField<bool>(); }
      set { SetField(value); }
    }
    
    public string Filepath
    {
      get { return GetField<string>(); }
      internal set { SetField(value); }
    }

    public override BaseViewModel CreateEditor(bool isForAdding)
    {
      return new AppPathEditorViewModel(this);
    }
  }
}
