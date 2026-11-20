using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Project;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
//using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace Developers_MathEngine_MVVM.Engine.Math
{
    public class Variable
    {
        public string Value { get; set; } = "0";
        public string Name { get; set; } = "";


        // Constructor
        //public Variable() { }
        public Variable(string Name, string Value)
        {
            this.Name = Name.Replace(" ", "").Replace("\r","");
            this.Value = Value.Replace(" ", "").Replace("\r", "");
        }

        //public Variable(string input)
        //{
        //    Value = input.Replace(" ", "").Replace("\r", "");
        //}



        //// The implicit operator that handles: CustomString obj = "your string";
        //public static implicit operator Variable(string input)
        //{
        //    // Return null if the input string is null to keep behavior predictable
        //    if (input == null) return null;

        //    return new Variable(input);
        //}

        // Optional: Overriding ToString makes it print nicely when used elsewhere
        public override string ToString()
        {
            return Value ?? string.Empty;
        }
    }
}
