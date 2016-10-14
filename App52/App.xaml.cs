namespace App52
{
  using Windows.ApplicationModel;
  using Windows.ApplicationModel.Activation;
  using Windows.UI.Xaml;
  using Windows.UI.Xaml.Controls;

  sealed partial class App : Application
  {
    public App()
    {
      this.InitializeComponent();
      this.LeavingBackground += OnLeftBackground;
      this.EnteredBackground += OnEnteredBackground;
      this.Suspending += OnSuspending;
      this.Resuming += OnResuming;
    }
    void OnLeftBackground(object sender, LeavingBackgroundEventArgs e)
    {
      AppState.Instance.LeftBackground++;
    }

    void OnEnteredBackground(object sender, EnteredBackgroundEventArgs e)
    {
      AppState.Instance.EnteredBackground++;
    }
    protected async override void OnLaunched(LaunchActivatedEventArgs e)
    {
      bool loadState =
        (e.PreviousExecutionState == ApplicationExecutionState.Terminated) ||
        (e.PreviousExecutionState == ApplicationExecutionState.NotRunning) ||
        (e.PreviousExecutionState == ApplicationExecutionState.ClosedByUser);

      if (loadState && !AppState.Instance.HasLoaded)
      {
        await AppState.Instance.LoadAsync();
      }
      switch (e.PreviousExecutionState)
      {
        case ApplicationExecutionState.NotRunning:
          AppState.Instance.LaunchedWhenNotRunning++;
          break;
        case ApplicationExecutionState.Running:
          AppState.Instance.LaunchedWhenRunning++;
          break;
        case ApplicationExecutionState.Suspended:
          break;
        case ApplicationExecutionState.Terminated:
          AppState.Instance.LaunchedWhenTerminated++;
          break;
        case ApplicationExecutionState.ClosedByUser:
          AppState.Instance.LaunchedWhenClosedByUser++;
          break;
        default:
          break;
      };

      if (e.TileId == "App")
      {
        AppState.Instance.LaunchedFromPrimaryTile++;
      }
      else if (e.TileId == "App2")
      {
        AppState.Instance.LaunchedFromKnownSecondaryTile++;
      }
      else
      {
        AppState.Instance.LaunchedFromUnknownSecondaryTile++;

        if (!AppState.Instance.SecondaryTileIds.Contains(e.TileId))
        {
          AppState.Instance.SecondaryTileIds.Add(e.TileId);
          AppState.Instance.NumberOfSecondaryTilesSeen++;
        }
      }
       
      Frame rootFrame = Window.Current.Content as Frame;

      if (rootFrame == null)
      {
        rootFrame = new Frame();

        Window.Current.Content = rootFrame;
      }

      if (e.PrelaunchActivated == false)
      {
        if (rootFrame.Content == null)
        {
          // When the navigation stack isn't restored navigate to the first page,
          // configuring the new page by passing required information as a navigation
          // parameter
          rootFrame.Navigate(typeof(MainPage), e.Arguments);
        }
        // Ensure the current window is active
        Window.Current.Activate();
      }
    }
    protected async override void OnActivated(IActivatedEventArgs args)
    {
      // Same as comment for OnBackgroundActivated.
      if (!AppState.Instance.HasLoaded)
      {
        await AppState.Instance.LoadAsync();
      }
      AppState.Instance.Activated++;
    }
    async void OnSuspending(object sender, SuspendingEventArgs e)
    {
      var deferral = e.SuspendingOperation.GetDeferral();

      AppState.Instance.Suspended++;
      await AppState.Instance.SaveAsync();

      deferral.Complete();
    }
    void OnResuming(object sender, object e)
    {
      AppState.Instance.Resumed++;
    }
    protected async override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
    {
      // At this point our UI may or may not be up and running and our state
      // may or may not have been loaded. That is - we may or may not have
      // been launched.
      var deferral = args.TaskInstance.GetDeferral();

      try
      {
        if (!AppState.Instance.HasLoaded)
        {
          await AppState.Instance.LoadAsync();
        }
        AppState.Instance.BackgroundActivations++;

        await AppState.Instance.SaveAsync();
      }
      finally
      {
        deferral.Complete();
      }
    }
  }
}
