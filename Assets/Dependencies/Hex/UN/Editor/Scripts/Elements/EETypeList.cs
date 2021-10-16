using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using HexCS.Core;

namespace HexUN.EditorElements
{
    /// <summary>
    /// An interactable list where a type can be selected
    /// </summary>
    public class EETypeList
    {
        //private Type[] _types;
        private EEDropdown _assembliesDropdown;
        private Assembly _assemblySelected;
        private string _assemblyRendering;

        private EEDropdown _namespaceDropdown;
        private string _namespaceRendering;

        private EEInteractableList _list;

        //private Dictionary<string, List<Type>> _typeCache = new Dictionary<string, List<Type>>();
        private Dictionary<Assembly, AssemblyContents> _assemblyMap = new Dictionary<Assembly, AssemblyContents>(); // for efficiency, use first level as assemblies
        private Predicate<Type> _filter;
        private int _namespaceLevels;

        #region API
        /// <summary>
        /// The current selected element
        /// </summary>
        public string SelectedNamespace => _namespaceDropdown.Selected.ReadableName;

        /// <summary>
        /// The last selected type
        /// </summary>
        public Type SelectedType { get; private set; } = null;

        /// <summary>
        /// Invoked when a button is click on a type. Returns the selected type
        /// </summary>
        public event Action<Type> OnTypeSelected;

        /// <summary>
        /// Create a TypeList
        /// </summary>
        /// <param name="assemblies">The assemblies to pull types from to detemrine available namespaces</param>
        /// <param name="namespaceLevels">The level of namespace to display. If levle is 2, then namespace x.y.z is resolved to x.y. If 0, does not constrain levels</param>
        /// <param name="filter">When a filter returns true, the element should be kept</param>
        public EETypeList(Assembly[] assemblies, int namespaceLevels = 0, Predicate<Type> filter = null)
        {
            _namespaceLevels = namespaceLevels;
            _filter = filter;
            foreach(Assembly ass in assemblies)
            {
                _assemblyMap[ass] = null;
            }

            _assembliesDropdown = new EEDropdown(
                _assemblyMap.Keys
                    .Select( a => new EEDropdown.SElement() { ReadableName = a.GetName().Name, Element = a } )
                    .OrderBy( a => a.ReadableName)
                    .ToArray()
            );
        }

        public void Render_Basic()
        {
            //Render dropdown containing namespaces
            EditorGUILayout.BeginHorizontal();
            _assembliesDropdown.Render_Basic();
            _namespaceDropdown?.Render_Basic();
            EditorGUILayout.EndHorizontal();

            // is assembly changed, render that assemblies namespaces
            if(_assemblyRendering != _assembliesDropdown.Selected.ReadableName)
            {
                // Get the assembly selected
                _assemblySelected = _assembliesDropdown.Selected.Element as Assembly;
                if (_assemblySelected != null && _assemblyMap[_assemblySelected] == null)
                    LoadAssemblyTypes(_assemblySelected, _namespaceLevels, _filter);

                _namespaceDropdown = new EEDropdown(
                    _assemblyMap[_assemblySelected].Namespaces.Select( 
                        n => new EEDropdown.SElement() { ReadableName = n.Namespace.SeparatorLevelPrune('.', _namespaceLevels) }
                    ).ToArray()
                );

                _assemblyRendering = _assemblySelected.GetName().Name;
            }

            // if the namespace changes, get the new types
            if (_namespaceDropdown != null && _namespaceRendering != _namespaceDropdown.Selected.ReadableName)
            {
                string selectedNamespace = _namespaceDropdown.Selected.ReadableName;
                Type[] types = _assemblyMap[_assemblySelected].Namespaces.FirstOrDefault(n => n.Namespace == selectedNamespace)?.Types;

                EEInteractableItem.Args[] args = types.Select(
                    t => new EEInteractableItem.Args()
                    {
                        ItemLabel = new SLabel() { ReadableName = t.Name, Id = t.Name },
                        ButtonLabels = new SLabel[]
                        {
                            new SLabel { ReadableName = "Select", Id = "select" }
                        }
                    }
                ).OrderBy(a => a.ItemLabel.ReadableName).ToArray();

                _list = new EEInteractableList(new SLabel() { ReadableName = "Types" }, args);
                _list.OnItemInteraction += OnItemInteraction;

                _namespaceRendering = selectedNamespace;
            }

            _list.Render_Basic(300);
        }
        #endregion

        private void LoadAssemblyTypes(Assembly ass, int namespaceLevels, Predicate<Type> filter)
        {
            Type[] types =  UTAssembly.GetTypesFromAssemblies(ass);
            if (filter != null) types = types.Where(e => filter(e)).ToArray();

            string[] nsps = UTType.NamespacesFromTypes(types, namespaceLevels);

            List<NamespaceMap> maps = new List<NamespaceMap>();

            foreach (string s in nsps)
            {
                NamespaceMap map = new NamespaceMap(
                    s,
                    types.Where(t => t.Namespace == null ? s == "_NoNamespace" : t.Namespace.SeparatorLevelPrune('.', namespaceLevels) == s).ToArray()
                );

                maps.Add(map);
            }

            _assemblyMap[ass] = new AssemblyContents(maps.ToArray());
        }

        private void OnItemInteraction(EEInteractableItem item, SLabel label)
        {
            AssemblyContents cont = _assemblyMap[_assemblySelected];
            NamespaceMap map = cont.Namespaces.FirstOrDefault(n => n.Namespace == _namespaceDropdown.Selected.ReadableName);
            Type type = map?.Types.FirstOrDefault(t => t.Name == item.Label.ReadableName);

            SelectedType = type;
            OnTypeSelected?.Invoke(type);
        }

        private class AssemblyContents
        {
            public NamespaceMap[] Namespaces;

            public AssemblyContents(NamespaceMap[] namespaces) => Namespaces = namespaces;
        }

        private class NamespaceMap
        {
            public string Namespace;
            public Type[] Types;

            public NamespaceMap(string @namespace, Type[] types)
            {
                Namespace = @namespace;
                Types = types;
            }
        }
    }
}