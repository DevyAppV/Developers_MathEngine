using CommunityToolkit.Mvvm.ComponentModel;
using Developers_MathEngine_MVVM.Model.Formula;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Developers_MathEngine_MVVM.Model.Project
{
    public partial class MainProject
    {

        public static ObservableCollection<FormulaProject> formulaProject_Collection = new ObservableCollection<FormulaProject>();
        //public static FormulaProject selectedFormulaProject = null;
        public static ObservableCollection<CustomButtonTab> CustomButtonTab_Collection = new ObservableCollection<CustomButtonTab>();
        public static CustomButtonTab selectedCustomButtonTab = null;
        public static CustomButton selectedCustomButton_ToView = null;
        //2d project
        //3d project
        //graphing project
        //graph project
        //graph project

    }
}
