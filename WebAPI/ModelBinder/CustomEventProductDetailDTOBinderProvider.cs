using EventZone.Domain.DTOs.EventPackageDTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace EventZone.WebAPI.ModelBinder
{
    public class CustomEventProductDetailDTOBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(List<ProductQuantityDTO>))
            {
                return new BinderTypeModelBinder(typeof(DtoFormBinder));
            }

            return null;
        }
    }
}