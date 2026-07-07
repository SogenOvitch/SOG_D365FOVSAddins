using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using SOG_SharedUtils;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;

namespace SOG_AutoFillPatternDesign
{
    /// <summary>
    /// TODO: Say a few words about what your AddIn is going to do
    /// </summary>
    [Export(typeof(IDesignerMenu))]
    // TODO: This addin will show when user right clicks on a form root node or table root node. 
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IForm))]
    public class DesignerContextMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SOG_AutoFillPatternDesign";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.DesignerAddinCaption;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return DesignerContextMenuAddIn.addinName;
            }
        }
        #endregion

        #region Callbacks
        /// <summary>
        /// Called when user clicks on the add-in menu
        /// </summary>
        /// <param name="e">The context of the VS tools and metadata</param>
        public override void OnClick(AddinDesignerEventArgs e)
        {
            try
            {
                MetaModelUtils.InitFromDesigner(e);
                MetaModelUtils.CheckThrowAllowedForm();

                var patterns = MetaModelUtils.PatternFactory.AllPatterns
                    .Where(x => x.Active == true &&
                        x.Category == "FormPatternGuideline")
                    .GroupBy(x => x.FriendlyName)
                    .Select(x => x.First());

                var dialog = new PatternPickerDialog(patterns.Select(x => x.FriendlyName),
                    MetaModelUtils.CurrentForm.Design.Pattern == null ? null :
                    patterns.FirstOrDefault(x => x.Name == MetaModelUtils.CurrentForm.Design.Pattern)?.FriendlyName);

                if (dialog.ShowDialog() != true) return;

                var pattern = MetaModelUtils.PatternFactory.AllPatterns
                    .First(x => x.Active == true &&
                        x.Category == "FormPatternGuideline" &&
                        x.FriendlyName == dialog.SelectedPattern);

                FormService.ApplyPattern(pattern, dialog.AddOptional);
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
        #endregion
    }
}