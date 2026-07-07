using EnvDTE;

using Microsoft.Dynamics.AX.Metadata.Core;
using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.AX.Metadata.Modeling;
using Microsoft.Dynamics.AX.Metadata.Modeling.Visitors;
using Microsoft.Dynamics.AX.Metadata.Service;
using Microsoft.Dynamics.AX.Metadata.Storage;
using Microsoft.Dynamics.AX.Metadata.Storage.DiskProviders;
using Microsoft.Dynamics.AX.Metadata.Storage.Runtime;
using Microsoft.Dynamics.Framework.Tools.Core;
using Microsoft.Dynamics.Framework.Tools.Core.Common;
using Microsoft.Dynamics.Framework.Tools.Core.Configuration;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.Labels;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.LabelFiles;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using Microsoft.Dynamics.Framework.Tools.ProjectSupport.Automation;
using Microsoft.Dynamics.Framework.Tools.ProjectSystem;
using Microsoft.Dynamics.Framework.Tools.ProjectSystem.Debugger;
using Microsoft.Dynamics.Framework.Tools.ProjectSystem.Navigation;
using Microsoft.Dynamics.Framework.Tools.ProjectSystem.StartPage;
using Microsoft.Dynamics.Framework.Tools.ProjectSystem.Wizards.LabelFileWizard.View;

using SOG_SharedUtils;

using System;
using System.Collections.Generic;

using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;

using System.Text;
using System.Windows.Shapes;

using static System.Net.Mime.MediaTypeNames;

namespace SOG_TestingAddin
{
    /// <summary>
    /// TODO: Say a few words about what your AddIn is going to do
    /// </summary>
    [Export(typeof(IMainMenu))]
    public class MainMenuAddIn : MainMenuBase
    {
        #region Member variables
        private const string addinName = "SOG_Testing";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.MainMenuAddInCaption;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return MainMenuAddIn.addinName;
            }
        }

        #endregion

        #region Callbacks
        /// <summary>
        /// Called when user clicks on the add-in menu
        /// </summary>
        /// <param name="e">The context of the VS tools and metadata</param>
        public override void OnClick(AddinEventArgs e)
        {
            try
            {
                MetaModelUtils.Init();

                StringBuilder sb = new StringBuilder();

                var vsSolutionProjects = CoreUtility.GetAllProjectsInSolution();

                ModelInfo projectModel = null;
                VSProjectNode vsProject = null;
                Microsoft.Dynamics.Framework.Tools.ProjectSystem.OAVSProject oavsProject = null;

                foreach (var project in vsSolutionProjects)
                {
                    if (project is Microsoft.Dynamics.Framework.Tools.ProjectSystem.OAVSProject)
                    {
                        oavsProject = project as Microsoft.Dynamics.Framework.Tools.ProjectSystem.OAVSProject;
                        vsProject = oavsProject.Project as VSProjectNode;

                        var property = vsProject.GetProjectProperty("Model");

                        projectModel = MetaModelUtils.MetaModelService.GetModel(property);

                        //var testClass = new AxClass()
                        //{
                        //    Name = "SOG_AddinTesting",
                        //    IsPublic = true,
                        //    IsFinal = true,
                        //};

                        //MetaModelUtils.MetaModelService.CreateClass(testClass, new ModelSaveInfo(projectModel));

                        //vsProject.AddModelElementsToProject(new List<MetadataReference>()
                        //{
                        //    new MetadataReference(testClass.Name, testClass.GetType())
                        //});
                    }
                }

                //sb.AppendLine(CoreUtility.GetInstallPath());
                //sb.AppendLine(CoreUtility.GetDynamicsConfigurationFolder());

                //sb.AppendLine("");

                ////Microsoft.Dynamics.Framework.Tools.MetaModel.Core.DesignMetaModelService
                //var DesignMetaModelService = AxServiceProvider.GetService<IDesignMetaModelService>();

                //var diskMetadataProvider = MetadataStorageUtility.FindProvider<IDiskMetadataProvider>(DesignMetaModelService.GetDiskProvider());

                ////Microsoft.Dynamics.AX.Metadata.Storage.DiskProviders.MetadataDiskProvider
                //sb.AppendLine(diskMetadataProvider.GetType().ToString());
                ////Microsoft.Dynamics.AX.Metadata.Storage.DiskProviders.ModelInfoDiskProvider
                //sb.AppendLine(diskMetadataProvider.ModelManifest.GetType().ToString());

                //new Microsoft.Dynamics.Framework.Tools.MetaModel.Core.ExtensibilityService()
                //var metaModelProviders = MetaModelUtils.ServiceProvider.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;

                //sb.AppendLine("");

                if (projectModel != null)
                {
                    //sb.AppendLine(DesignMetaModelService.GetModelStorePathForModel(projectModel));
                    //sb.AppendLine("");

                    var labelFiles = MetaModelUtils.MetadataProvider.LabelFiles.ListObjectsForModel(projectModel.Name);

                    foreach (var labelFile in labelFiles)
                    {
                        var axLabelFile = MetaModelUtils.MetaModelService.GetLabelFile(labelFile);

                        //sb.AppendLine($"{axLabelFile.Name} {axLabelFile.LabelFileId} {axLabelFile.LabelContentFileName}");
                        //sb.AppendLine($"{axLabelFile.Language} {axLabelFile.RelativeUriInModelStore}");
                        //sb.AppendLine($"{axLabelFile.LocalPath()}");

                        //var controller = new LabelControllerFactory()
                        //    .GetOrCreateLabelController(axLabelFile, new VSApplicationContext(MetaModelUtils.ServiceProvider));
                        //if (!controller.Exists("SOG_Z_NewLabel"))
                        //{
                        //    controller.Insert("SOG_Z_NewLabel", "Test1", vsProject.Name);
                        //}
                        //if (!controller.Exists("SOG_A_NewLabel"))
                        //{
                        //    controller.Insert("SOG_A_NewLabel", "Test2", vsProject.Name);
                        //}
                        //controller.Save();

                        //var stream = MetaModelUtils.MetadataProvider.LabelFiles.GetContent(axLabelFile, projectModel);
                        //var stream = new FileStream(axLabelFile.LocalPath(), FileMode.Append, FileAccess.Write);

                        //using (StreamWriter sw = new StreamWriter(stream))
                        //{
                        //    string newLabel = $"SOG_Z_NewLabel=Test\r\n ;{vsProject.Name}";
                        //    sw.WriteLine(newLabel);
                        //    newLabel = $"SOG_A_NewLabel=Test\r\n ;{vsProject.Name}";
                        //    sw.WriteLine(newLabel);
                        //}

                        //using (StreamReader sr = new StreamReader(stream))
                        //{
                        //    var content = sr.ReadToEnd();
                        //    //This allows you to do one Read operation.
                        //    sb.AppendLine(content);
                        //}
                    }
                }

                //SOG_CustomField=Champ spécifique
                // ;SOG_1_Setup

                //var metaModelProviders = MetaModelUtils.ServiceProvider.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;
                //sb.AppendLine($"{metaModelProviders.GetType().FullName}");

                WindowUtils.ShowDebugDialog(sb.ToString());
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
        #endregion
    }
}