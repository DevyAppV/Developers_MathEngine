//using Developers_MathEngine_Library.Interpreter.Errors;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_MVVM.Engine.CoreCLR
{
    public class CoreCLR
    {

        public static async Task<ScriptState<object>> EvaluateScriptAsync(string code, List<string> defaultUsingImports, object CoreCLRApiScript_Globals = null, ScriptState<object> continueWithPrevScript = null)
        {
            try
            {
                // 1. Define base references required for basic C# expressions
                var references = new[]
                {
                    typeof(object).Assembly,                       // mscorlib / System.Private.CoreLib
                    typeof(System.Linq.Enumerable).Assembly,       // System.Core
                    Assembly.GetExecutingAssembly()                // Your WinUI 3 App Assembly
                };

                // 2. Define default imports (usings)
                //var imports = new[] { "System", "System.Linq", "System.Collections.Generic", "Microsoft.UI.Xaml", "System.Text", "System.Threading.Tasks" };

                var options = ScriptOptions.Default
                    .WithReferences(references)
                    .WithImports(defaultUsingImports);

                // 3. Run the script. If you need the script to mutate or read app state, 
                // pass a host object via the 'globals' parameter.
                if (continueWithPrevScript == null)
                {
                    var scriptState = await CSharpScript.RunAsync(code, options, globals: CoreCLRApiScript_Globals);
                    return scriptState;
                }
                else
                {
                    var scriptState = await continueWithPrevScript.ContinueWithAsync(code, options);
                    return scriptState;
                }

            }
            catch (CompilationErrorException ex)
            {
                // Handle syntax errors or compilation failures here
                //System.Diagnostics.Debug.WriteLine(string.Join(Environment.NewLine, ex.Diagnostics));
                //ErrorHandler.errorMessage = string.Join(Environment.NewLine, ex.Diagnostics);
                throw new Exception(string.Join(Environment.NewLine, ex.Diagnostics));
                //throw;
                //return null;
            }
            catch (Exception ex)
            {
                // Handle syntax errors or compilation failures here
                //System.Diagnostics.Debug.WriteLine(string.Join(Environment.NewLine, ex.Message));
                //ErrorHandler.errorMessage = string.Join(Environment.NewLine, ex.Message);
                throw new Exception(string.Join(Environment.NewLine, ex.Message));
                //throw;
                //throw;
                //return null;
            }
            //finally
            //{
            //    if (!string.IsNullOrEmpty(ErrorHandler.errorMessage))
            //    {
            //        throw new Exception(ErrorHandler.errorMessage);
            //    }
            //}
        }

    }
}
