using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App52
{
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
      this.Loaded += OnLoaded;
    }
    void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.DataContext = AppState.Instance;
    }

    async void OnPinSecondaryTile(object sender, RoutedEventArgs e)
    {
      var tileName = "App2";
      var tiles = await SecondaryTile.FindAllAsync();

      if (tiles.Count(t => t.DisplayName == tileName) == 0)
      {
        Uri square150x150Logo = new Uri("ms-appx:///Assets/square150x150Tile-sdk.png");
        Uri wide310x150Logo = new Uri("ms-appx:///Assets/wide310x150Tile-sdk.png");
        Uri square310x310Logo = new Uri("ms-appx:///Assets/square310x310Tile-sdk.png");

        // Create a medium size Secondary tile
        SecondaryTile secondaryTile = new SecondaryTile(
          tileName,
          "Title text shown on the tile",
          "my arguments",          
          square150x150Logo,
          TileSize.Square150x150);

        // To have the larger tile sizes available the assets must be provided.
        secondaryTile.VisualElements.Wide310x150Logo = wide310x150Logo;
        secondaryTile.VisualElements.Square310x310Logo = square310x310Logo;

        // The display of the app name can be controlled for each tile size.
        // The default is false.
        secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
        secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;
        secondaryTile.VisualElements.ShowNameOnSquare310x310Logo = true;

        // Specify a foreground text value.
        // The tile background color is inherited from the parent unless a separate value is specified.
        secondaryTile.VisualElements.ForegroundText = ForegroundText.Dark;

        // OK, the tile is created and we can now attempt to pin the tile.
        // Note that the status message is updated when the async operation to pin the tile completes.
        bool isPinned = await secondaryTile.RequestCreateAsync();
      }
    }

    async void OnRegisterTask(object sender, RoutedEventArgs e)
    {
      var status = BackgroundExecutionManager.GetAccessStatus();

      if (status == BackgroundAccessStatus.Unspecified)
      {
        status = await BackgroundExecutionManager.RequestAccessAsync();
      }
      bool allowed = true;

      switch (status)
      {
        case BackgroundAccessStatus.Unspecified:
        case BackgroundAccessStatus.DeniedBySystemPolicy:
        case BackgroundAccessStatus.DeniedByUser:
          allowed = false;
          break;
        default:
          break;
      }
      if ((allowed) && (BackgroundTaskRegistration.AllTasks.Count == 0))
      {
        // NB: we do not have to specify the background task type, if we leave
        // it empty then we assume the single process execution model.
        var backgroundTaskBuilder = new BackgroundTaskBuilder();
        backgroundTaskBuilder.Name = "My Background Task";
        backgroundTaskBuilder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
        backgroundTaskBuilder.Register();
      }
    }
    void OnUnregisterTask(object sender, RoutedEventArgs e)
    {
      var tasks = BackgroundTaskRegistration.AllTasks;

      if (BackgroundTaskRegistration.AllTasks.Count > 0)
      {
        BackgroundTaskRegistration.AllTasks.First().Value.Unregister(false);
      }
    }
  }
}
