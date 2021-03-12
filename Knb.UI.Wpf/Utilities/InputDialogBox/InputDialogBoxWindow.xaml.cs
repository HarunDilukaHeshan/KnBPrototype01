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

namespace Knb.UI.Wpf.Utilities.InputDialogBox
{
    /// <summary>
    /// Interaction logic for InputDialogBoxWindow.xaml
    /// </summary>
    partial class InputDialogBoxWindow : Window
    {
        public string Message { get => TxtB_message.Text; set { TxtB_message.Text = value; } }
        public string InputString { get => Txt_input.Text; set { Txt_input.Text = value; } }
        public InputDialogBoxWindow()
        {
            InitializeComponent();
        }

        public new void Show()
        {
            throw new NotImplementedException();
        }

        private void Btn_ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
