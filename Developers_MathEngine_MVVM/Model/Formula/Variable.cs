using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Developers_MathEngine_MVVM.Model.Formula
{
    public partial class Variable :ObservableObject
    {
        [ObservableProperty]
        public partial string VariableIndex {  get; set; }
        [ObservableProperty]
        public partial string VariableName {  get; set; }
        [ObservableProperty]
        public partial string VariableValue {  get; set; }
        [ObservableProperty]
        public partial string VariableDescription {  get; set; }

    }
}
