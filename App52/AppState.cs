using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace App52
{
  public abstract class ViewModelBase :
    INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected bool SetProperty<T>(ref T storage, T value,
      [CallerMemberName] String propertyName = null)
    {
      if (object.Equals(storage, value)) return false;

      storage = value;
      this.OnPropertyChanged(propertyName);
      return true;
    }
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var eventHandler = this.PropertyChanged;
      if (eventHandler != null)
      {
        eventHandler(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }

  class AppState : ViewModelBase
  { 
    private AppState()
    {
      this.SecondaryTileIds = new List<string>();
    }
    public static AppState Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new AppState();
        }
        return (instance);
      }
    }

    public int Activated
    {
      get
      {
        return (this.activated);
      }
      set
      {
        base.SetProperty(ref this.activated, value);
      }
    }
    int activated;

    public int LaunchedFromPrimaryTile
    {
      get
      {
        return (this.launchedFromPrimaryTile);
      }
      set
      {
        base.SetProperty(ref this.launchedFromPrimaryTile, value);
      }
    }
    int launchedFromPrimaryTile;

    public int LaunchedFromKnownSecondaryTile
    {
      get
      {
        return (this.launchedFromKnownSecondaryTile);
      }
      set
      {
        base.SetProperty(ref this.launchedFromKnownSecondaryTile, value);
      }
    }
    int launchedFromKnownSecondaryTile;

    public int LaunchedFromUnknownSecondaryTile
    {
      get
      {
        return (this.launchedFromUnknownSecondaryTile);
      }
      set
      {
        base.SetProperty(ref this.launchedFromUnknownSecondaryTile, value);
      }
    }
    int launchedFromUnknownSecondaryTile;

    public int LaunchedWhenNotRunning
    {
      get
      {
        return (this.launchedWhenNotRunning);
      }
      set
      {
        base.SetProperty(ref this.launchedWhenNotRunning, value);
      }
    }
    int launchedWhenNotRunning;

    public int LaunchedWhenRunning
    {
      get
      {
        return (this.launchedWhenRunning);
      }
      set
      {
        base.SetProperty(ref this.launchedWhenRunning, value);
      }
    }
    int launchedWhenRunning;

    public int LaunchedWhenClosedByUser
    {
      get
      {
        return (this.launchedWhenClosedByUser);
      }
      set
      {
        base.SetProperty(ref this.launchedWhenClosedByUser, value);
      }
    }
    int launchedWhenClosedByUser;

    public int LaunchedWhenTerminated
    {
      get
      {
        return (this.launchedWhenTerminated);
      }
      set
      {
        base.SetProperty(ref this.launchedWhenTerminated, value);
      }
    }
    int launchedWhenTerminated;

    public int NumberOfSecondaryTilesSeen
    {
      get
      {
        return (this.secondaryTilesSeen);
      }
      set
      {
        base.SetProperty(ref this.secondaryTilesSeen, value);
      }
    }
    int secondaryTilesSeen;

    public List<string> SecondaryTileIds
    {
      get
      {
        return (this.secondaryTileIds);
      }
      set
      {
        base.SetProperty(ref this.secondaryTileIds, value);
      }
    }
    List<string> secondaryTileIds;

    public int EnteredBackground
    {
      get
      {
        return (this.enteredBackground);
      }
      set
      {
        base.SetProperty(ref this.enteredBackground, value);
      }
    }
    int enteredBackground;

    public int LeftBackground
    {
      get
      {
        return (this.leftBackground);
      }
      set
      {
        base.SetProperty(ref this.leftBackground, value);
      }
    }
    int leftBackground;

    public int Suspended
    {
      get
      {
        return (this.suspended);
      }
      set
      {
        base.SetProperty(ref this.suspended, value);
      }
    }
    int suspended;

    public int Resumed
    {
      get
      {
        return (this.resumed);
      }
      set
      {
        base.SetProperty(ref this.resumed, value);
      }
    }
    int resumed;

    public int BackgroundActivations
    {
      get
      {
        return (this.backgroundActivations);
      }
      set
      {
        base.SetProperty(ref this.backgroundActivations, value);
      }
    }
    int backgroundActivations;

    [JsonIgnore]
    public bool HasLoaded { get; private set; } 

    public async Task SaveAsync()
    {
      var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
        stateFile, CreationCollisionOption.ReplaceExisting);

      var content = JsonConvert.SerializeObject(this);
      await FileIO.WriteTextAsync(file, content);
    }
    public async Task LoadAsync()
    {
      try
      {
        var file = await ApplicationData.Current.LocalFolder.GetFileAsync(stateFile);
        var content = await FileIO.ReadTextAsync(file);
        var state = JsonConvert.DeserializeObject<AppState>(content);
        instance = state;
      }
      catch (FileNotFoundException)
      {

      }
      instance.HasLoaded = true;
    }
    static AppState instance;  
    const string stateFile = "state.json";
  }
}