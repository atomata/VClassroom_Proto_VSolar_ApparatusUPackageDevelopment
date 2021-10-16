using System.Collections.Generic;
using System.Linq;
using System.Text;
using HexCS.Core;
using HexCS.Data.Generation.CSharp;

namespace HexUN.Events
{
    /// <summary>
    /// COntains all utility function used to genrated events
    /// </summary>
    public static class UTEventGeneration
    {
        /// <summary>
        /// Using the provided arguments generates events of all types. Currently autogenerates
        /// base type and array events
        /// </summary>
        public static void GenerateEventsOfAllTypes(PathString path, string evtType, string evtNamespace, string menuPath, string evtTypeNamespace)
        {
            new EventGenerationArgs(evtType, evtNamespace, menuPath, evtTypeNamespace).GenerateEvents(path.InsertAtEnd("Base"));
            new EventGenerationArgs($"{evtType}[]", evtNamespace, menuPath, evtTypeNamespace).GenerateEvents(path.InsertAtEnd("Array"));
        }

        /// <summary>
        /// Generates generic and scriptable object events at the path specified
        /// </summary>
        /// <param name="args"></param>
        /// <param name="path"></param>
        public static void GenerateEvents(this EventGenerationArgs args, PathString path)
        {
            StringBuilder sb = new StringBuilder();

            // The Event Files
            args.GenerateGenericEvents(sb, path.InsertAtEnd("Generic"));
            args.GenerateSOEvents(sb, path.InsertAtEnd("ScriptableObject"));
        }

