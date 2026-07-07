using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Commands;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Reflection;
using System.Text;
using System.Windows;

namespace SOG_SharedUtils
{
    public class FormUtils
    {
        public static TType Add<TType>(ICommandProvider commandProvider, string name = null) where TType : class
        {
            return Add(commandProvider, typeof(TType), name) as TType;
        }

        public static IFormControl Add(ICommandProvider commandProvider, Type type, string name = null)
        {
            string guid = DomainClassId(type);

            IFormControl ret = null;

            if (guid != null)
            {
                commandProvider.ExecuteAdd(Guid.Parse(guid));

                int count;

                if (commandProvider is IFormDesign design)
                {
                    count = Count(design);

                    if (count > 0)
                    {
                        ret = design.FormControls[count - 1];
                    }
                }
                else if (commandProvider is IFormControlWithChildren formControl)
                {
                    count = Count(formControl);

                    if (count > 0)
                    {
                        ret = formControl.FormControls[count - 1];
                    }
                }

                if (ret != null && !string.IsNullOrEmpty(name))
                {
                    ret.Name = name;
                }
            }

            return ret;
        }

        public static void Pattern(ICommandProvider commandProvider, string pattern, string patternVersion)
        {
            if (commandProvider is IFormDesign design)
            {
                if (string.IsNullOrEmpty(design.Pattern))
                {
                    design.Execute(CommandType.Pattern);
                }

                design.Pattern = pattern;
                design.PatternVersion = patternVersion;
            }
            else if (commandProvider is IFormControl formControl)
            {
                if (string.IsNullOrEmpty(formControl.Pattern))
                {
                    formControl.Execute(CommandType.Pattern);
                }

                formControl.Pattern = pattern;
                formControl.PatternVersion = patternVersion;
            }
        }

        public static void Clear(IFormDesign design)
        {
            try
            {
                IFormControl formControl;
                while ((formControl = design.FormControls[0]) != null)
                {
                    formControl.Execute(CommandType.Delete);
                }
            }
            catch
            {
            }
        }

        public static int Count(object obj)
        {
            int count = 0;

            try
            {
                if (obj is IFormDesign design)
                {
                    while (design.FormControls[count++] != null)
                    {
                    }
                }
                else if (obj is IFormControlWithChildren formControl)
                {
                    while (formControl.FormControls[count++] != null)
                    {
                    }
                }
            }
            catch
            {
                --count;
            }

            return count;
        }

        public static string DomainClassId<TType>() where TType : class
        {
            return DomainClassId(typeof(TType));
        }

        public static string DomainClassId(Type type)
        {
            string guid = null;

            if (type == typeof(IFormControl))
            {
                guid = "e5d71308-dadb-4ec2-b3f5-88237def09fc";
            }
            else if (type == typeof(IFormControlExtension))
            {
                guid = "b927d5eb-051f-4433-8499-04ff13a18ee5";
            }
            else if (type == typeof(IFormGroupControl))
            {
                guid = "d5e268e2-cc05-4140-8762-f9299f40a8c3";
            }
            else if (type == typeof(IFormGridControl))
            {
                guid = "966e2a53-13ac-40ac-8fa6-359362a05a0a";
            }
            else if (type == typeof(IFormActionPaneControl))
            {
                guid = "106c1ced-c829-49f5-b7fe-f212e1e41d96";
            }
            else if (type == typeof(IFormTabControl))
            {
                guid = "62072bcd-198d-4147-9b03-3589ebab6261";
            }
            else if (type == typeof(IFormTabPageControl))
            {
                guid = "2aa535d3-0540-4d27-9dfe-b54ec7e840d1";
            }
            else if (type == typeof(IFormButtonControl))
            {
                guid = "e47a667d-99cd-475c-8dd5-8a34a746095c";
            }
            else if (type == typeof(IFormCommandButtonControl))
            {
                guid = "3566737b-66c9-4600-987e-8be127516999";
            }
            else if (type == typeof(IFormDropDialogButtonControl))
            {
                guid = "b1a444c0-44c4-45e0-b231-3c3ee9ffa366";
            }
            else if (type == typeof(IFormMenuButtonControl))
            {
                guid = "104dd9c7-cc7b-4ed2-afcb-bdd997004380";
            }
            else if (type == typeof(IFormMenuFunctionButtonControl))
            {
                guid = "b4f41a75-fafd-40d3-b17d-3241ae988dd2";
            }
            else if (type == typeof(IFormRadioButtonControl))
            {
                guid = "34b2311e-c864-4bde-aa77-c453c301db9e";
            }
            else if (type == typeof(IFormActionPaneTabControl))
            {
                guid = "dcd83da7-b9ab-4f5b-b3ea-3b26e5b40aa1";
            }
            else if (type == typeof(IFormButtonGroupControl))
            {
                guid = "b9aa14fb-a0d9-4eff-af36-3759c9148a14";
            }
            else if (type == typeof(IFormContainerControl))
            {
                guid = "b2b3189e-5d9c-4760-92bd-9ccc45af1ffe";
            }
            else if (type == typeof(IFormListBoxControl))
            {
                guid = "67843a17-adbe-4fdc-9e2c-3e64f6441cf1";
            }
            else if (type == typeof(IFormListViewControl))
            {
                guid = "d913cf53-230e-46e6-9d12-56aab2a78bbb";
            }
            else if (type == typeof(IFormStaticTextControl))
            {
                guid = "29998cc5-0360-4dcc-95cf-f49a5471ff91";
            }
            else if (type == typeof(IFormTreeControl))
            {
                guid = "4a3223e1-c17a-452a-a09f-f910b8e249b2";
            }
            else if (type == typeof(IFormStringControl))
            {
                guid = "9ed160b6-b980-440f-9c6e-ae395149aa84";
            }
            else if (type == typeof(IFormImageControl))
            {
                guid = "64a876e6-1120-4295-aa4f-8360b48e3efc";
            }

            return guid;
        }

