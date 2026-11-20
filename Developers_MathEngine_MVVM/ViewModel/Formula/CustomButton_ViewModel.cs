using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Developers_MathEngine_MVVM.Dependency_Injection;
using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Project;
using Developers_MathEngine_MVVM.View;
using Developers_MathEngine_MVVM.View.Formula;
using Developers_MathEngine_MVVM.View.Template;
using Developers_MathEngine_MVVM.WindowHandle;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Windows.System;
using Windows.UI.Core;

namespace Developers_MathEngine_MVVM.ViewModel.Formula
{
    public partial class CustomButton_ViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial ObservableCollection<CustomButton> selectedCustomButtonTab_ToDisplayButtons { get; set; }
        //[ObservableProperty]
        //public partial ObservableCollection<CustomButtonTab> selectedCustomButtonTab_ToDisplaySubTab0 { get; set; } = new ObservableCollection<CustomButtonTab>();

        [ObservableProperty]
        public partial ObservableCollection<CustomButtonTab> selectedCustomButtonTab_ToDisplaySubTab1 { get; set; } = new ObservableCollection<CustomButtonTab>();

        [ObservableProperty]
        public partial ObservableCollection<CustomButtonTab> selectedCustomButtonTab_ToDisplaySubTab2 { get; set; } = new ObservableCollection<CustomButtonTab>();
        [ObservableProperty]
        public partial CustomButtonTab selectedCustomButtonTab { get; set; }

        [ObservableProperty]
        public partial CustomButton selectedCustomButton { get; set; }



        [RelayCommand]
        private void selectTab(CustomButtonTab customButtonTab)
        {
            selectedCustomButtonTab = customButtonTab;
            selectedCustomButtonTab_ToDisplayButtons = customButtonTab.customButtons_Collection;
            if (customButtonTab.row == 0)
            {
                selectedCustomButtonTab_ToDisplaySubTab1 = customButtonTab.customButtonsSubTab_Collection;


            }
            else if (customButtonTab.row == 1)
            {
                selectedCustomButtonTab_ToDisplaySubTab2 = customButtonTab.customButtonsSubTab_Collection;

            }
        }

        [RelayCommand]
        private void selectButton(CustomButton customButton)
        {
            if (DependencyInjectionShared.formula_ViewModel.selectedTextBox != null && customButton.Use != null)
            {
                DependencyInjectionShared.formula_ViewModel.selectedTextBox.Select(
                            DependencyInjectionShared.formula_ViewModel.selectedTextBox.SelectionStart,
                            DependencyInjectionShared.formula_ViewModel.selectedTextBox.SelectionLength);

                DependencyInjectionShared.formula_ViewModel.selectedTextBox.SelectedText = customButton.Use;
                DependencyInjectionShared.formula_ViewModel.selectedTextBox.SelectionStart =
                    DependencyInjectionShared.formula_ViewModel.selectedTextBox.SelectionStart + customButton.Use.Length;

                DependencyInjectionShared.formula_ViewModel.selectedTextBox.Select(
                    DependencyInjectionShared.formula_ViewModel.selectedTextBox.SelectionStart, 0);
            }

        }

        [RelayCommand]
        private async Task createTab(string tabName_Textbox_Value)
        {
            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            await Task.Run(() =>
            {
                bool exist = false;
                foreach (var row0 in MainProject.CustomButtonTab_Collection)
                {
                    exist = (row0.Name.Equals(tabName_Textbox_Value)) ? true : false;
                    if (exist) break;
                    foreach (var row1 in row0.customButtonsSubTab_Collection)
                    {

                        exist = (row1.Name.Equals(tabName_Textbox_Value)) ? true : false;

                        if (exist) break;
                        foreach (var row2 in row1.customButtonsSubTab_Collection)
                        {
                            exist = (row2.Name.Equals(tabName_Textbox_Value)) ? true : false;

                            if (exist) break;

                        }
                        if (exist) break;
                    }
                    if (exist) break;

                }
                if (!exist)
                {
                    dispatcherQueue.TryEnqueue(() =>
                    {

                        if (!string.IsNullOrEmpty(tabName_Textbox_Value))
                        {
                            MainProject.CustomButtonTab_Collection.Add(new CustomButtonTab() { Name = tabName_Textbox_Value, SubTabName = "" });
                        }
                    });
                }
            });

        }

