using System.Collections.Generic;
using FilingPortal.PluginEngine.Models;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.FieldConfigurations.Common
{
    /// <summary>
    /// Defines the <see cref="TreeNode" />
    /// </summary>
    [TsInterface(Name = "TreeNodeServer", IncludeNamespace = false, AutoI = false, Order = 100)]
    public class TreeNode
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Node Type
        /// </summary>
        public TreeNodeType NodeType { get; set; } = TreeNodeType.Container;

        /// <summary>
        /// Gets the Actions
        /// </summary>
        [TsProperty(StrongType = typeof(SortedDictionary<string, bool>))]
        public Actions Actions { get; } = new Actions();

        /// <summary>
        /// Gets or sets the Parent Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Gets the Children
        /// </summary>
        public List<TreeNode> Children { get; } = new List<TreeNode>();
    }

    /// <summary>
    /// Tree node type
    /// </summary>
    [TsEnum(IncludeNamespace = false, Order = 99)]
    public enum TreeNodeType
    {
        /// <summary>
        /// Fields container
        /// </summary>
        Container,
        /// <summary>
        /// Field
        /// </summary>
        Field,
        /// <summary>
        /// Document
        /// </summary>
        Document
    }
}
