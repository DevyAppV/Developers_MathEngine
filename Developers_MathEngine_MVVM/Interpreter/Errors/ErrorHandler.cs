using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Developers_MathEngine_MVVM.Interpreter.Errors
{
    public class ErrorHandler
    {
        public static int line = -1;
        public static string errorMessage = "";
        public ObservableCollection<string> error_List = new ObservableCollection<string>();
    }
}
