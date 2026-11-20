using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Developers_MathEngine_MVVM.Model.Project
{
    public partial class FormulaProject : ObservableObject
    {
        [ObservableProperty]
        public partial string ProjectName { get; set; }
        [ObservableProperty]
        public partial string ProjectIndex { get; set; }
        public ObservableCollection<Formula.Formula> formula_Collection = new ObservableCollection<Formula.Formula>();

        public Formula.Formula selectedFormula = null;
    }
}
