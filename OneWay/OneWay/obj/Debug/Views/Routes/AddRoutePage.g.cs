﻿#pragma checksum "..\..\..\..\Views\Routes\AddRoutePage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F8459D90478538B13C4F9E249DCA791AFF85A35AF3CEB55302EB51772EFD7FD2"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using MahApps.Metro.IconPacks.Converter;
using OneWay.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace OneWay.Pages {
    
    
    /// <summary>
    /// AddRoutePage
    /// </summary>
    public partial class AddRoutePage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox StartPoint;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboBoxStartPoint;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Points;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddCarButton;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FinishPoint;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Distance;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboBoxLastPointInfo;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateRouteButton;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Fill;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel TripInfo;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/OneWay;component/views/routes/addroutepage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.StartPoint = ((System.Windows.Controls.TextBox)(target));
            
            #line 33 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
            this.StartPoint.LostFocus += new System.Windows.RoutedEventHandler(this.StartPoint_LostFocus);
            
            #line default
            #line hidden
            return;
            case 2:
            this.comboBoxStartPoint = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.Points = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.AddCarButton = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
            this.AddCarButton.Click += new System.Windows.RoutedEventHandler(this.CreatePointButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.FinishPoint = ((System.Windows.Controls.TextBox)(target));
            
            #line 59 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
            this.FinishPoint.LostFocus += new System.Windows.RoutedEventHandler(this.FinishPoint_LostFocus);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Distance = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.comboBoxLastPointInfo = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.CreateRouteButton = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
            this.CreateRouteButton.Click += new System.Windows.RoutedEventHandler(this.CreateRouteButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.Fill = ((System.Windows.Controls.ComboBox)(target));
            
            #line 80 "..\..\..\..\Views\Routes\AddRoutePage.xaml"
            this.Fill.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Fill_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.TripInfo = ((System.Windows.Controls.StackPanel)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
