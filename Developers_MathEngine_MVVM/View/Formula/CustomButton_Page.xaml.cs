using Developers_MathEngine_MVVM.Dependency_Injection;
using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Project;
using Developers_MathEngine_MVVM.ViewModel.Formula;
using Microsoft.UI.Input;
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
using Windows.System;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Developers_MathEngine_MVVM.View.Formula
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomButton_Page : Page
    {
        CustomButton_ViewModel customButton_ViewModel;
        public CustomButton_Page()
        {
            InitializeComponent();
            customButton_ViewModel = DependencyInjectionShared.customButton_ViewModel;
        }

        private void Tab0_Button_Click(object sender, RoutedEventArgs e)
        {
            CutomButtonTabDisplay_1.Visibility = Visibility.Visible;
            CutomButtonTabDisplay_2.Visibility = Visibility.Collapsed;
        }

        private void Tab1_Button_Click(object sender, RoutedEventArgs e)
        {

            CutomButtonTabDisplay_2.Visibility = Visibility.Visible;
        }

        private void CustomButton_Button_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //display button description
            Button btn = (Button)sender;
            CoreVirtualKeyStates ctrlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
            bool isCtrlDown = ctrlState.HasFlag(CoreVirtualKeyStates.Down);
            if (!isCtrlDown && e.GetCurrentPoint((UIElement)sender).Properties.IsRightButtonPressed)
            {
                customButton_ViewModel.getButtonDescription_ThroughSelectCommand.Execute(btn.CommandParameter);
            }
            //delete button
            else
            {

                if (isCtrlDown)
                {
                    customButton_ViewModel.deleteCustomButtonCommand.Execute(btn.CommandParameter);
                }
            }
        }

        private void Tab_Button_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Button btn = (Button)sender;
            CoreVirtualKeyStates ctrlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
            bool isCtrlDown = ctrlState.HasFlag(CoreVirtualKeyStates.Down);

            if (isCtrlDown)
            {
                if (((CustomButtonTab)btn.CommandParameter).row ==0)
                {
                    CutomButtonTabDisplay_1.Visibility = Visibility.Collapsed;
                    CutomButtonTabDisplay_2.Visibility = Visibility.Collapsed;
                }
                else if (((CustomButtonTab)btn.CommandParameter).row ==1)
                {
                    CutomButtonTabDisplay_2.Visibility = Visibility.Collapsed;
                }
                customButton_ViewModel.deleteCustomButtonTabCommand.Execute(btn.CommandParameter);
            }
        }
    }
}
