using System.Windows;
using System.Windows.Input;

namespace Handle.WPF.Controls
{
  [TemplatePart(Name = PART_TitleBar, Type = typeof(UIElement))]
  [TemplatePart(Name = PART_WindowCommands, Type = typeof(WindowCommands))]
  public class MetroWindow : Window
  {
    private const string PART_TitleBar = "PART_TitleBar";
    private const string PART_WindowCommands = "PART_WindowCommands";

    public static readonly DependencyProperty ShowIconOnTitleBarProperty = DependencyProperty.Register("ShowIconOnTitleBar", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
    public static readonly DependencyProperty ShowTitleBarProperty = DependencyProperty.Register("ShowTitleBar", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
    public static readonly DependencyProperty ShowSettingsInTitleBarProperty = DependencyProperty.Register("ShowSettingsInTitleBar", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false));
    public static readonly DependencyProperty TitleTextProperty = DependencyProperty.Register("TitleText", typeof(string), typeof(MetroWindow), new PropertyMetadata(""));
    public static readonly DependencyProperty ShowProgressBarProperty = DependencyProperty.Register("ShowProgressBar", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false));

    private WindowCommands windowCommands;

    static MetroWindow()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
    }

    public bool ShowIconOnTitleBar
    {
      get { return (bool)GetValue(ShowIconOnTitleBarProperty); }
      set { SetValue(ShowIconOnTitleBarProperty, value); }
    }

    public bool ShowTitleBar
    {
      get { return (bool)GetValue(ShowTitleBarProperty); }
      set { SetValue(ShowTitleBarProperty, value); }
    }

    public bool ShowSettingsInTitleBar
    {
      get { return (bool)GetValue(ShowSettingsInTitleBarProperty); }
      set { SetValue(ShowSettingsInTitleBarProperty, value); }
    }

    public string TitleText
    {
      get { return (string)GetValue(TitleTextProperty); }
      set { SetValue(TitleTextProperty, value); }
    }

    public bool ShowProgressBar
    {
      get { return (bool)GetValue(ShowProgressBarProperty); }
      set { SetValue(ShowProgressBarProperty, value); }
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      var titleBar = GetTemplateChild(PART_TitleBar) as UIElement;
      windowCommands = GetTemplateChild(PART_WindowCommands) as WindowCommands;

      if (titleBar != null)
      {
        titleBar.MouseDown += TitleBarMouseDown;
        titleBar.MouseMove += TitleBarMouseMove;
      }
      else
      {
        MouseDown += TitleBarMouseDown;
        MouseMove += TitleBarMouseMove;
      }
    }

    protected override void OnStateChanged(System.EventArgs e)
    {
      if (windowCommands != null)
      {
        windowCommands.RefreshMaximiseIconState();
      }

      base.OnStateChanged(e);
    }

    protected void TitleBarMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.RightButton != MouseButtonState.Pressed && e.MiddleButton != MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Pressed)
        DragMove();

      if (e.ClickCount == 2)
      {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
      }
    }

    protected void TitleBarMouseMove(object sender, MouseEventArgs e)
    {
      if (e.RightButton != MouseButtonState.Pressed && e.MiddleButton != MouseButtonState.Pressed
          && e.LeftButton == MouseButtonState.Pressed && WindowState == WindowState.Maximized)
      {
        // Calcualting correct left coordinate for multi-screen system.
        double mouseX = PointToScreen(Mouse.GetPosition(this)).X;
        double width = RestoreBounds.Width;
        double left = mouseX - width / 2;

        // Aligning window's position to fit the screen.
        double virtualScreenWidth = SystemParameters.VirtualScreenWidth;
        left = left < 0 ? 0 : left;
        left = left + width > virtualScreenWidth ? virtualScreenWidth - width : left;

        Top = 0;
        Left = left;

        // Restore window to normal state.
        WindowState = WindowState.Normal;

        DragMove();
      }
    }

    internal T GetPart<T>(string name) where T : DependencyObject
    {
      return (T)GetTemplateChild(name);
    }
  }
}
