using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AlwaysRunAsAdmin.Wpf
{
  public static class WindowCloseBehavior
  {
    public static readonly DependencyProperty AcceptButtonProperty = DependencyProperty.RegisterAttached(
      "AcceptButton", typeof(Button), typeof(WindowCloseBehavior), 
      new FrameworkPropertyMetadata(null, OnAcceptButtonChanged));

    public static Button GetAcceptButton(DependencyObject d)
    {
      return (Button)d.GetValue(AcceptButtonProperty);
    }

    public static void SetAcceptButton(DependencyObject d, Button button)
    {
      d.SetValue(AcceptButtonProperty, button);
    }

    private static void OnAcceptButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      SetupBinding(d, e, isDefault: true);
    }

    public static readonly DependencyProperty CancelButtonProperty = DependencyProperty.RegisterAttached(
      "CancelButton", typeof(Button), typeof(WindowCloseBehavior),
      new FrameworkPropertyMetadata(null, OnCancelButtonChanged));

    public static Button GetCancelButton(DependencyObject d)
    {
      return (Button)d.GetValue(CancelButtonProperty);
    }

    public static void SetCancelButton(DependencyObject d, Button button)
    {
      d.SetValue(CancelButtonProperty, button);
    }

    private static void OnCancelButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      SetupBinding(d, e, isCancel: true);
    }

    private static void SetupBinding(DependencyObject d, DependencyPropertyChangedEventArgs e, bool isDefault = false, bool isCancel = false)
    {
      var oldButton = e.OldValue as Button;
      if (oldButton != null)
      {
        oldButton.Click -= OnButtonClick;
      }

      var newButton = e.NewValue as Button;
      if (newButton == null)
      {
        return;
      }

      newButton.IsDefault = isDefault;
      newButton.IsCancel = isCancel;
      newButton.Click += OnButtonClick;
    }

    private static void OnButtonClick(object sender, RoutedEventArgs e)
    {
      Window.GetWindow((DependencyObject)sender).Close();
    }
  }
}
