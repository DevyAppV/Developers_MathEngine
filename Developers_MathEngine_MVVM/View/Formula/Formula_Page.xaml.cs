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
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Developers_MathEngine_MVVM.View.Formula
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Formula_Page : Page
    {
        public Formula_ViewModel formula_ViewModel;
        public Formula_Page()
        {
            InitializeComponent();
            formula_ViewModel = DependencyInjectionShared.formula_ViewModel;
        }


        private void TextBox_LosingFocus(object sender, RoutedEventArgs e)
        {
            formula_ViewModel.textboxSelectedCommand.Execute((TextBox)sender);
        }

        private void Textbox_BringIntoViewRequested(UIElement sender, BringIntoViewRequestedEventArgs args)
        {
            args.Handled = true;
        }

        private void TabKeyDown_Textbox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Tab)
            {
                if (sender is TextBox textBox)
                {
                    // Get current cursor location or start of selection
                    //int selectionStart = textBox.SelectionStart;

                    // Insert a tab character (or 4 spaces, depending on preference)
                    string tabString = "\t"; // Alternatively use "    " for spaces

                    // Inject the text at the correct position
                    //textBox.Text = textBox.Text.Insert(selectionStart, tabString);
                    textBox.SelectedText = tabString;
                    textBox.SelectionStart = textBox.SelectionStart + tabString.Length;
                    textBox.Select(textBox.SelectionStart, 0);
                    // Reposition the cursor right after the newly inserted tab
                    //textBox.SelectionStart = selectionStart + tabString.Length;

                    // Clear any lingering text selection
                    //textBox.SelectionLength = 0;
                }
                // Mark the event as handled to stop WinUI from shifting control focus
                e.Handled = true;
            }
        }
    }
}
