using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Labels;

using SOG_SharedUtils;

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SOG_CreateForm
{
    public class AddinService
    {
        public static void Launch()
        {
            MetaModelUtils.Init();
            MetaModelUtils.CheckThrowAllowedProjects();

            var window = new SOG_OkCancelWindow("SOG_CreateForm Dialog");

            var nameTextBox = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 8),
            };
            window.AddWithLabel("Technical name of form:", nameTextBox);

            var projectComboBox = new ComboBox
            {
                Margin = new Thickness(0, 0, 0, 8),
                ItemsSource = MetaModelUtils.Projects.Select(x => x.OAVSProject.Name),
                SelectedIndex = 0,
            };
            window.AddWithLabel("Select a project to add form to:", projectComboBox);

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
                    window = new SOG_OkCancelWindow("SOG_CreateForm Dialog");

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
                    sogLabelLanguage.Labels.Add(new MetaModelUtils.SOG_D365Label(labelId + "View", labelText + " (read)", labelComment));
                    sogLabelLanguage.Labels.Add(new MetaModelUtils.SOG_D365Label(labelId + "Maintain", labelText + " (delete)", labelComment));
                }

                selectedLabelFile.Languages.ForEach(x => x.InsertLabelsIfNotExists(selectedProject));
            }

            var form = new AxForm()
            {
                Name = nameTextBox.Text,
                Design = new AxFormDesign()
                {
                    Caption = string.IsNullOrEmpty(labelFileName) ? "" : (labelFileName + labelId)
                }
            };

            var displayMenuItem = new AxMenuItemDisplay()
            {
                Name = nameTextBox.Text,
                ObjectType = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.MenuItemObjectType.Form,
                Object = form.Name,
                SubscriberAccessLevel = new Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrant()
                {
                    Read = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow,
                },
                Label = string.IsNullOrEmpty(labelFileName) ? "" : (labelFileName + labelId),
            };

            var viewPrivilege = new AxSecurityPrivilege()
            {
                Name = nameTextBox.Text + "View",
                EntryPoints = new Microsoft.Dynamics.AX.Metadata.Core.Collections.KeyedObjectCollection<AxSecurityEntryPointReference>
                {
                    new AxSecurityEntryPointReference()
                    {
                        Name = displayMenuItem.Name,
                        ObjectType = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.EntryPointType.MenuItemDisplay,
                        ObjectName = displayMenuItem.Name,
                        Grant = new Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrant()
                        {
                            Read = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow,
                        }
                    }
                },
                Label = string.IsNullOrEmpty(labelFileName) ? "" : (labelFileName + labelId + "View"),
            };

            var maintainPrivilege = new AxSecurityPrivilege()
            {
                Name = nameTextBox.Text + "Maintain",
                EntryPoints = new Microsoft.Dynamics.AX.Metadata.Core.Collections.KeyedObjectCollection<AxSecurityEntryPointReference>
                {
                    new AxSecurityEntryPointReference()
                    {
                        Name = displayMenuItem.Name,
                        ObjectType = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.EntryPointType.MenuItemDisplay,
                        ObjectName = displayMenuItem.Name,
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

            if (MetaModelUtils.MetaModelService.GetForm(form.Name) == null)
            {
                MetaModelUtils.MetaModelService.CreateForm(form, modelSaveInfo);
            }
            if (MetaModelUtils.MetaModelService.GetMenuItemDisplay(displayMenuItem.Name) == null)
            {
                MetaModelUtils.MetaModelService.CreateMenuItemDisplay(displayMenuItem, modelSaveInfo);
            }
            if (MetaModelUtils.MetaModelService.GetSecurityPrivilege(viewPrivilege.Name) == null)
            {
                MetaModelUtils.MetaModelService.CreateSecurityPrivilege(viewPrivilege, modelSaveInfo);
            }
            if (MetaModelUtils.MetaModelService.GetSecurityPrivilege(maintainPrivilege.Name) == null)
            {
                MetaModelUtils.MetaModelService.CreateSecurityPrivilege(maintainPrivilege, modelSaveInfo);
            }

            selectedProject.VSProjectNode.AddModelElementsToProject(new List<MetadataReference>()
            {
                new MetadataReference(form.Name, form.GetType()),
                new MetadataReference(displayMenuItem.Name, displayMenuItem.GetType()),
                new MetadataReference(viewPrivilege.Name, viewPrivilege.GetType()),
                new MetadataReference(maintainPrivilege.Name, maintainPrivilege.GetType()),
            });
        }
    }
}
