using Developers_MathEngine_MVVM.Dependency_Injection;
using Developers_MathEngine_MVVM.ViewModel.Formula;
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

namespace Developers_MathEngine_MVVM.View.Formula
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FormulaProject_Page : Page
    {
        public Formula_ViewModel formula_ViewModel;
        public FormulaProject_Page()
        {
            InitializeComponent();
            formula_ViewModel = DependencyInjectionShared.formula_ViewModel;
        }

    }
}
