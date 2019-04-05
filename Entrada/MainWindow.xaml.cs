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
            cmb_Dispositivos.SelectedIndex = 0;
        }

        private void btn_Iniciar_Click(object sender, RoutedEventArgs e)
        {
            waveIn = new WaveIn();

            // Formato de Audio (Sample Rate, Bit Depth, Channels)
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);

            // Buffer
            waveIn.BufferMilliseconds = 250;

            // ¿Qué hacer cuando hay muestras disponibles?
            waveIn.DataAvailable += WaveIn_DataAvailable;

            waveIn.StartRecording();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesGrabados = e.BytesRecorded;
            float acumulador = 0.0f;

            // Aquí se transforman 2 bytes separados en una muestra de 16 bits
            for (int i = 0; i < bytesGrabados; i += 2)
            {                                           // Este "OR" revisa ambos parámetros para ver si se cumple la condición
                short muestra = (short)(buffer[i + 1] << 8 | buffer[i]);

                /* 1. Se toma el segundo byte y anteponemos 8 ceros al principio
                   2. Se hace un OR (matemática discreta) con el primer byte, al cual automáticamente se le llenan los 8 ceros 
                   del principio con los últimos 8 valores del segundo byte */

                float muestra32bits = (float)(muestra/32768.0f);
                acumulador += Math.Abs(muestra32bits);
            }
            /* Puesto que cada muestra son 2 bytes, divides entre 2 el número de bytes grabados para dividir lo acumulado entre
               el número de muestras (para obtener el promedio) */
            float promedio = acumulador / (bytesGrabados / 2.0f);
            sld_Microfono.Value = (double)promedio;
        }

        private void btn_Detener_Click(object sender, RoutedEventArgs e)
        {
            waveIn.StopRecording();
        }
    }
}
