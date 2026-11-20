using Developers_MathEngine_MVVM.ViewModel.Formula;
using Developers_MathEngine_MVVM.ViewModel.MainWindow;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Developers_MathEngine_MVVM.Dependency_Injection
{
    public class DependencyInjectionShared
    {
        public static MainWindow_ViewModel mainWindow_ViewModel = DependencyInjection.Services.GetRequiredService<MainWindow_ViewModel>();
        public static CustomButton_ViewModel customButton_ViewModel = DependencyInjection.Services.GetRequiredService<CustomButton_ViewModel>();
        public static Formula_ViewModel formula_ViewModel = DependencyInjection.Services.GetRequiredService<Formula_ViewModel>();
    }
}
