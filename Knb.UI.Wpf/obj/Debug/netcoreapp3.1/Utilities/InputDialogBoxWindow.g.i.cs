﻿#pragma checksum "..\..\..\..\Utilities\InputDialogBoxWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8FAED0F57EA03D357D30EE9D3B18CA7469D31750"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Knb.UI.Wpf.Utilities;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Knb.UI.Wpf.Utilities {
    
    
    /// <summary>
    /// InputDialogBoxWindow
    /// </summary>
    partial class InputDialogBoxWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Utilities\InputDialogBoxWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TxtB_message;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\..\Utilities\InputDialogBoxWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Txt_input;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Utilities\InputDialogBoxWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_ok;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\Utilities\InputDialogBoxWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_cancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Knb.UI.Wpf;V1.0.0.0;component/utilities/inputdialogboxwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Utilities\InputDialogBoxWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.TxtB_message = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.Txt_input = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.Btn_ok = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\..\Utilities\InputDialogBoxWindow.xaml"
            this.Btn_ok.Click += new System.Windows.RoutedEventHandler(this.Btn_ok_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Btn_cancel = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\..\Utilities\InputDialogBoxWindow.xaml"
            this.Btn_cancel.Click += new System.Windows.RoutedEventHandler(this.Btn_cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

