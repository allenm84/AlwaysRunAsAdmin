
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace AlwaysRunAsAdmin.Wpf
{
  /// <summary>
  /// Interaction logic for AppPathEditorView.xaml
  /// </summary>
  public partial class AppPathEditorView : Window
  {
    public AppPathEditorView()
    {
      InitializeComponent();
    }

    private void btnBrowseFilepath_Click(object sender, RoutedEventArgs e)
    {
      var dlg = new OpenFileDialog();
      dlg.CheckFileExists = true;
      dlg.CheckPathExists = true;
      dlg.Filter = "Applications (*.exe) | *.exe";
      dlg.Multiselect = false;
      dlg.RestoreDirectory = true;
      if (dlg.ShowDialog(this).GetValueOrDefault())
      {
        txtFilepath.Text = dlg.FileName;
      }
    }
  }
}
