using Developers_MathEngine_MVVM.View.Formula;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Developers_MathEngine_MVVM.View.Template
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomButtonDescription_Template : Page
    {
        public CustomButtonDescription_Template()
        {
            InitializeComponent();
        }

        private void CustomButtonDescription_Page_Frame_Loaded(object sender, RoutedEventArgs e)
        {
            //CustomButtonDescription_Page_Frame.DispatcherQueue.TryEnqueue(() =>
            //{
                CustomButtonDescription_Page_Frame.Navigate(typeof(CustomButtonDescription_Page));
            //});
        }
    }
}