        /// <summary>
        /// Generates the generic event files associated with the EventGenerationArgs at the path specified
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="folderPath"></param>
        /// <param name="args"></param>
        public static void GenerateGenericEvents(this EventGenerationArgs args, StringBuilder sb, PathString folderPath)
        {
            // The Unity Event File
            sb.Clear();
            using (GTFile file = new GTFile(sb, folderPath.InsertAtEnd($"{args.UnityEvtName}.cs"), Encoding.UTF8))
            {
                using (GTUsings us = file.Generate_Usings<GTUsings>())
                {
                    List<string> usings = new List<string>();

                    usings.Add("HexUN.Events");
                    usings.Add("UnityEngine");
                    usings.Add("UnityEngine.Events");
                    if (args.EvtTypeNamespace != null) usings.Add(args.EvtTypeNamespace);


                    us.Add_Usings(usings.Where(u => u != args.EvtNamespace).Distinct().ToArray());
                }

                using (GTNamespace nm = file.Generate_Namespace<GTNamespace>())
                {
                    nm.SetRequired(args.EvtNamespace);

                    using (GTClass cls = nm.Generate_NamespaceObject<GTClass>())
                    {
                        cls.SetRequired(args.UnityEvtName, EKeyword.PUBLIC);

                        cls.Generate_Attribute<GTAttribute>().SetRequired("System.Serializable");

                        cls.Add_Inheritances(
                            $"UnityEvent<{args.EvtType}>"
                        );
                    }
                }
            }

            // The Reliable Event File
            sb.Clear();
            using (GTFile file = new GTFile(sb, folderPath.InsertAtEnd($"{args.ReliableEvtName}.cs"), Encoding.UTF8))
            {
                using (GTUsings us = file.Generate_Usings<GTUsings>())
                {
                    List<string> usings = new List<string>();

                    usings.Add("HexUN.Events");
                    if (args.EvtTypeNamespace != null) usings.Add(args.EvtTypeNamespace);

                    us.Add_Usings(usings.Where(u => u != args.EvtNamespace).Distinct().ToArray());
                }

                using (GTNamespace nm = file.Generate_Namespace<GTNamespace>())
                {
                    nm.SetRequired(args.EvtNamespace);

                    using (GTClass cls = nm.Generate_NamespaceObject<GTClass>())
                    {
                        cls.SetRequired(args.ReliableEvtName, EKeyword.PUBLIC);

                        cls.Generate_Attribute<GTAttribute>().SetRequired("System.Serializable");

                        cls.Add_Inheritances(
                            $"ReliableEvent<{args.EvtType}, {args.UnityEvtName}>"
                        );
                    }
                }
            }

            //// The Event Listener File
            //sb.Clear();
            //using (GTFile file = new GTFile(sb, folderPath.AddStep($"{args.EvtListenerName}.cs"), Encoding.UTF8))
            //{
            //    using (GTUsings us = file.Generate_Usings<GTUsings>())
            //    {
            //        List<string> usings = new List<string>();

            //        usings.Add("UnityEngine");
            //        usings.Add("HexUN.Events");
            //        if (args.EvtTypeNamespace != null) usings.Add(args.EvtTypeNamespace);

            //        us.Add_Usings(usings.Where(u => u != args.EvtNamespace).Distinct().ToArray());
            //    }

            //    using (GTNamespace nm = file.Generate_Namespace<GTNamespace>())
            //    {
            //        nm.SetRequired(args.EvtNamespace);

            //        using (GTClass cls = nm.Generate_NamespaceObject<GTClass>())
            //        {
            //            using (GTAttribute att = cls.Generate_Attribute<GTAttribute>())
            //            {
            //                att.SetRequired("AddComponentMenu");
            //                att.Add_Args(new Arg_Basic($"\"{args.MenuPath}/{args.ReadableEvtType}/{args.EvtListenerName}\""));
            //            }

            //            cls.SetRequired(args.EvtListenerName, EKeyword.PUBLIC);

            //            cls.Add_Inheritances(
            //                $"EventListener<{args.EvtType}, {args.UnityEvtName}>"
            //            );
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Generates the scriptable object event files associated with the given EventGenerationArgs at the path specified
        /// </summary>
        /// <param name="args"></param>
        /// <param name="sb"></param>
        /// <param name="folderPath"></param>
        public static void GenerateSOEvents(this EventGenerationArgs args, StringBuilder sb, PathString folderPath)
        {
            sb.Clear();
            // Generate the SO Event file
            using (GTFile file = new GTFile(sb, folderPath.InsertAtEnd($"{args.SoEvtName}.cs"), Encoding.UTF8))
            {
                using (GTUsings us = file.Generate_Usings<GTUsings>())
                {
                    List<string> usings = new List<string>();

                    usings.Add("UnityEngine");
                    usings.Add("HexUN.Events");
                    if (args.EvtTypeNamespace != null) usings.Add(args.EvtTypeNamespace);

                    us.Add_Usings(usings.Where(u => u != args.EvtNamespace).Distinct().ToArray());
                }

                using (GTNamespace nm = file.Generate_Namespace<GTNamespace>())
                {
                    nm.SetRequired(args.EvtNamespace);

                    using (GTClass cls = nm.Generate_NamespaceObject<GTClass>())
                    {
                        using (GTAttribute attr = cls.Generate_Attribute<GTAttribute>())
                        {
                            attr.SetRequired("CreateAssetMenu");

                            attr.Add_Args(
                                new Arg_Named("fileName", $"\"{args.SoEvtName}\""),
                                new Arg_Named("menuName", $"\"{args.MenuPath}/{args.ReadableEvtType}\"")
                            );
                        }

                        cls.SetRequired(args.SoEvtName, EKeyword.PUBLIC);

                        cls.Add_Inheritances(
                            $"ScriptableObjectEvent<{args.EvtType}>"
                        );
                    }
                }
            }

            // Generate the SO Event Listener file
            sb.Clear();
            using (GTFile file = new GTFile(sb, folderPath.InsertAtEnd($"{args.SoEvtListenerName}.cs"), Encoding.UTF8))
            {
                using (GTUsings us = file.Generate_Usings<GTUsings>())
                {
                    List<string> usings = new List<string>();

                    usings.Add("HexUN.Events");
                    usings.Add("UnityEngine");
                    usings.Add("UnityEngine.Events");
                    if (args.EvtTypeNamespace != null) usings.Add(args.EvtTypeNamespace);


                    us.Add_Usings(usings.Where(u => u != args.EvtNamespace).Distinct().ToArray());
                }

                using (GTNamespace nm = file.Generate_Namespace<GTNamespace>())
                {
                    nm.SetRequired(args.EvtNamespace);

                    using (GTClass cls = nm.Generate_NamespaceObject<GTClass>())
                    {

                        using (GTAttribute att = cls.Generate_Attribute<GTAttribute>())
                        {
                            att.SetRequired("AddComponentMenu");
                            att.Add_Args(new Arg_Basic($"\"{args.MenuPath}/{args.ReadableEvtType}/{args.SoEvtListenerName}\""));
                        }

                        cls.SetRequired(args.SoEvtListenerName, EKeyword.PUBLIC);

                        cls.Add_Inheritances(
                            $"ScriptableObjectEventListener<{args.EvtType}, {args.SoEvtName}, {args.UnityEvtName}>"
                        );
                    }
                }
            }
        }
    }
}