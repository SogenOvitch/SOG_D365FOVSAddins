using Microsoft.Dynamics.AX.Metadata.Core.MetaModel;
using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.AX.Metadata.Modeling.Extensions;
using Microsoft.Dynamics.AX.Metadata.Patterns;
using Microsoft.Dynamics.AX.Metadata.Providers;
using Microsoft.Dynamics.AX.Metadata.Service;
using Microsoft.Dynamics.AX.Metadata.Storage;
using Microsoft.Dynamics.Framework.Tools.Core;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.Labels;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using Microsoft.Dynamics.Framework.Tools.ProjectSystem;
using Microsoft.VisualStudio.Shell;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOG_SharedUtils
{
    public class MetaModelUtils
    {
        public static PatternFactory PatternFactory { get; } = new PatternFactory(includeStandardPatterns: true);

        public static IServiceProvider ServiceProvider { get; private set; }
        public static IMetadataProvider MetadataProvider { get; private set; }
        public static IMetaModelService MetaModelService { get; private set; }

        public static AxForm CurrentForm { get; private set; }
        public static ModelInfo CurrentFormModelInfo { get; private set; }

        public static AxTable CurrentTable { get; private set; }
        public static ModelInfo CurrentTableModelInfo { get; private set; }

        public static List<SOG_D365Project> Projects { get; private set; } = new List<SOG_D365Project>();

        public static void Init()
        {
            if (ServiceProvider != null)
            {
                return;
            }

            ServiceProvider = CoreUtility.ServiceProvider ?? new SOG_AsyncServiceProvider();
            IServiceContainer serviceContainer = (ServiceProvider is IServiceContainer) ? ServiceProvider as IServiceContainer : null;

            var metadataProvider = ServiceProvider.GetService(typeof(IMetadataProvider)) as IMetadataProvider;

            if (metadataProvider == null)
            {
                var metadataPath = @"C:\AOSService\PackagesLocalDirectory";

                if (!Directory.Exists(metadataPath))
                {
                    metadataPath = @"K:\AOSService\PackagesLocalDirectory";
                }

                if (Directory.Exists(metadataPath))
                {
                    metadataProvider = new MetadataProviderFactory()
                        .CreateDiskProvider(metadataPath);
                    serviceContainer?.AddService(typeof(IMetadataProvider), metadataProvider);
                }
            }

            MetadataProvider = metadataProvider;

            var metaModelService = ServiceProvider.GetService(typeof(IMetaModelService)) as IMetaModelService;

            if (metaModelService == null)
            {
                metaModelService = new MetaModelServiceFactory()
                    .Create(metadataProvider);
                serviceContainer?.AddService(typeof(IMetaModelService), metaModelService);
            }

            MetaModelService = metaModelService;

            AxServiceProvider.SetSite(ServiceProvider);

            var projects = CoreUtility.GetAllProjectsInSolution();

            foreach (var project in projects)
            {
                if (project is OAVSProject oavsProject &&
                    oavsProject.Project is VSProjectNode vsProjectNode)
                {
                    var projectModelInfo = vsProjectNode.GetProjectsModelInfo(false);

                    if (projectModelInfo != null &&
                        projectModelInfo.Customization != ModelCustomizationLevel.DoNotAllow &&
                        projectModelInfo.Readonly == false)
                    {
                        var sogProject = new SOG_D365Project
                        {
                            OAVSProject = oavsProject,
                            VSProjectNode = vsProjectNode,
                            ModelInfo = new SOG_D365ModelInfo
                            {
                                AxModelInfo = projectModelInfo,
                            }
                        };

                        var labelFiles = MetadataProvider.LabelFiles.ListObjectsForModel(projectModelInfo.Name);
                        foreach (var labelFile in labelFiles)
                        {
                            var axLabelFile = MetaModelService.GetLabelFile(labelFile);

                            var sogLabelFile = sogProject.ModelInfo.LabelFiles
                                .FirstOrDefault(x => x.LabelFileId == axLabelFile.LabelFileId);

                            if (sogLabelFile != null)
                            {
                                var sogLabelLanguage = sogLabelFile.Languages
                                    .FirstOrDefault(x => x.Language == axLabelFile.Language);

                                if (sogLabelLanguage == null)
                                {
                                    sogLabelFile.Languages.Add(new SOG_D365LabelLanguage
                                    {
                                        AxLabelFile = axLabelFile,
                                    });
                                }
                            }
                            else
                            {
                                sogProject.ModelInfo.LabelFiles.Add(new SOG_D365LabelFile
                                {
                                    Languages = new List<SOG_D365LabelLanguage>
                                    {
                                        new SOG_D365LabelLanguage
                                        {
                                            AxLabelFile = axLabelFile
                                        }
                                    }
                                });
                            }
                        }

                        Projects.Add(sogProject);
                    }
                }
            }
        }

        public static void InitFromDesigner(AddinDesignerEventArgs e)
        {
            if (e.SelectedElement is IForm form)
            {
                InitFromForm(form);
            }
            else if (e.SelectedElement is ITable table)
            {
                InitFromTable(table);
            }
        }

        public static void InitFromForm(IForm form)
        {
            Init();

            CurrentForm = (AxForm)form.GetMetadataType();
            CurrentFormModelInfo = MetaModelService.GetFormModelInfo(CurrentForm.Name).First();
        }

        public static void InitFromTable(ITable table)
        {
            Init();

            CurrentTable = (AxTable)table.GetMetadataType();
            CurrentTableModelInfo = MetaModelService.GetTableModelInfo(CurrentTable.Name).First();
        }

        public static void CheckThrowAllowedForm()
        {
            if (CurrentForm == null || CurrentFormModelInfo == null ||
                CurrentFormModelInfo.Customization == ModelCustomizationLevel.DoNotAllow ||
                CurrentFormModelInfo.Readonly)
            {
                throw new Exception("Form not found or customization not allowed");
            }
        }

        public static void CheckThrowAllowedTable()
        {
            if (CurrentTable == null || CurrentTableModelInfo == null ||
                CurrentTableModelInfo.Customization == ModelCustomizationLevel.DoNotAllow ||
                CurrentTableModelInfo.Readonly)
            {
                throw new Exception("Table not found or customization not allowed");
            }
        }

        public static void CheckThrowAllowedProjects()
        {
            if (Projects.Count == 0)
            {
                throw new Exception("Projects not found or customization not allowed");
            }
        }

        public class SOG_D365Project
        {
            public OAVSProject OAVSProject { get; set; }
            public VSProjectNode VSProjectNode { get; set; }
            public SOG_D365ModelInfo ModelInfo { get; set; }
        }

        public class SOG_D365ModelInfo
        {
            public string Name => AxModelInfo.Name;
            public ModelInfo AxModelInfo { get; set; }

            public bool HasLabels => LabelFiles.Count > 0;
            public List<SOG_D365LabelFile> LabelFiles { get; set; } = new List<SOG_D365LabelFile>();
        }

        public class SOG_D365LabelFile
        {
            public string LabelFileId => Languages.First().LabelFileId;
            public List<SOG_D365LabelLanguage> Languages { get; set; } = new List<SOG_D365LabelLanguage>();

            public void InsertLabelsIfNotExists(SOG_D365Project project = null)
            {
                Languages.ForEach(x => x.InsertLabelsIfNotExists(project));
            }
        }

        public class SOG_D365LabelLanguage
        {
            public string LabelFileId => AxLabelFile.LabelFileId;
            public string Language => AxLabelFile.Language;
            public AxLabelFile AxLabelFile { get; set; }

            public List<SOG_D365Label> Labels { get; set; } = new List<SOG_D365Label>();

            private LabelEditorController controller;

            public LabelEditorController Controller
            {
                get
                {
                    if (controller == null)
                    {
                        controller = new LabelControllerFactory()
                            .GetOrCreateLabelController(AxLabelFile, new VSApplicationContext(ServiceProvider));
                    }
                    return controller;
                }
                set
                {
                    controller = value;
                }
            }

            public void InsertLabelsIfNotExists(SOG_D365Project project = null)
            {
                var save = false;
                var addToProject = false;

                foreach (var label in Labels)
                {
                    if (label.State == LabelState.Insert)
                    {
                        if (!Controller.Exists(label.LabelId))
                        {
                            Controller.Insert(label.LabelId, label.LabelText, label.LabelComment);

                            save = true;
                        }
                        addToProject = true;

                        label.State = LabelState.Read;
                    }
                }

                if (save == true)
                {
                    Controller.Save();
                }
                if (addToProject == true && project != null)
                {
                    project.VSProjectNode.AddModelElementsToProject(new List<MetadataReference>()
                    {
                        new MetadataReference(AxLabelFile.Name, AxLabelFile.GetType()),
                    });
                }
            }
        }

        public class SOG_D365Label
        {
            public string LabelId { get; set; }
            public string LabelText { get; set; }
            public string LabelComment { get; set; }

            public LabelState State { get; set; }

            public SOG_D365Label(string labelId, string labelText, string labelComment, LabelState state = LabelState.Insert)
            {
                LabelId = labelId;
                LabelText = labelText;
                LabelComment = labelComment;
                State = state;
            }
        }
    }
}
