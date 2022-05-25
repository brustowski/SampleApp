using System.Collections.Generic;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.FieldConfigurations.Common
{
    /// <summary>
    /// Defines the <see cref="DocumentTreeNode" />
    /// </summary>
    [TsInterface(Name = "DocumentTreeNodeServer", IncludeNamespace = false, AutoI = false, Order = 102)]
    public class DocumentTreeNode : TreeNode
    {
        /// <summary>
        /// Creates a new instance of <see cref="DocumentTreeNode"/>
        /// </summary>
        public DocumentTreeNode()
        {
            NodeType = TreeNodeType.Document;
        }
        /// <summary>
        /// Gets the Documents
        /// </summary>
        public List<InboundRecordDocumentViewModel> Documents { get; } = new List<InboundRecordDocumentViewModel>();
    }
}
