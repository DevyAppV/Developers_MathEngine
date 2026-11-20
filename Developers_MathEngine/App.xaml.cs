using Developers_MathEngine_MVVM.Dependency_Injection;
using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Project;
using Developers_MathEngine_MVVM.View;
using Developers_MathEngine_MVVM.ViewModel.Formula;
using Developers_MathEngine_MVVM.ViewModel.MainWindow;
using Developers_MathEngine_MVVM.WindowHandle;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Developers_MathEngine
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        //public static IServiceProvider? Services { get; private set; }
        //public static Window? _window;
        //public static List<Window>? window_List = new List<Window>();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {

            InitializeComponent();
            ServiceCollection serviceCollection = new ServiceCollection();

            // Main
            //serviceCollection.AddTransient<MainWindow>();
            //serviceCollection.AddTransient<MainTemplate_Page>();

            // Formula
            //serviceCollection.AddTransient<SelectedProjectName>();
            serviceCollection.AddTransient<Formula_ViewModel>();
            serviceCollection.AddTransient<CustomButton_ViewModel>();
            serviceCollection.AddTransient<MainWindow_ViewModel>();
            //serviceCollection.AddTransient<MainProject>();
            //serviceCollection.AddTransient<FormulaList_Page>();
            //serviceCollection.AddTransient<FormulaProjectList_Page>();
            //serviceCollection.AddTransient<CustomButtons_Page>();
            //serviceCollection.AddTransient<CustomButtonsDescription_Page>();
            //serviceCollection.AddTransient<VisualNotes_Page>();

            DependencyInjection.Services = serviceCollection.BuildServiceProvider();
            MainProject.CustomButtonTab_Collection.Add(new CustomButtonTab());
            MainProject.CustomButtonTab_Collection.Last().setUpDefaultCustomButton();

        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //_window = new MainWindow();
            //_window.Activate();
            WindowHandler.window_List?.Add(new MainWindow());
            WindowHandler.window_List?.Last().Activate();
        }
    }
}
