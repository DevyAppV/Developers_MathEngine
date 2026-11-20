using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Developers_MathEngine_MVVM.Model.Script
{
    public partial class Scripting : ObservableObject
    {
        //exec script
        //global script
        //local script
        [ObservableProperty]
        public partial string ScriptName { get; set; } = "Exec";

        [ObservableProperty]
        public partial string Script {  get; set; }
    }
}
