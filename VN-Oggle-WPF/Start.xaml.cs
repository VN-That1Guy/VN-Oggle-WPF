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

namespace WPFOggle
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Page
    {
        MainWindow window = (MainWindow)Application.Current.MainWindow;
        public Start()
        {
            InitializeComponent();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerName.Text != "")
            {
                window.player.Name = PlayerName.Text;
               
            }
            this.NavigationService.Navigate(new Uri("PageA.xaml", UriKind.Relative));
        }
    }
}
