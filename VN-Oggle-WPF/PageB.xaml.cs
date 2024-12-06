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
using System.Windows.Threading;

namespace WPFOggle
{
    /// <summary>
    /// Interaction logic for PageB.xaml
    /// </summary>
    public partial class PageB : Page
    {
        MainWindow window = (MainWindow)Application.Current.MainWindow;
   
        public PageB()
        {
            InitializeComponent();
            SetUp();
        }
        private void SetUp()
        {
            PlayerName.Text = window.player.Name;
            PlayerScore.Text = window.player.Points.ToString() + " Points";
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("PageA.xaml", UriKind.Relative));
        }
    }
}
