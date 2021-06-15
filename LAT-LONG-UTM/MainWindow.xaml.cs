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
            textoCoordenada = textoCoordenada.Trim().Replace(" ", "").ToUpper().Replace("N", "").Replace("E", "").Replace(",", ".");
            coordenada = Double.Parse(textoCoordenada, CultureInfo.InvariantCulture);
            return coordenada;

        }

        public double coordenadaLatLongDecimal(string textoCoordenada) {
            double coordenada = 0;
            textoCoordenada = textoCoordenada.Trim().Replace("°", "º").Replace(",", ".").Replace(" ", "");
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
        public static double ConvertDegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }
        public static double ConvertRadiansToDegrees(double radians)
        {
            double degrees = radians / (Math.PI / 180)  ;
            return (degrees);
        }
        public class coordenadaXYZ{
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            }
        public class coordenadaLatLongAlt {
            public double latitude { get; set; }
            public double longitude { get; set; }
            public double altitude { get; set; }
        }

        public coordenadaXYZ converTerLatLongXYZ(double latitude, double longitude, double altitude) {

            coordenadaXYZ coordenadaConvertidaXYZ = new coordenadaXYZ();
            var cosLat = Math.Cos(latitude * Math.PI / 180.0);
            var sinLat = Math.Sin(latitude * Math.PI / 180.0);
            var cosLon = Math.Cos(longitude * Math.PI / 180.0);
            var sinLon = Math.Sin(longitude * Math.PI / 180.0);
            var rad = 6378137.0;
            var f = 1.0 / 298.257222101;
            var C = 1.0 / Math.Sqrt(cosLat * cosLat + (1 - f) * (1 - f) * sinLat * sinLat);
            var S = (1.0 - f) * (1.0 - f) * C;
            var h = altitude;
            double x = (rad * C + h) * cosLat * cosLon;
            double y = (rad * C + h) * cosLat * sinLon;
            double z = (rad * S + h) * sinLat;
            coordenadaConvertidaXYZ.x = x;
            coordenadaConvertidaXYZ.y = y;
            coordenadaConvertidaXYZ.z = z;
            return coordenadaConvertidaXYZ;
        }
        public coordenadaLatLongAlt converXYZTerLatLong(double latitude, double longitude, double altitude)
        {
            coordenadaLatLongAlt coordenada = new coordenadaLatLongAlt();
            double a = 6378137; // radius
            double e = 8.1819190842622e-2;
            double asq = Math.Pow(a, 2);
            double esq = Math.Pow(e, 2);

            double x = latitude;
            double y = longitude;
            double z = altitude;

            double b = Math.Sqrt(asq * (1 - esq));
            double bsq = Math.Pow(b, 2);
            double ep = Math.Sqrt((asq - bsq) / bsq);
            double p = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double th = Math.Atan2(a * z, b * p);

            double lon = Math.Atan2(y, x);
            double lat = Math.Atan2((z + Math.Pow(ep, 2) * b * Math.Pow(Math.Sin(th), 3)), (p - esq * a * Math.Pow(Math.Cos(th), 3)));
            double N = a / (Math.Sqrt(1 - esq * Math.Pow(Math.Sin(lat), 2)));
            double alt = p / Math.Cos(lat) - N;

            // mod lat to 0-2pi
            lon = lon % (2 * Math.PI);
            coordenada.latitude = ConvertRadiansToDegrees(lat);
            coordenada.longitude = ConvertRadiansToDegrees(lon);
            coordenada.altitude = alt;
            // correction for altitude near poles left out.
            return coordenada;
            //_ = c.Cartesian;

        }
        public MainWindow()
        {
            double latitude = -3.726225, longitude = -38.53271667, altitude = 20;
            //double x = raio * Math.Cos(ConvertDegreesToRadians(latitude)) * Math.Cos(ConvertDegreesToRadians(longitude));
            //double y = raio * Math.Cos(ConvertDegreesToRadians(latitude)) * Math.Sin(ConvertDegreesToRadians(longitude));
            //double z = raio * Math.Sin(ConvertDegreesToRadians(latitude));


            
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
            if (txtbUtmEast.Text != "" && txtbUtmNorth.Text != "" && txtbUtmFuso.Text!= "")
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

        private void btCalcularLatLongXYZ_Click(object sender, RoutedEventArgs e)
        {
            if (txtbLatitudeXYZ.Text != "" && txtbLongitudeXYZ.Text != "" && txtbAltitudeXYZ.Text != "")
            {
                LatLngUTMConverter converter = new LatLngUTMConverter("WGS 84");
                LatLngUTMConverter.LatLng latLng = new LatLngUTMConverter.LatLng() { Lat = coordenadaLatLongDecimal(txtbLatitudeXYZ.Text), Lng = coordenadaLatLongDecimal(txtbLongitudeXYZ.Text) };
                coordenadaXYZ cordXYZ = converTerLatLongXYZ(latLng.Lat, latLng.Lng, Double.Parse(txtbAltitudeXYZ.Text.Replace(",","."), CultureInfo.InvariantCulture)) ;
                string stringResultado = $"{Math.Round(cordXYZ.x, 3)}X {Math.Round(cordXYZ.y, 3)}Y {Math.Round(cordXYZ.z,3)}Z";
                txtResultadosLatLongXYZ.Text = stringResultado;
            }
            else
            {
                txtResultadosLatLongXYZ.Text = "Verifique o preenchimento dos dados";
            }
        }

        private void btCalcularXYZLatLong_Click(object sender, RoutedEventArgs e)
        {
            if (txtbXYZLatitude.Text != "" && txtLongitude.Text != "" && txtAltitudeXYZ.Text != "")
            {
                coordenadaLatLongAlt coordenadaLatLongAlt = converXYZTerLatLong(Double.Parse(txtbXYZLatitude.Text.Replace(",", "."), CultureInfo.InvariantCulture), Double.Parse(txtbXYZLongitude.Text.Replace(",", "."), CultureInfo.InvariantCulture), Double.Parse(txtbXYZAltitude.Text.Replace(",", "."), CultureInfo.InvariantCulture));
                string stringResultado = $"{decimalParaGrausMinutosSegundos(coordenadaLatLongAlt.latitude)}N {decimalParaGrausMinutosSegundos(coordenadaLatLongAlt.longitude)}E {Math.Round(coordenadaLatLongAlt.altitude, 3)} h(m)";
                txtResultadosXYZLatLong.Text = stringResultado;
            }
            else
            {
                txtResultadosXYZLatLong.Text = "Verifique o preenchimento dos dados";
            }
        }
    }
}
