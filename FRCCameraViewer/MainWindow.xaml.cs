using FRCCameraViewer.Properties;
using MjpegProcessor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FRCCameraViewer {
    public partial class MainWindow : Window {

        MjpegDecoder mjpeg;
        Timer timer;

        string lastText = "";

        public MainWindow() {
            InitializeComponent();
            mjpeg = new MjpegDecoder();
            mjpeg.FrameReady += FrameReady;

            timer = new Timer();
            timer.AutoReset = true;
            timer.Interval = 1000;
            timer.Elapsed += (sender, e) => {
                Application.Current.Dispatcher.Invoke(() => {
                    if (!lastText.Equals(source.Text))
                        mjpeg.StopStream();
                    lastText = source.Text;
                    mjpeg.ParseStream(new Uri(source.Text));
                });
            };
            timer.Start();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            Settings.Default.Save();
            base.OnClosing(e);
        }

        private void FrameReady(object sender, FrameReadyEventArgs e) {
            imageView.Source = e.BitmapImage;
        }
    }
}
