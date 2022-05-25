using Framework.Domain;

namespace FilingPortal.Domain.DocumentTypes.Entities
{
    public class DocumentType: Entity
    {
        public string TypeName { get; set; }
        public string Description { get; set; }
    }
}