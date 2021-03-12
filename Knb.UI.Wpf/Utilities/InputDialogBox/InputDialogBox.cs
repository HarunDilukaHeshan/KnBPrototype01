using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Knb.UI.Wpf.Utilities.InputDialogBox
{
    public static class InputDialogBox
    {
        public static InputDialogBoxResult Show(string message)
        {
            return Show(message, "");
        }

        public static InputDialogBoxResult Show(string message, string caption)
        {
            var dlgBox = new InputDialogBoxWindow
            {
                Title = caption,
                Message = message
            };

            var result = dlgBox.ShowDialog();

            if (result == true)
            {
                var input = dlgBox.InputString;
                return new InputDialogBoxResult(InputDialogBoxButton.Okay, input);
            }
            else
                return new InputDialogBoxResult(InputDialogBoxButton.Cancel, "");
        }
    }

    public class InputDialogBoxResult
    {
        public InputDialogBoxResult(InputDialogBoxButton clickedBtn, string inputString)
        {
            ClickedBtn = clickedBtn;
            InputString = inputString ?? throw new ArgumentNullException();
        }

        public InputDialogBoxButton ClickedBtn { get; }
        public string InputString { get; }
    }

    public enum InputDialogBoxButton { Okay, Cancel }
}