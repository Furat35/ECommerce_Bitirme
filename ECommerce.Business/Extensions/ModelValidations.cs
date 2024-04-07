using ECommerce.Core.Exceptions;

namespace ECommerce.Business.Extensions
{
    public static class ModelValidations
    {
        public static void ThrowBadRequestIfIdIsNotValidGuid(params string[] ids)
        {
            foreach (var id in ids)
                if (!Guid.TryParse(id, out Guid validId))
                    throw new BadRequestException("Geçersiz id bilgisi! Tekrar deneyiniz.");

        }
    }
}
