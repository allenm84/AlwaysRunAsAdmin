using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelFramework;

namespace AlwaysRunAsAdmin
{
  public class ConfirmRefreshViewModel : ConfirmViewModel
  {
    public ConfirmRefreshViewModel(string message, string caption)
      : base(message, caption)
    {
      SaveChanges = true;
    }

    public bool SaveChanges
    {
      get { return GetField<bool>(); }
      set { SetField(value); }
    }
  }
}
