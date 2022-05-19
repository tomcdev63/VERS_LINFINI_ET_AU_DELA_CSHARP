using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

//---------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------


namespace VIEAD
{

    public partial class MainWindow : Window
    {

        private Boolean celestron_connected = false;
        private MediaPlayer player = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new Home();
        }


    //---------------------------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------------------------

        private async void OnLoad(object sender, EventArgs e)
        {
            while (true)
            {
                await Task.Delay(100);
                List<string> ports = new List<string>(SerialPort.GetPortNames());

                switch (ports.Contains("COM8"))
                {
                    case true:
                        if (celestron_connected == false)
                        {
                            checkbox_connected.IsChecked = true;
                            celestron_connected = true;
                            Buzz_sound();
                            Task.Delay(2000);
                        }
                        break;

                    case false:
                        if (celestron_connected == true)
                        {
                            checkbox_connected.IsChecked = false;
                            celestron_connected = false;
                            GoodBye_sound();
                        }
                        break;
                }
            }
        }

            //---------------------------------------------------------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------------------------------------------------------


            private void Buzz_sound()
            {
                var uri = new Uri(@"G:\TOMDEV\VERS_LINFINI_ET_AU_DELA\windows\data\buzz.mp3", UriKind.RelativeOrAbsolute);
                player.Open(uri);
                player.Play();
            }

            //---------------------------------------------------------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------------------------------------------------------


            private void GoodBye_sound()
            {
                var uri = new Uri(@"G:\TOMDEV\VERS_LINFINI_ET_AU_DELA\windows\data\goodbye.mp3", UriKind.RelativeOrAbsolute);
                player.Open(uri);
                player.Play();
            }

    }

}


