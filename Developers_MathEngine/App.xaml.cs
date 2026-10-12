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
using Microsoft.Extensions.DependencyInjection;
using Developers_MathEngine.View.Formula;
using Developers_MathEngine.View.Template_View;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Developers_MathEngine
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }
        //public static Window? _window;
        public static List<Window>? window_List = new List<Window>();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();

            ServiceCollection serviceCollection = new ServiceCollection();

            // Main
            serviceCollection.AddTransient<MainWindow>();
            serviceCollection.AddTransient<MainTemplate_Page>();

            // Formula
            serviceCollection.AddTransient<FormulaList_Page>();
            serviceCollection.AddTransient<FormulaProjectList_Page>();
            serviceCollection.AddTransient<CustomButtons_Page>();
            serviceCollection.AddTransient<CustomButtonsDescription_Page>();
            serviceCollection.AddTransient<VisualNotes_Page>();

            Services = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //_window = new MainWindow();
            //_window.Activate();
            window_List?.Add(new MainWindow());
            window_List?.Last().Activate();
        }
    }
}
