using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Controls;
using Developers_MathEngine.View.Formula;
using Developers_MathEngine.View.Template_View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI;


namespace Developers_MathEngine.Templates
{
    public class Presets
    {//[ObservableProperty]
        //[RelayCommand]
        //private void SayHello()
        //{
        //    Title = "Hello!";
        //}
        public class PresetTemplate
        {
            public List<GridLength> rows_Pos = new List<GridLength>();
            public List<GridLength> columns_Pos = new List<GridLength>();
        }
        public static PresetTemplate? FormulaCalculator_Template;


        public void getFormulaCalculator_Template()
        {
            if (FormulaCalculator_Template == null)
            {
                FormulaCalculator_Template = new PresetTemplate();
                FormulaCalculator_Template.columns_Pos.Add(new GridLength(1d, GridUnitType.Star));
                FormulaCalculator_Template.columns_Pos.Add(new GridLength(5d));
                FormulaCalculator_Template.columns_Pos.Add(new GridLength(1d, GridUnitType.Star));
                FormulaCalculator_Template.columns_Pos.Add(new GridLength(5d));
                FormulaCalculator_Template.columns_Pos.Add(new GridLength(1d, GridUnitType.Star));
            }

            MainTemplate_Page? MainTemplate_Page = MainWindow.mainTemplate_Page;
                MainTemplate_Page?.Modification_Grid.ColumnDefinitions.Clear();
                MainTemplate_Page?.Modification_Grid.RowDefinitions.Clear();

                foreach (GridLength rowHeight in FormulaCalculator_Template.rows_Pos)
                {
                    MainTemplate_Page?.Modification_Grid.RowDefinitions.Add(new Microsoft.UI.Xaml.Controls.RowDefinition() { Height = rowHeight });
                }

                foreach (GridLength colWidth in FormulaCalculator_Template.columns_Pos)
                {
                    MainTemplate_Page?.Modification_Grid.ColumnDefinitions.Add(new Microsoft.UI.Xaml.Controls.ColumnDefinition() { Width = colWidth });

                }

                List<GridSplitter> gridSplitter_List = new List<GridSplitter>();
                gridSplitter_List.Add(new GridSplitter());
                gridSplitter_List.Last().Background = new SolidColorBrush(Colors.Gray);
                gridSplitter_List.Add(new GridSplitter());
                gridSplitter_List.Last().Background = new SolidColorBrush(Colors.Gray);

                //set border
                Grid.SetColumn(gridSplitter_List[0], 1);
                Grid.SetColumn(gridSplitter_List[1], 3);
                MainTemplate_Page?.Modification_Grid.Children.Add(gridSplitter_List[0]);
                MainTemplate_Page?.Modification_Grid.Children.Add(gridSplitter_List[1]);

                //setFrame

                List<Frame> Frame_List = new List<Frame>();
                Frame_List.Add(new Frame());
                Frame_List.Add(new Frame());
                Frame_List.Add(new Frame());

                Grid.SetColumn(Frame_List[0], 0);
                Grid.SetColumn(Frame_List[1], 2);
                Grid.SetColumn(Frame_List[2], 4);
                MainTemplate_Page?.Modification_Grid.Children.Add(Frame_List[0]);
                MainTemplate_Page?.Modification_Grid.Children.Add(Frame_List[1]);
                MainTemplate_Page?.Modification_Grid.Children.Add(Frame_List[2]);


            Frame_List[0].Content = App.Services?.GetRequiredService<FormulaProjectList_Page>();
            Frame_List[1].Content = App.Services?.GetRequiredService<FormulaList_Page>();
            Frame_List[2].Content = App.Services?.GetRequiredService<CustomButtons_Page>();
            //Frame_List[0].Navigate(typeof(FormulaProjectList_Page));
            //    Frame_List[1].Navigate(typeof(FormulaList_Page));
            //    Frame_List[2].Navigate(typeof(CustomButtons_Page));


        }
    }
}
