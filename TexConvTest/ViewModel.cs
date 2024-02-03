using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using TexConverter;

namespace TexConvTest
{
    public class ViewModel : ObservableObject
    {
        public ObservableCollection<ConvertProfile> Profiles { get; set; }
        private ConvertProfile _profile= ConvertProfiles.BC7sRgbFast;
        public ConvertProfile Profile
        {
            get { return _profile; }
            set { if (SetProperty(ref _profile, value)) { } }
        }

        private string _currentFile;
        public string CurrentFile
        {
            get { return _currentFile; }
            set
            {
                if (SetProperty(ref _currentFile, value))
                {
                    CurrentDist = Path.ChangeExtension(CurrentFile, ".dds");
                }
            }
        }
        private string _currentDist;
        public string CurrentDist
        {
            get { return _currentDist; }
            set { if (SetProperty(ref _currentDist, value)) { } }
        }
        public ICommand GetSourceImageCommand { get; set; }
        public ICommand StartConversionCommand { get; set; }

        private DispatcherTimer _timer;
        private Stopwatch _stopwatch;
        private string _elapsedTime="0:0";

        public string ElapsedTime
        {
            get { return _elapsedTime; }
            set { SetProperty(ref _elapsedTime, value); }
        }
        TextBlock TimeView { get; }
        public ViewModel(TextBlock text)
        {
            TimeView = text;
            Profiles = new ObservableCollection<ConvertProfile>(ConvertProfiles.GetAllProfiles());
            GetSourceImageCommand = new RelayCommand(ExcuteGetSourceImageCommand);
            StartConversionCommand = new RelayCommand(ExcuteStartConversionCommand);
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // Update every second
            _timer.Tick += Timer_Tick;
            _stopwatch = new Stopwatch();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the elapsed time
            TimeView.Text = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
        }

        private void ExcuteGetSourceImageCommand()
        {
            var fd = new OpenFileDialog();

            // Set the filter for PNG and JPG files
            fd.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";

            if ((bool)fd.ShowDialog())
            {
                CurrentFile = fd.FileName;
            }
        }

        private void ExcuteStartConversionCommand()
        {
            StartConversion();
        }

        private async void StartConversion()
        {
            // Start the timer
            _stopwatch.Restart();
            _timer.Start();
            

            // Wait for 5 seconds
            await TextureConverter.ConvertToDdsFileAsync(CurrentFile,CurrentDist,Profile);
            // Stop the timer
            _timer.Stop();
            _stopwatch.Stop();

            // Show message box
            MessageBox.Show($"Converting {Path.GetFileName(CurrentFile)} to DDS Done");
        }
    }
}
