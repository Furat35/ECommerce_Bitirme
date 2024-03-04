using ECommerce.Core.Entities.Abstract;

namespace ECommerce.Core.Entities
{
    public class BaseEntity : IBaseEntity
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual DateTime DeletedDate { get; set; }
        public virtual bool IsValid { get; set; }
    }
}
