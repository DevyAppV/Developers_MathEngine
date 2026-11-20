using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Developers_MathEngine_UnitTest.UnitTests
{
    public class UnitTest_Setup
    {
        //delete_For test only
        public static void createCustomButtonTab(string tabName)
        {

            MainProject.CustomButtonTab_Collection.Add(new CustomButtonTab() { Name = tabName/*, row = 0 */});
            MainProject.selectedCustomButtonTab = MainProject.CustomButtonTab_Collection.Last();

        }

        public static void CreateSubTab(string subTabName)
        {
            //selectedSubTab_Previous = customButtonsSubTab_Collection.Last();
            MainProject.selectedCustomButtonTab.customButtonsSubTab_Collection.Add(new CustomButtonTab() { Name = subTabName/*, row = this.row + 1 */});
            //MainProject.selectedCustomButtonTab = customButtonsSubTab_Collection.Last();
            //selectedSubTab = customButtonsSubTab_Collection.Last();
        }
        public static void setupFormula() {

            MainProject.formulaProject_Collection.Add(new FormulaProject()
            {
                formula_Collection = new System.Collections.ObjectModel.ObservableCollection<Developers_MathEngine_MVVM.Model.Formula.Formula>() {
                    new Formula(){
                        FormulaName = "Test Function",
                        FormulaExec_Script = new Developers_MathEngine_MVVM.Model.Script.Scripting()
                        {
                            //Script="return solve(abs(-1));"
                            //Script="return solve(1+1);"
                        }
                    }
                }
            });

            createCustomButtonTab("Default");
            CreateSubTab("Algebra");
            MainProject.selectedCustomButtonTab.addSetDefaultCustomButtons_Algebra();
            MainProject.selectedCustomButtonTab.addCustomButtonToDictionary();
            CreateSubTab("Trig");
            MainProject.selectedCustomButtonTab.addSetDefaultCustomButtons_Trig();
            MainProject.selectedCustomButtonTab.addCustomButtonToDictionary();

            //MainProject.selectedFormulaProject = MainProject.formulaProject_Collection.Last();
            //MainProject.selectedFormulaProject.selectedFormula = MainProject.selectedFormulaProject.formula_Collection.Last();
        }
    }
}
