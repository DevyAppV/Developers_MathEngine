using Developers_MathEngine.Templates;
using Developers_MathEngine.View;
using Developers_MathEngine.View.Formula;
using Developers_MathEngine.View.Template_View;
using Microsoft.Extensions.DependencyInjection;
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

namespace Developers_MathEngine
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static MainTemplate_Page? mainTemplate_Page;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Frame_Loaded(object sender, RoutedEventArgs e)
        {
            mainTemplate_Page = App.Services?.GetRequiredService<MainTemplate_Page>();
            PageViewer_Frame.Content = mainTemplate_Page;
            //PageViewer_Frame.Navigate(typeof(MainTemplate_Page));
        }

        private void NewWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            App.window_List?.Add(new MainWindow());
            App.window_List?.Last().Activate();
        }

        private void PresetFormula_Button_Click(object sender, RoutedEventArgs e)
        {
            //App.Services?.GetRequiredService<MainTemplate_Page>();
            Presets preset = new Presets();
            preset.getFormulaCalculator_Template();
        }


        //private void PresetFormula_Button_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
