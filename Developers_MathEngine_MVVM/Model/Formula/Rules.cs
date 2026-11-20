using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using static Developers_MathEngine_MVVM.Engine.CoreCLR.CoreCLR_API;

namespace Developers_MathEngine_MVVM.Model.Formula
{
    public partial class Rule : ObservableObject
    {
        public ScriptGlobals ScriptGlobals { get; set; }

        [ObservableProperty]
        public partial string ruleIndex { get; set; }
        [ObservableProperty]
        public partial string codeRule { get; set; }

    }
}
