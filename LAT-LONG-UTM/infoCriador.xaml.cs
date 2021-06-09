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
using System.Windows.Shapes;

namespace LAT_LONG_UTM
{
    /// <summary>
    /// Lógica interna para infoCriador.xaml
    /// </summary>
    public partial class infoCriador : Window
    {
        public infoCriador()
        {
            InitializeComponent();
        }

        private void bttCriador_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
    }
}
