using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Labels;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;

using SOG_SharedUtils;

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SOG_CreateBatchJob
{
    public class AddinService
    {
        public static void Launch()
        {
            MetaModelUtils.Init();
            MetaModelUtils.CheckThrowAllowedProjects();

            var window = new SOG_OkCancelWindow("SOG_CreateBatchJob Dialog");

            var nameTextBox = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 8),
            };
            window.AddWithLabel("Technical name of batch:", nameTextBox);

            var projectComboBox = new ComboBox
            {
                Margin = new Thickness(0, 0, 0, 8),
                ItemsSource = MetaModelUtils.Projects.Select(x => x.OAVSProject.Name),
                SelectedIndex = 0,
            };
            window.AddWithLabel("Select a project to add batch to:", projectComboBox);

            window.DialogResultFunc = () =>
            !string.IsNullOrEmpty(nameTextBox.Text) &&
            projectComboBox.SelectedItem != null;

            if (window.ShowDialog() == false) return;

            var selectedProject = MetaModelUtils.Projects.First(x => x.OAVSProject.Name == projectComboBox.SelectedItem as string);

            var labelId = nameTextBox.Text;
            var labelComment = selectedProject.VSProjectNode.Name;
            var labelFileName = "";
            var labelText = "";

            if (selectedProject.ModelInfo.HasLabels)
            {
                window = new SOG_OkCancelWindow("SOG_CreateForm Dialog");

                var labelFileIdComboBox = new ComboBox
                {
                    Margin = new Thickness(0, 0, 0, 8),
                    ItemsSource = selectedProject.ModelInfo.LabelFiles.Select(x => x.LabelFileId),
                    SelectedIndex = 0,
                };
                window.AddWithLabel("Select a label file:", labelFileIdComboBox);

                var labelIdTextBox = new TextBox
                {
                    Margin = new Thickness(0, 0, 0, 8),
                    Text = labelId,
                };
                window.AddWithLabel("Label Id:", labelIdTextBox);

                var labelCommentTextBox = new TextBox
                {
                    Margin = new Thickness(0, 0, 0, 8),
                    Text = labelComment,
                };
                window.AddWithLabel("Label Description:", labelCommentTextBox);

                window.DialogResultFunc = () =>
                labelFileIdComboBox.SelectedItem != null &&
                !string.IsNullOrEmpty(labelIdTextBox.Text) &&
                !string.IsNullOrEmpty(labelCommentTextBox.Text);

                if (window.ShowDialog() == false) return;

                var selectedLabelFile = selectedProject.ModelInfo.LabelFiles.First(x => x.LabelFileId == labelFileIdComboBox.SelectedItem as string);

                labelId = labelIdTextBox.Text;
                labelComment = labelCommentTextBox.Text;
                labelFileName = $"@{selectedLabelFile.LabelFileId}:";

                foreach (var sogLabelLanguage in selectedLabelFile.Languages)
                {
                    window = new SOG_OkCancelWindow("SOG_CreateBatchJob Dialog");

                    window.AddWithLabel("Label Id and Language:", new TextBox
                    {
                        Margin = new Thickness(0, 0, 0, 8),
                        Text = labelId + " " + sogLabelLanguage.Language,
                        IsReadOnly = true,
                    });

                    var labelTextTextBox = new TextBox
                    {
                        Margin = new Thickness(0, 0, 0, 8),
                        Text = labelText,
                    };
                    window.AddWithLabel("Label Text:", labelTextTextBox);

                    window.DialogResultFunc = () => !string.IsNullOrEmpty(labelTextTextBox.Text);

                    if (window.ShowDialog() == false) return;

                    labelText = labelTextTextBox.Text;

                    sogLabelLanguage.Labels.Add(new MetaModelUtils.SOG_D365Label(labelId, labelText, labelComment));
                    sogLabelLanguage.Labels.Add(new MetaModelUtils.SOG_D365Label(labelId + "Maintain", labelText + " (delete)", labelComment));
                }

                selectedLabelFile.Languages.ForEach(x => x.InsertLabelsIfNotExists(selectedProject));
            }

            var contractClass = new AxClass()
            {
                Name = nameTextBox.Text + "Contract",
                SourceCode = new AxPropertyCollection()
                {
                    Declaration =
                        "[DataContract]\r\n" +
                        "//[SysOperationAlwaysInitialize]\r\n" +
                        "public class " + nameTextBox.Text + "Contract //implements SysOperationInitializable, SysOperationValidatable\r\n" +
                        "{\r\n" +
                        "    //private RecId recId;\r\n\r\n" +
                        "    //[DataMember, SysOperationControlVisibility(true)]\r\n" +
                        "    //public RecId parmRecId(RecId _recId = recId)\r\n" +
                        "    //{\r\n" +
                        "    //    recId = _recId;\r\n" +
                        "    //    return recId;\r\n" +
                        "    //}\r\n\r\n" +
                        "    //public void initialize()\r\n" +
                        "    //{\r\n" +
                        "    //}\r\n\r\n" +
                        "    //public boolean validate()\r\n" +
                        "    //{\r\n" +
                        "    //    boolean isValid = true;\r\n\r\n" +
                        "    //    if (!recId)\r\n" +
                        "    //    {\r\n" +
                        "    //        error(strFmt(\"@SYS84378\", extendedTypeId2pname(extendedTypeNum(RecId))));\r\n" +
                        "    //        isValid = false;\r\n" +
                        "    //    }\r\n\r\n" +
                        "    //    return isValid;\r\n" +
                        "    //}\r\n" +
                        "}",
                },
            };
            var serviceClass = new AxClass()
            {
                Name = nameTextBox.Text + "Service",
                SourceCode = new AxPropertyCollection()
                {
                    Declaration = "public class " + nameTextBox.Text + "Service extends SysOperationServiceBase\r\n{\r\n}",
                },
                Methods = new Microsoft.Dynamics.AX.Metadata.Core.Collections.KeyedObjectCollection<AxMethod>
                {
                    new AxMethod()
                    {
                        Name = "process",
                        Source =
                            "    public void process(" + contractClass.Name + " _contract)\r\n" +
                            "    {\r\n" +
                            "        // 1 - Checking missing fields\r\n" +
                            "        //CustParameters parameters = CustParameters::find();\r\n\r\n" +
                            "        //if (!parameters.DefaultCust)\r\n" +
                            "        //{\r\n" +
                            "        //    throw error(strFmt(\"@SYS86936\", tableId2PName(parameters.TableId)));\r\n" +
                            "        //}\r\n\r\n" +
                            "        // 2 - Checking missing record\r\n" +
                            "        //CustTable record;\r\n" +
                            "        //LedgerJournalAC accountNumber = record.AccountNum;\r\n\r\n" +
                            "        //if (!record)\r\n" +
                            "        //{\r\n" +
                            "        //    throw error(strFmt(\"@SYS76877\", strFmt(\"@SYS76498\", tableId2PName(record.TableId), accountNumber)));\r\n" +
                            "        //}\r\n\r\n" +
                            "        // 3 - Success\r\n" +
                            "        //info(\"@SYS80122\");\r\n\r\n" +
                            "        //----------------------------\r\n" +
                            "        // Multi Thread Sample\r\n" +
                            "        //----------------------------\r\n\r\n" +
                            "        //BatchHeader header = this.getCurrentBatchHeader();\r\n\r\n" +
                            "        //if (header) // not null if launched in batch or controller returns true in mustGoBatch\r\n" +
                            "        //{\r\n" +
                            "        //    " + nameTextBox.Text + "Controller controller;\r\n\r\n" +
                            "        //    controller = " + nameTextBox.Text + "Controller::construct();\r\n\r\n" +
                            "        //    controller.parmDialogCaption();\r\n\r\n" +
                            "        //    header.addTask(controller);\r\n\r\n" +
                            "        //    header.save();\r\n" +
                            "        //}\r\n" +
                            "    }"
                    }
                }
            };
            var controllerClass = new AxClass()
            {
                Name = nameTextBox.Text + "Controller",
                SourceCode = new AxPropertyCollection()
                {
                    Declaration = "public class " + nameTextBox.Text + "Controller extends SysOperationServiceController\r\n{\r\n}",
                },
                Methods = new Microsoft.Dynamics.AX.Metadata.Core.Collections.KeyedObjectCollection<AxMethod>
                {
                    new AxMethod()
                    {
                        Name = "new",
                        Source =
                            "    protected void new(SysOperationExecutionMode _executionMode = SysOperationExecutionMode::Synchronous)\r\n" +
                            "    {\r\n" +
                            "        super(classStr(" + serviceClass.Name + "),\r\n" +
                            "            methodStr(" + serviceClass.Name + ", process),\r\n" +
                            "            _executionMode);\r\n" +
                            "    }"
                    },
                    new AxMethod()
                    {
                        Name = "defaultCaption",
                        Source =
                            "    public ClassDescription defaultCaption()\r\n" +
                            "    {\r\n" +
                            "        return \"" + labelFileName + labelId + "\";\r\n" +
                            "    }"
                    },
                    new AxMethod()
                    {
                        Name = "construct",
                        Source =
                            "    public static " + nameTextBox.Text + "Controller construct(SysOperationExecutionMode _executionMode = SysOperationExecutionMode::Synchronous)\r\n" +
                            "    {\r\n" +
                            "        " + nameTextBox.Text + "Controller controller;\r\n\r\n" +
                            "        controller = new " + nameTextBox.Text + "Controller(_executionMode);\r\n\r\n" +
                            "        return controller;\r\n" +
                            "    }"
                    },
                    new AxMethod()
                    {
                        Name = "main",
                        Source =
                            "    public static void main(Args _args)\r\n" +
                            "    {\r\n" +
                            "        " + nameTextBox.Text + "Controller controller;\r\n\r\n" +
                            "        if (_args.parmEnumType() == enumNum(SysOperationExecutionMode))\r\n" +
                            "        {\r\n" +
                            "            controller = " + nameTextBox.Text + "Controller::construct(_args.parmEnum());\r\n" +
                            "        }\r\n" +
                            "        else\r\n" +
                            "        {\r\n" +
                            "            controller = " + nameTextBox.Text + "Controller::construct();\r\n" +
                            "        }\r\n" +
                            "        \r\n" +
                            "        // If not set, sets caption to defaultCaption()\r\n" +
                            "        controller.parmDialogCaption();\r\n" +
                            "        controller.parmArgs(_args);\r\n\r\n" +
                            "        controller.startOperation();\r\n" +
                            "    }"
                    },
                }
            };

            var actionMenuItem = new AxMenuItemAction()
            {
                Name = nameTextBox.Text,
                ObjectType = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.MenuItemObjectType.Class,
                Object = controllerClass.Name,
                SubscriberAccessLevel = new Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrant()
                {
                    Read = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow,
                },
                Label = string.IsNullOrEmpty(labelFileName) ? "" : (labelFileName + labelId),
            };

            var maintainPrivilege = new AxSecurityPrivilege()
            {
                Name = nameTextBox.Text + "Maintain",
                EntryPoints = new Microsoft.Dynamics.AX.Metadata.Core.Collections.KeyedObjectCollection<AxSecurityEntryPointReference>
                {
                    new AxSecurityEntryPointReference()
                    {
                        Name = actionMenuItem.Name,
                        ObjectType = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.EntryPointType.MenuItemAction,
                        ObjectName = actionMenuItem.Name,
                        Grant = new Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrant()
                        {
                            Correct = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow,
                            Create = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow,
                            Delete = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow,
                            Read = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow,
                            Update = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow,
                        }
                    }
                },
                Label = string.IsNullOrEmpty(labelFileName) ? "" : (labelFileName + labelId + "Maintain"),
            };

            var modelSaveInfo = new ModelSaveInfo(selectedProject.ModelInfo.AxModelInfo);

            if (MetaModelUtils.MetaModelService.GetClass(contractClass.Name) == null)
            {
                MetaModelUtils.MetaModelService.CreateClass(contractClass, modelSaveInfo);
            }
            if (MetaModelUtils.MetaModelService.GetClass(serviceClass.Name) == null)
            {
                MetaModelUtils.MetaModelService.CreateClass(serviceClass, modelSaveInfo);
            }
            if (MetaModelUtils.MetaModelService.GetClass(controllerClass.Name) == null)
            {
                MetaModelUtils.MetaModelService.CreateClass(controllerClass, modelSaveInfo);
            }
            if (MetaModelUtils.MetaModelService.GetMenuItemAction(actionMenuItem.Name) == null)
            {
                MetaModelUtils.MetaModelService.CreateMenuItemAction(actionMenuItem, modelSaveInfo);
            }
            if (MetaModelUtils.MetaModelService.GetSecurityPrivilege(maintainPrivilege.Name) == null)
            {
                MetaModelUtils.MetaModelService.CreateSecurityPrivilege(maintainPrivilege, modelSaveInfo);
            }

            selectedProject.VSProjectNode.AddModelElementsToProject(new List<MetadataReference>()
            {
                new MetadataReference(contractClass.Name, contractClass.GetType()),
                new MetadataReference(serviceClass.Name, serviceClass.GetType()),
                new MetadataReference(controllerClass.Name, controllerClass.GetType()),
                new MetadataReference(actionMenuItem.Name, actionMenuItem.GetType()),
                new MetadataReference(maintainPrivilege.Name, maintainPrivilege.GetType()),
            });
        }
    }
}
