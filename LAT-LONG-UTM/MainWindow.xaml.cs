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
using System.Globalization;
using latLong;

namespace LAT_LONG_UTM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string decimalParaGrausMinutosSegundos(Double valorDecimal) {
            string valorGrausMinutosSegundos = "";
            bool negativo = false;
            if (valorDecimal < 0)
            {
                negativo = true;
                valorDecimal = valorDecimal * -1;
            }
            double graus = Math.Truncate(valorDecimal);
            double minutos = (valorDecimal - Math.Floor(valorDecimal)) * 60.0;
            double segundos = (minutos - Math.Floor(minutos)) * 60.0;

            minutos = Math.Floor(minutos);
            segundos = Math.Round(segundos, 2);
            if (negativo == true)
            {
                graus = graus * -1;
            }
            valorGrausMinutosSegundos = $"{graus}º{minutos}'{segundos}\"";
            return valorGrausMinutosSegundos;
        }

        public double coordenadaUtmDecimal(string textoCoordenada) {
            double coordenada = 0;
            textoCoordenada = textoCoordenada.Trim().Replace(" ", "").ToUpper().Replace("N", "").Replace("E", "").Replace(",",".");
            coordenada = Double.Parse(textoCoordenada, CultureInfo.InvariantCulture);
            return coordenada;

        }

        public double coordenadaLatLongDecimal(string textoCoordenada) {
            double coordenada = 0;
            textoCoordenada = textoCoordenada.Trim().Replace("°", "º").Replace(",",".").Replace(" ", "");
            if (Convert.ToDouble(textoCoordenada.Split("º")[0]) < 0)
            {
                coordenada = Double.Parse(textoCoordenada.Split("º")[0], CultureInfo.InvariantCulture) - Double.Parse(textoCoordenada.Split("º")[1].Split("'")[0], CultureInfo.InvariantCulture) / 60 - Double.Parse(textoCoordenada.Split("º")[1].Split("'")[1].Split("\"")[0], CultureInfo.InvariantCulture) / 3600;
            }
            else
            {
                coordenada = Double.Parse(textoCoordenada.Split("º")[0], CultureInfo.InvariantCulture) + Double.Parse(textoCoordenada.Split("º")[1].Split("'")[0], CultureInfo.InvariantCulture) / 60 + Double.Parse(textoCoordenada.Split("º")[1].Split("'")[1].Split("\"")[0], CultureInfo.InvariantCulture) / 3600;
            }
            return coordenada;
        }
        public MainWindow()
        {
            List<string> listaLetrasFuso = new List<string>() {"C","D","E","F","G","H","J","K","L","M","N","P","Q","R","S","T","U","V","W","X"};
            InitializeComponent();
            cbbLetraFuso.ItemsSource = listaLetrasFuso.AsEnumerable();
            cbbLetraFuso.SelectedIndex = 9;
        }

        private void btCalcularLatLongUtm_Click(object sender, RoutedEventArgs e)
        {
            if (txtbLatitude.Text != "" && txtbLongitude.Text != "")
            {
                LatLngUTMConverter converter = new LatLngUTMConverter("WGS 84");
                LatLngUTMConverter.LatLng latLng = new LatLngUTMConverter.LatLng() { Lat = coordenadaLatLongDecimal(txtbLatitude.Text), Lng = coordenadaLatLongDecimal(txtbLongitude.Text) };
                LatLngUTMConverter.UTMResult resultadoUtm = converter.convertLatLngToUtm(latLng.Lat, latLng.Lng);
                string stringResultado = $"{Math.Round(resultadoUtm.Northing, 3)}N {Math.Round(resultadoUtm.Easting, 3)}E Zona {resultadoUtm.Zona}";
                txtResultadosLatLongUtm.Text = stringResultado;
            }
            else
            {
                txtResultadosLatLongUtm.Text = "Verifique o preenchimento dos dados";
            }
        }

        private void btCalcularUtmLatLong_Click(object sender, RoutedEventArgs e)
        {
            if (txtbUtmEast.Text != "" && txtbLongitude.Text != "" && txtbUtmNorth.Text != "" && txtbUtmFuso.Text!= "")
            {
                LatLngUTMConverter converter = new LatLngUTMConverter("WGS 84");
                LatLngUTMConverter.UTMResult utm = new LatLngUTMConverter.UTMResult() { UTMEasting = coordenadaUtmDecimal(txtbUtmEast.Text), UTMNorthing = coordenadaUtmDecimal(txtbUtmNorth.Text), ZoneNumber = Convert.ToInt32(txtbUtmFuso.Text), ZoneLetter = cbbLetraFuso.SelectedItem.ToString() };
                LatLngUTMConverter.LatLng resultadoLatlng = converter.convertUtmToLatLng(utm.UTMEasting, utm.UTMNorthing, utm.ZoneNumber, utm.ZoneLetter);
                string stringResultado = $"Latitude {decimalParaGrausMinutosSegundos(resultadoLatlng.Lat)}N Longitude {decimalParaGrausMinutosSegundos(resultadoLatlng.Lng)}E";
                txtResultadosUtmLatLong.Text = stringResultado;
            }
            else
            {
                txtResultadosUtmLatLong.Text = "Verifique o preenchimento dos dados";

            }
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
        private void Minimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void bttInfo_Click(object sender, RoutedEventArgs e)
        {
            infoCriador infoCriador = new infoCriador();
            infoCriador.ShowDialog();
        }
    }
}
