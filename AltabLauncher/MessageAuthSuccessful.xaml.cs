using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AltabLauncher
{
    /// <summary>
    /// Interaction logic for MessageAuthSuccessful.xaml
    /// </summary>
    public partial class MessageAuthSuccessful : Window
    {
        public MessageAuthSuccessful()
        {
            InitializeComponent();
        }

        private void MessageAuthSuccessful_ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
