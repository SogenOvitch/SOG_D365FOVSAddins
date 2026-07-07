using EnvDTE;

using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.AX.Metadata.Modeling;
using Microsoft.Dynamics.AX.Metadata.Modeling.Extensions;
using Microsoft.Dynamics.AX.Metadata.Patterns;
using Microsoft.Dynamics.AX.Metadata.Providers;
using Microsoft.Dynamics.AX.Metadata.Service;
using Microsoft.Dynamics.AX.Metadata.Static.Access;
using Microsoft.Dynamics.AX.Metadata.Storage;
using Microsoft.Dynamics.AX.Metadata.Storage.Runtime;
using Microsoft.Dynamics.Framework.Tools.Core;
using Microsoft.Dynamics.Framework.Tools.Core.Common;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Commands;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.ProjectSystem;

using SOG_SharedUtils;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SOG_Testing
{
    public class Program
    {
        public static void TestMetamodel()
        {
            MetaModelUtils.Init();

            var myModel = MetaModelUtils.MetaModelService.GetFormModelInfo("SOG_AddinTesting").First();
            var axModel = MetaModelUtils.MetaModelService.GetFormModelInfo("Accountant_BR").First();

            if (myModel.Customization == Microsoft.Dynamics.AX.Metadata.Core.MetaModel.ModelCustomizationLevel.DoNotAllow)
            {
            }

            var labelFiles = MetaModelUtils.MetadataProvider.LabelFiles.ListObjectsForModel(myModel.Name);

            //IVsSolution solution = null;
            //serviceProvider.AddService(typeof(SVsSolution), solution);

            var vsSolutionProjects = CoreUtility.GetAllProjectsInSolution();

            foreach (var project in vsSolutionProjects)
            {
            }

            //ShellUtility.FindProjectsInSolution(solution);
            //SolutionClass solution = new SolutionClass();
            //ISolutionSearchService
        }

        public static void Main()
        {
            //Microsoft.Dynamics.Framework.Tools.MetaModel.Core.ILabelService
            //Microsoft.Dynamics.Framework.Tools.MetaModel.LabelFiles.LabelFilesDomainModel

            //AxLabelFile test = null;
            //var controller = new Microsoft.Dynamics.Framework.Tools.Labels.LabelControllerFactory().GetOrCreateLabelController(test);
            //controller.Insert("id", "text", "comment");
            //controller.Save();

            //======== C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\Extensions\oqmold1t.3tl\Microsoft.Dynamics.Framework.Tools.Labels.Resolvers.dll ========
            //=== Microsoft.Dynamics.Framework.Tools.Labels.Resolvers.LabelResolverFactory > System.Object ===

            //Microsoft.Dynamics.Framework.Tools.Labels.LabelResource

            //=== Microsoft.Dynamics.Framework.Tools.Labels.LabelEditorController > Microsoft.Dynamics.Framework.Tools.Labels.LabelController

            //=== Microsoft.Dynamics.Framework.Tools.Labels.LabelControllerFactory

            //======== C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\Extensions\oqmold1t.3tl\Microsoft.Dynamics.AX.Xpp.AxShared.dll ========
            //=== Microsoft.Dynamics.Ax.Xpp.LabelHelper

            //var path = Environment.GetEnvironmentVariable("DynamicsVSTools");
            //Console.WriteLine(path);
            //return;

            // Pre-load the dependency first
            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\dotnet\net8.0\runtime\shared\Microsoft.NETCore.App\8.0.17\System.Core.dll");

            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\PrivateAssemblies\Microsoft.VisualStudio.Modeling.Sdk.dll");
            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\PrivateAssemblies\Microsoft.VisualStudio.Modeling.Sdk.Shell.dll");
            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\Extensions\xcfzbgjs.fem\Microsoft.VisualStudio.Interop.dll");
            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\PublicAssemblies\Microsoft.VisualStudio.Shell.15.0.dll");
            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\PublicAssemblies\Microsoft.VisualStudio.Shell.Framework.dll");
            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\Extensions\oqmold1t.3tl\LanguageServerDependencies\Microsoft.VisualStudio.Validation.dll");
            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\PublicAssemblies\Microsoft.VisualStudio.Threading.17.x\Microsoft.VisualStudio.Threading.dll");

            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\Extensions\oqmold1t.3tl\Microsoft.Dynamics.Framework.Tools.AutomationObjects.17.0.dll");
            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\Extensions\oqmold1t.3tl\Microsoft.Dynamics.Framework.Tools.MetaModel.17.0.dll");
            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\Extensions\oqmold1t.3tl\Microsoft.Dynamics.Framework.Tools.MetaModel.Core.17.0.dll");
            Assembly.LoadFrom(@"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\Extensions\oqmold1t.3tl\Microsoft.Dynamics.Framework.Tools.Designers.17.0.dll");

            //ExtractDlls("Label");
            //return;

            TestMetamodel();
            return;

            /* LOOKING FOR DLLS
            
            Get-ChildItem "C:\Program Files\Microsoft Visual Studio\2022\Professional\" -Recurse -Filter "<ASSEMBLY>.dll" -ErrorAction SilentlyContinue | Select-Object FullName

             */

            //new Microsoft.Dynamics.Framework.Tools.MetaModel.Forms.FormControlExtension().

            var extension = new Microsoft.Dynamics.Framework.Tools.FormControlExtension.PowerBIReportViewer.PowerBIReportViewer();

            //var chart = new Microsoft.Dynamics.Framework.Tools.FormControlExtension.SysChartControl.SysChartControl();
            //var QuickFilterControl = new Microsoft.Dynamics.Framework.Tools.FormControlExtension.QuickFilterControl.QuickFilterControl();
            //var DefaultColumnTypeConverter = new Microsoft.Dynamics.Framework.Tools.FormControlExtension.QuickFilterControl.DefaultColumnTypeConverter();
            //var TargetControlNameTypeConverter = new Microsoft.Dynamics.Framework.Tools.FormControlExtension.QuickFilterControl.TargetControlNameTypeConverter();

            var patternFactory = new Microsoft.Dynamics.AX.Metadata.Patterns.PatternFactory(includeStandardPatterns: true);

            foreach (var pattern in patternFactory.AllPatterns.ToList().GroupBy(x => x.Name).Select(x => x.First()))
            {
                //if (!pattern.FriendlyName.Contains("Hub") && pattern.Name != "CustomAndQuickFilters")
                //{
                //    continue;
                //}
                //if (!pattern.FriendlyName.Contains("Details"))
                //{
                //    continue;
                //}
                if (!pattern.FriendlyName.Contains("Lookup w/ Preview") &&
                    !pattern.FriendlyName.Contains("Workspace Operational") &&
                    !pattern.FriendlyName.Contains("Chart") &&
                    !pattern.Name.Contains("Section") &&
                    !pattern.Name.Contains("Quick"))
                {
                    continue;
                }
                //if (!pattern.FriendlyName.Contains("Dimension"))
                //{
                //    continue;
                //}
                Console.WriteLine($"{pattern.FriendlyName} ({pattern.Name}) {pattern.Version} {pattern.Category}");
                var node = pattern.Root;
                WritePatternNode(node);
            }
        }

        public static void WritePatternNode(PatternNode node, string indent = "")
        {
            //if (node.RequireOne == false)
            //{
            //    return;
            //}

            string childIndent = indent + "\t";

            //if (node.RestrictedProperties.Any(x => x.Op.ToString() != "Equals"))
            //{
            string infos = $"{indent}{node.FriendlyName} ({node.Part}) {node.Type}";

            if (node.SubPatterns.Any())
            {
                var pattern = node.SubPatterns.First();
                infos += $" | {pattern}";
            }

            if (node.RestrictedProperties.Any())
            {
                infos += $"\n{indent + " o "}{node.RestrictedProperties.Select(x => $"{x.Name}->{x.ExpectedValue} ({x.Condition}|{x.Op})").Aggregate((a, b) => $"{a}, {b}")}";
            }

            Console.WriteLine(infos);
            //}

            foreach (var childNode in node.SubNodes)
            {
                WritePatternNode(childNode, childIndent);
            }
        }

        public static void ExtractDlls(string filter = "Quick", bool showErrors = false)
        {
            var basePath = @"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\Extensions\oqmold1t.3tl\";

            //var dlls = new[]
            //{
            //    //"Microsoft.Dynamics.Framework.Tools.AutomationObjects.17.0.dll",
            //    //"Microsoft.Dynamics.Framework.Tools.MetaModel.17.0.dll",
            //    //"Microsoft.Dynamics.Framework.Tools.Designers.17.0.dll",
            //    "Microsoft.Dynamics.Framework.Tools.FormControlExtension.17.0.dll",
            //    //"Microsoft.Dynamics.AX.Metadata.Patterns.dll",
            //};

            string[] dlls = Directory.GetFiles(basePath, "*.dll", SearchOption.AllDirectories);

            foreach (var dll in dlls)
            {
                IEnumerable<Type> types;
                try
                {
                    var asm = Assembly.LoadFrom(Path.Combine(basePath, dll));

                    types = asm.GetExportedTypes();

                    ExtractAssembly(dll, asm, filter);
                }
                catch (Exception ex)
                {
                    if (showErrors)
                    {
                        Console.WriteLine($"\n\n======== {dll} ========");
                        Console.WriteLine(ex.Message);
                    }
                    continue;
                }
            }
        }

        public static void ExtractAssembly(string dll, Assembly asm, string filter = "Quick")
        {
            var sb = new StringBuilder();
            bool dllAdded = false;

            foreach (var type in asm.GetExportedTypes().OrderBy(t => t.Name))
            {
                // 1 — Grab all static Guid fields/props (DomainClassId candidates)
                var guidFields = type
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)
                    //.Where(f => f.FieldType == typeof(Guid))
                    .ToList();

                var guidProps = type
                    .GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)
                    //.Where(p => p.PropertyType == typeof(Guid))
                    .ToList();

                // 2 — Flag types related to Commands or Form controls
                bool isInteresting = /*type.Name.Contains("Command")
                                  || type.Name.Contains("FormControl")
                                  || type.Name.Contains("FormDesign")
                                  || type.Name.Contains("FormGroup")
                                  || type.Name.Contains("FormGrid")
                                  || type.Name.Contains("FormTab")
                                  || type.Name.Contains("ActionPane")
                                  ||*/ type.Name.Contains(filter)/*
                                  || guidFields.Any()
                                  || guidProps.Any()*/;

                if (!isInteresting) continue;

                if (!dllAdded)
                {
                    sb.AppendLine($"\n\n======== {dll} ========");
                    dllAdded = true;
                }

                sb.AppendLine($"\n  === {type.FullName} > {type.BaseType?.FullName} ===");

                // Print all members for command/form types
                foreach (var m in type.GetMembers().OrderBy(m => m.Name))
                {
                    sb.AppendLine($"    {m.MemberType,-12} {m.Name}");
                }

                // Print Guid values
                foreach (var f in guidFields)
                {
                    sb.AppendLine($"    GUID FIELD  {f.Name} = {f.GetValue(null)}");
                }
                foreach (var p in guidProps)
                {
                    sb.AppendLine($"    GUID PROP   {p.Name} = {p.GetValue(null)}");
                }
            }

            if (dllAdded)
            {
                Console.WriteLine(sb.ToString());
            }
        }
    }
}