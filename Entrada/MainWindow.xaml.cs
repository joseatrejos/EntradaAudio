using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Dsp;

namespace Entrada
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaveIn waveIn;
        public MainWindow()
        {
            InitializeComponent();
            LlenarComboDispositivos();
        }

        public void LlenarComboDispositivos()
        {
            for(int i = 0; i < WaveIn.DeviceCount; i++)
            {
                // La variable tipo WaveInCapabilities almacena los dispositivos en la iteración [i] hasta guardar todos los
                // aparatos de entrada
                WaveInCapabilities capacidades = WaveIn.GetCapabilities(i);

                // Ahora agregamos a los "items" de nuestro combo box el Nombre del dispositivo almacenado en capacidades[i]
                cmb_Dispositivos.Items.Add(capacidades.ProductName);
            }
        }

        private void btn_Iniciar_Click(object sender, RoutedEventArgs e)
        {
            waveIn = new WaveIn();

            // Formato de Audio (Sample Rate, Bit Depth, Channels)
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);

            // Buffer
            waveIn.BufferMilliseconds = 250;

            // 
        }
    }
}
