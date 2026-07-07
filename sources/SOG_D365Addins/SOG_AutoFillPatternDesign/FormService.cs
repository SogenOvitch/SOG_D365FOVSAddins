using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.AX.Metadata.Patterns;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using SOG_SharedUtils;
using System;
using System.Linq;

namespace SOG_AutoFillPatternDesign
{
    public class FormService
    {
        private static System.Collections.Generic.Dictionary<string, int> WordsCounts { get; } = new System.Collections.Generic.Dictionary<string, int>();
        private static bool AddOptional { get; set; }

        public static void ApplyPattern(Pattern pattern, bool addOptional = false)
        {
            // Init values
            WordsCounts.Clear();
            AddOptional = addOptional;
            var design = MetaModelUtils.CurrentForm.Design;

            // Remove all Controls
            design.Controls.Clear();

            // Add top level Nodes only
            foreach (var patternSubNode in pattern.Root.SubNodes)
            {
                AddPatternNode(design, patternSubNode);
            }
            // Set Design Pattern
            design.ApplyPattern(pattern);

            // Add Subpatterns of top level Nodes, their Subnodes, recursively
            int index = 0;
            foreach (var patternSubNode in pattern.Root.SubNodes)
            {
                index = AddPatternNodeSubpatterns(design, index, patternSubNode);
            }

            // Update Form
            MetaModelUtils.MetaModelService.UpdateForm(MetaModelUtils.CurrentForm, new ModelSaveInfo(MetaModelUtils.CurrentFormModelInfo));
        }

        private static void AddPatternNode(IFormControlCollection formControlCollection, PatternNode patternNode)
        {
            if (patternNode.RequireOne == false && AddOptional == false)
            {
                return;
            }

            if (patternNode.IsOneOf == true)
            {
                patternNode = patternNode.SubNodes.FirstOrDefault();

                if (patternNode == null)
                {
                    return;
                }
            }

            var childFormControl = GetPatternNodeTypeInstance(patternNode);

            childFormControl.Name = GetUniqueName(childFormControl.GetType(), patternNode.Part);

            formControlCollection.AddControl(childFormControl);

            if (childFormControl is IFormControlCollection childFormControlWithChildren)
            {
                foreach (var patternSubNode in patternNode.SubNodes)
                {
                    AddPatternNode(childFormControlWithChildren, patternSubNode);
                }
            }
        }

        private static int AddPatternNodeSubpatterns(IFormControlCollection formControlCollection, int index, PatternNode patternNode)
        {
            if ((patternNode.RequireOne == false && AddOptional == false) ||
                formControlCollection.ControlCount() <= index)
            {
                return index;
            }

            AxFormControl childFormControl = formControlCollection.GetControl(index);

            if (childFormControl is IFormControlCollection childFormControlWithChildren)
            {
                int childIndex;

                if (patternNode.SubPatterns.Count() == 1 && childFormControl is IPatternable childFormControlPatternable)
                {
                    var patternName = patternNode.SubPatterns.First();

                    var pattern = MetaModelUtils.PatternFactory.AllPatterns
                        .First(x => x.Active == true &&
                            x.Category != "FormPatternGuideline" &&
                            x.Name == patternName);

                    foreach (var patternSubNode in pattern.Root.SubNodes)
                    {
                        AddPatternNode(childFormControlWithChildren, patternSubNode);
                    }

                    childFormControlPatternable.ApplyPattern(pattern);

                    childIndex = 0;
                    foreach (var patternSubNode in pattern.Root.SubNodes)
                    {
                        childIndex = AddPatternNodeSubpatterns(childFormControlWithChildren, childIndex, patternSubNode);
                    }
                }

                childIndex = 0;
                foreach (var patternSubNode in patternNode.SubNodes)
                {
                    childIndex = AddPatternNodeSubpatterns(childFormControlWithChildren, childIndex, patternSubNode);
                }
            }

            return index + 1;
        }

        private static string GetUniqueName(Type patternNodeType, string name)
        {
            string uniqueName = name;

            if (string.IsNullOrEmpty(uniqueName))
            {
                uniqueName = patternNodeType.Name;
            }

            if (WordsCounts.Any(x => x.Key == uniqueName))
            {
                WordsCounts[uniqueName]++;

                uniqueName += WordsCounts[uniqueName];
            }
            else
            {
                WordsCounts.Add(uniqueName, 0);
            }

            return uniqueName;
        }

        private static AxFormControl GetPatternNodeTypeInstance(PatternNode patternNode)
        {
            AxFormControl ret;

            if (patternNode.Type == "Group")
            {
                ret = new AxFormGroupControl();
            }
            else if (patternNode.Type == "Grid")
            {
                ret = new AxFormGridControl();
            }
            else if (patternNode.Type == "ActionPane")
            {
                ret = new AxFormActionPaneControl();
            }
            else if (patternNode.Type == "Tab")
            {
                ret = new AxFormTabControl();
            }
            else if (patternNode.Type == "TabPage")
            {
                ret = new AxFormTabPageControl();
            }
            else if (patternNode.Type == "Button" ||
                patternNode.Type == "$Button")
            {
                ret = new AxFormButtonControl();
            }
            else if (patternNode.Type == "CommandButton")
            {
                ret = new AxFormCommandButtonControl();
            }
            else if (patternNode.Type == "DropDialogButton")
            {
                ret = new AxFormDropDialogButtonControl();
            }
            else if (patternNode.Type == "MenuButton")
            {
                ret = new AxFormMenuButtonControl();
            }
            else if (patternNode.Type == "MenuFunctionButton" ||
                patternNode.Type == "TileButtonControl")
            {
                ret = new AxFormMenuFunctionButtonControl();
            }
            else if (patternNode.Type == "RadioButton")
            {
                ret = new AxFormRadioButtonControl();
            }
            else if (patternNode.Type == "ActionPaneTab")
            {
                ret = new AxFormActionPaneTabControl();
            }
            else if (patternNode.Type == "ButtonGroup")
            {
                ret = new AxFormButtonGroupControl();
            }
            else if (patternNode.Type == "Container" ||
                patternNode.Type == "SysChart" ||
                patternNode.Type == "FormPartControl")
            {
                ret = new AxFormContainerControl();
            }
            else if (patternNode.Type == "ListBox")
            {
                ret = new AxFormListBoxControl();
            }
            else if (patternNode.Type == "ListView")
            {
                ret = new AxFormListViewControl();
            }
            else if (patternNode.Type == "StaticText")
            {
                ret = new AxFormStaticTextControl();
            }
            else if (patternNode.Type == "Tree")
            {
                ret = new AxFormTreeControl();
            }
            else if (patternNode.Type == "String" ||
                patternNode.Type == "$Field")
            {
                ret = new AxFormStringControl();
            }
            else if (patternNode.Type == "Image")
            {
                ret = new AxFormImageControl();
            }
            else
            {
                ret = new AxFormControl();
            }

            if (patternNode.Type == "QuickFilterControl" ||
                patternNode.Type == "TileButtonControl" ||
                patternNode.Type == "SysChart" ||
                patternNode.Type == "FormPartControl" ||
                patternNode.Type == "PowerBIControl")
            {
                ret.FormControlExtension = new AxFormControlExtension()
                {
                    Name = patternNode.Type
                };
            }

            return ret;
        }
    }
}
