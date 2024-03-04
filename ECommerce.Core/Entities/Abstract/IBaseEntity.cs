namespace ECommerce.Core.Entities.Abstract
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
        DateTime DeletedDate { get; set; }
        bool IsValid { get; set; }
    }
}
