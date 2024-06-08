using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using WebAPI.Controllers;

namespace WebAPI
{
    public class CustomEventProductDetailDTOBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(List<EventProductDetailDTO>))
            {
                return new BinderTypeModelBinder(typeof(DtoFormBinder));
            }

            return null;
        }
    }
}