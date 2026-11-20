using Developers_MathEngine_MVVM.Dependency_Injection;
using Developers_MathEngine_MVVM.View.Template;
using Developers_MathEngine_MVVM.ViewModel.MainWindow;
using Developers_MathEngine_MVVM.WindowHandle;
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

namespace Developers_MathEngine_MVVM.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow_ViewModel mainWindow_ViewModel;
        public MainWindow()
        {
            InitializeComponent();
            mainWindow_ViewModel = DependencyInjectionShared.mainWindow_ViewModel;
        }

        private void PageViewer_Frame_Loaded(object sender, RoutedEventArgs e)
        {
            if (PageViewer_Frame.Content == null)
            {
                PageViewer_Frame.Navigate(typeof(Formula_Template));
            }
            //mainTemplate_Page = App.Services?.GetRequiredService<MainTemplate_Page>();
            //PageViewer_Frame.Content = mainTemplate_Page;
            //PageViewer_Frame.Navigate(typeof(MainTemplate_Page));
        }

        private void NewWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            WindowHandler.window_List?.Add(new MainWindow());
            WindowHandler.window_List?.Last().Activate();
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.CodeActivated ||
            args.WindowActivationState == WindowActivationState.PointerActivated)
            {
                WindowHandler.activeWindow = (Window)sender;
            }
        }

        private void Preset_Formula_Button_Click(object sender, RoutedEventArgs e)
        {
            PageViewer_Frame.Navigate(typeof(Formula_Template));
        }

        //private void PresetFormula_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    //App.Services?.GetRequiredService<MainTemplate_Page>();
        //    //Presets preset = new Presets();
        //    //preset.getFormulaCalculator_Template();
        //}

    }
}