        [RelayCommand]
        private async Task createSubTab(string tabName_Textbox_Value)
        {
            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            await Task.Run(() =>
            {
                bool exist = false;
                foreach (var row0 in MainProject.CustomButtonTab_Collection)
                {
                    exist = (row0.Name.Equals(tabName_Textbox_Value)) ? true : false;
                    if (exist) break;
                    foreach (var row1 in row0.customButtonsSubTab_Collection)
                    {

                        exist = (row1.Name.Equals(tabName_Textbox_Value)) ? true : false;

                        if (exist) break;
                        foreach (var row2 in row1.customButtonsSubTab_Collection)
                        {
                            exist = (row2.Name.Equals(tabName_Textbox_Value)) ? true : false;

                            if (exist) break;

                        }
                        if (exist) break;
                    }
                    if (exist) break;

                }
                if (!exist)
                {


                    dispatcherQueue.TryEnqueue(() =>
                    {



                        if (!string.IsNullOrEmpty(tabName_Textbox_Value) && selectedCustomButtonTab != null)
                        {

                            if (selectedCustomButtonTab.row == 0)
                            {
                                selectedCustomButtonTab.customButtonsSubTab_Collection.Add(new CustomButtonTab() { Name = tabName_Textbox_Value, SubTabName= selectedCustomButtonTab.Name, row = selectedCustomButtonTab.row + 1 });

                                selectedCustomButtonTab_ToDisplaySubTab1 = new ObservableCollection<CustomButtonTab>(selectedCustomButtonTab.customButtonsSubTab_Collection);

                            }
                            else if (selectedCustomButtonTab.row == 1)
                            {
                                selectedCustomButtonTab.customButtonsSubTab_Collection.Add(new CustomButtonTab() { Name = tabName_Textbox_Value, SubTabName = selectedCustomButtonTab.Name, row = selectedCustomButtonTab.row + 1 });

                                selectedCustomButtonTab_ToDisplaySubTab2 = new ObservableCollection<CustomButtonTab>(selectedCustomButtonTab.customButtonsSubTab_Collection);

                            }
                        }
                    });
                }
            });
        }

        [RelayCommand]
        private void getButtonDescription_ThroughSearch(string buttonName_Textbox_Value)
        {
            if (CustomButtonTab.customButtons_Dictionary.ContainsKey(buttonName_Textbox_Value))
            {
                WindowHandler.window_List.Add(new View.MainWindow());

                WindowHandler.window_List?.Last().Activate();
                View.MainWindow mainWindow = (View.MainWindow)WindowHandler.window_List.Last();
                mainWindow.PageViewer_Frame.Navigate(typeof(CustomButtonDescription_Template));
                selectedCustomButton = CustomButtonTab.customButtons_Dictionary[buttonName_Textbox_Value];
            }
        }

        [RelayCommand]
        private void getButtonDescription_ThroughSelect(CustomButton selectedButton)
        {
            if (CustomButtonTab.customButtons_Dictionary.ContainsKey(selectedButton.FormulaName))
            {
                WindowHandler.window_List.Add(new View.MainWindow());

                WindowHandler.window_List?.Last().Activate();
                View.MainWindow mainWindow = (View.MainWindow)WindowHandler.window_List.Last();
                selectedCustomButton = selectedButton;
                mainWindow.PageViewer_Frame.Navigate(typeof(CustomButtonDescription_Page));
            }
        }

        [RelayCommand]
        private void deleteCustomButton(CustomButton selectedButton)
        {
            if (CustomButtonTab.customButtons_Dictionary.ContainsKey(selectedButton.FormulaName))
            {
                CustomButtonTab.customButtons_Dictionary.Remove(selectedButton.FormulaName);
                selectedCustomButtonTab.customButtons_Collection.Remove(selectedButton);
            }
        }

