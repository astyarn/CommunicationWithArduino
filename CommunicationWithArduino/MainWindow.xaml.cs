using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace CommunicationWithArduino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;

        public MainWindow()
        {
            InitializeComponent();

            vm = new ViewModel();
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.StartSerialConnection(); //Setup/close serial connection through button on WPF
        }

        private void setTempViaSerial(object sender, RoutedEventArgs e)
        {
            vm.SetTempOnArduino(); //Set wanted Temperature through button on WPF
        }
    }
}
