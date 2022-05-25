using System.Collections.Generic;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.FieldConfigurations.Common
{
    /// <summary>
    /// Defines the <see cref="FieldTreeNode" />
    /// </summary>
    [TsInterface(Name = "FieldTreeNodeServer", IncludeNamespace = false, AutoI = false, Order = 101)]
    public class FieldTreeNode : TreeNode
    {
        /// <summary>
        /// Creates a new instance of <see cref="FieldTreeNode" />
        /// </summary>
        public FieldTreeNode()
        {
            NodeType = TreeNodeType.Field;
        }
        /// <summary>
        /// Gets the Fields
        /// </summary>
        public List<BaseInboundRecordField> Fields { get; } = new List<BaseInboundRecordField>();
    }
}