        [RelayCommand]
        private void deleteCustomButtonTab(CustomButtonTab selectCustomButtonTab)
        {
            Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
            {
                bool clearDisplayButtons = false;
                if (MainProject.CustomButtonTab_Collection.Contains(selectCustomButtonTab))
                {
                    var removeLinkedCollection = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x == selectCustomButtonTab);

                    if (removeLinkedCollection.customButtons_Collection.Equals( selectedCustomButtonTab_ToDisplayButtons))
                    {
                        clearDisplayButtons = true;
                    }
                    deleteCustomButtonDeleteAllDictionary(removeLinkedCollection);
                    
                    MainProject.CustomButtonTab_Collection.Remove(selectCustomButtonTab);
                }
                else if (selectedCustomButtonTab_ToDisplaySubTab1.Contains(selectCustomButtonTab))
                {
                    var removeLinkedCollection = selectedCustomButtonTab_ToDisplaySubTab1.FirstOrDefault(x => x == selectCustomButtonTab);

                    if (removeLinkedCollection.customButtons_Collection.Equals(selectedCustomButtonTab_ToDisplayButtons))
                    {
                        clearDisplayButtons = true;
                    }
                    deleteCustomButtonDeleteAllDictionary(removeLinkedCollection);

                    selectedCustomButtonTab_ToDisplaySubTab1.Remove(selectCustomButtonTab);

                }
                else if (selectedCustomButtonTab_ToDisplaySubTab2.Contains(selectCustomButtonTab))
                {

                    var removeLinkedCollection = selectedCustomButtonTab_ToDisplaySubTab2.FirstOrDefault(x => x == selectCustomButtonTab);

                    if (removeLinkedCollection.customButtons_Collection.Equals(selectedCustomButtonTab_ToDisplayButtons))
                    {
                        clearDisplayButtons = true;
                    }
                    deleteCustomButtonDeleteAllDictionary(removeLinkedCollection);
                    selectedCustomButtonTab_ToDisplaySubTab2.Remove(selectCustomButtonTab);
                }

                if (clearDisplayButtons)
                {
                    selectedCustomButtonTab_ToDisplayButtons.Clear();
                }
            });
        }


        private void deleteCustomButtonDeleteAllDictionary(CustomButtonTab customButtonTab)
        {
            if (customButtonTab == null) return;
            foreach (var c in customButtonTab.customButtonsSubTab_Collection)
            {
                deleteCustomButtonDeleteAllDictionary(c);
            }
            foreach (var button in customButtonTab.customButtons_Collection)
            {
                if (CustomButtonTab.customButtons_Dictionary.ContainsKey(button.FormulaName))
                {
                    CustomButtonTab.customButtons_Dictionary.Remove(button.FormulaName);
                }
            }
            if (customButtonTab.customButtons_Collection.Equals(selectedCustomButtonTab_ToDisplayButtons))
            {
                selectedCustomButtonTab_ToDisplayButtons.Clear();
            }

        }



        [RelayCommand]
        private void updateCustomButton(CustomButton custombutton)
        {
            //MainProject.CustomButtonTab_Collection
            if (custombutton.Variable_Collection.Count > 0)
            {
                custombutton.Use =
                    custombutton.FormulaName + "(";
                int addedVarToUse = 0;
                foreach (Variable v in custombutton.Variable_Collection)
                {
                    if (v.VariableName.Equals(v.VariableValue))
                    {
                        DependencyInjectionShared.customButton_ViewModel.selectedCustomButton.Use += v.VariableName + ",";
                        addedVarToUse++;
                    }
                }

                if (addedVarToUse > 0)
                {
                    int lastComma = DependencyInjectionShared.customButton_ViewModel.selectedCustomButton.Use.LastIndexOf(',');

                    if (lastComma > 0)
                    {
                        DependencyInjectionShared.customButton_ViewModel.selectedCustomButton.Use =
                        DependencyInjectionShared.customButton_ViewModel.selectedCustomButton.Use.Remove(
                            lastComma
                            ).Insert(lastComma, ")");
                    }
                }
                else
                {
                    DependencyInjectionShared.customButton_ViewModel.selectedCustomButton.Use = custombutton.FormulaName;
                }
            }

            if (CustomButtonTab.customButtons_Dictionary.ContainsKey(custombutton.FormulaName))
            {
                CustomButtonTab.customButtons_Dictionary[custombutton.FormulaName] = custombutton;
            }
            else
            {
                CustomButtonTab.customButtons_Dictionary.Add(custombutton.FormulaName, custombutton);
            }

        }
    }
}
