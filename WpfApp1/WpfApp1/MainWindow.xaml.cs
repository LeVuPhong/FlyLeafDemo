using FlyleafLib.MediaPlayer;
using FlyleafLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using FlyleafLib.Controls.WPF;

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public Player Player { get; set; }
		public Config Config { get; set; }

		// public string SampleVideo   { get; set; } = Utils.FindFileBelow("Sample.mp4"); // file mp4
		public string SampleVideo { get; set; } = "https://vcloud.vcv.vn:20991/livestream/UBHT_TV_4G00620PAGBDA12/index.mkv?token=5436f0a67c7e0498be2250eea1de8463b6076c0a0878899895&sessionId=64dae72b7ef4298282322ad3&localIp=10.10.10.15";


		public string LastError { get => _LastError; set { if (_LastError == value) return; _LastError = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastError))); } }
		string _LastError = string.Empty;

		public bool ShowDebug { get => _ShowDebug; set { if (_ShowDebug == value) return; _ShowDebug = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowDebug))); } }
		bool _ShowDebug;

		public ICommand ToggleDebug { get; set; }

		public MainWindow()
		{
			//InitializeComponent();
			// Initializes Engine (Specifies FFmpeg libraries path which is required)
			Engine.Start(new EngineConfig()
			{
#if DEBUG
				LogOutput = ":debug",
				LogLevel = LogLevel.Debug,
				FFmpegLogLevel = FFmpegLogLevel.Warning,
#endif

				PluginsPath = ":Plugins",
				FFmpegPath = ":FFmpeg",

				// Use UIRefresh to update Stats/BufferDuration (and CurTime more frequently than a second)
				UIRefresh = true,
				UIRefreshInterval = 100,
				UICurTimePerSecond = false // If set to true it updates when the actual timestamps second change rather than a fixed interval
			});

			ToggleDebug = new RelayCommandSimple(new Action(() => { ShowDebug = !ShowDebug; }));

			InitializeComponent();

			Config = new Config();

			// Inform the lib to refresh stats
			Config.Player.Stats = true;

			Player = new Player(Config);

			DataContext = this;

			// Keep track of error messages
			Player.OpenCompleted += (o, e) => { LastError = e.Error; };
			Player.BufferingCompleted += (o, e) => { LastError = e.Error; };
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