        public static void Scan(IForm form)
        {
            var sb = new StringBuilder();
            var formControls = form.FormDesign.FormControls;

            var enumerator = formControls.GetEnumerator();

            while (true)
            {
                bool hasNext;
                try { hasNext = enumerator.MoveNext(); }
                catch { break; }
                if (!hasNext) break;

                var ctrl = enumerator.Current;
                if (ctrl == null) continue;

                var type = ctrl.GetType();
                sb.AppendLine($"Runtime type: {type.FullName}");
                sb.AppendLine($"Base type: {type.BaseType?.FullName}");

                // Check the type itself AND all its base types up the chain
                var current = type;
                while (current != null && current != typeof(object))
                {
                    sb.AppendLine($"\n--- Scanning type: {current.FullName} ---");

                    // Static Guid fields
                    foreach (var f in current.GetFields(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    {
                        try
                        {
                            if (f.FieldType == typeof(Guid))
                                sb.AppendLine($"  STATIC GUID  {f.Name} = {f.GetValue(null)}");
                        }
                        catch { }
                    }

                    // Instance Guid fields
                    foreach (var f in current.GetFields(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        try
                        {
                            if (f.FieldType == typeof(Guid))
                                sb.AppendLine($"  INST GUID    {f.Name} = {f.GetValue(ctrl)}");
                        }
                        catch { }
                    }

                    // Static Guid properties
                    foreach (var p in current.GetProperties(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    {
                        try
                        {
                            if (p.PropertyType == typeof(Guid))
                                sb.AppendLine($"  STATIC GPROP {p.Name} = {p.GetValue(null)}");
                        }
                        catch { }
                    }

                    // Instance Guid properties
                    foreach (var p in current.GetProperties(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        try
                        {
                            if (p.PropertyType == typeof(Guid))
                                sb.AppendLine($"  INST GPROP   {p.Name} = {p.GetValue(ctrl)}");
                        }
                        catch { }
                    }

                    // Also check ALL interfaces this type implements
                    foreach (var iface in current.GetInterfaces())
                    {
                        foreach (var f in iface.GetFields(
                            BindingFlags.Public | BindingFlags.Static))
                        {
                            try
                            {
                                if (f.FieldType == typeof(Guid))
                                    sb.AppendLine($"  IFACE GUID [{iface.Name}] {f.Name} = {f.GetValue(null)}");
                            }
                            catch { }
                        }
                    }

                    current = current.BaseType;
                }

                //// Look for DomainClassId specifically
                //foreach (var f in type
                //    .GetFields(BindingFlags.Public | BindingFlags.Static)
                //    .Where(f => f.FieldType == typeof(Guid)))
                //    sb.AppendLine($"  GUID {f.Name} = {f.GetValue(null)}");

                //// Also check the instance for a class/type identifier
                //foreach (var prop in type.GetProperties())
                //{
                //    try
                //    {
                //        var val = prop.GetValue(ctrl);
                //        if (val is Guid g)
                //            sb.AppendLine($"  PROP {prop.Name} = {g}");
                //    }
                //    catch { }
                //}
            }

            string result = sb.ToString();

            Clipboard.SetText(result);
            MessageBox.Show(result);
        }
    }
}
