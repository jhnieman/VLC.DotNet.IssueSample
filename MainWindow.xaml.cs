using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Vlc.DotNet.Core;
using Vlc.DotNet.Wpf;

namespace VLC.NetTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // a reference to the VLC control
        private VlcControl control;
        // A path to the VLC files
        private DirectoryInfo VlcLibDirectory = GetVlcLibDirectory();
        // A default internet location for stream testing
        private string internetStreamPath = @"rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov";

        public MainWindow()
        {
            InitializeComponent();

            // Instantiate the player
            SetupNewVlcPlayer();

            // Hook up the media events for error handling
            HookPlayerEvents();

            // Add it to the UI
            mainGrid.Children.Add(control);

            // Start playing
            PlayStream(internetStreamPath);
        }

        private void HookPlayerEvents()
        {
            this.control.SourceProvider.MediaPlayer.EncounteredError += MediaPlayer_EncounteredError; ;
            this.control.SourceProvider.MediaPlayer.EndReached += MediaPlayer_EndReached; ;
        }

        private void MediaPlayer_EndReached(object sender, VlcMediaPlayerEndReachedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MediaPlayer_EncounteredError(object sender, VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            throw new NotImplementedException();
        }


        private void SetupNewVlcPlayer()
        {
            // Set up the objects
            this.control = new VlcControl();
            this.control.Background = System.Windows.Media.Brushes.Transparent;
            var options = new string[]
            {
            "--file-logging",
            "-vvv",
            "--extraintf=logger",
            "--logfile=Logs.log"
            };
            this.control.SourceProvider.CreatePlayer(this.VlcLibDirectory, options);
            this.control.SourceProvider.MediaPlayer.Audio.Volume = 0;
        }

        public void PlayStream(string mediaPath)
        {
            if (this.control.SourceProvider.MediaPlayer != null)
            {
                // Restart if you reach the end of the media
                //string[] mediaOptions = { "input-repeat=65535" };
                string[] mediaOptions = { };

                Uri mediaUri;
                try
                {
                    mediaUri = new Uri(mediaPath);
                    this.control.SourceProvider.MediaPlayer.SetMedia(mediaUri, mediaOptions);
                    this.control.SourceProvider.MediaPlayer.Play();
                }
                catch
                {
                    return;
                }
            }
        }

        private static DirectoryInfo GetVlcLibDirectory()
        {
            var currentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            return libDirectory;
        }
    }
}
