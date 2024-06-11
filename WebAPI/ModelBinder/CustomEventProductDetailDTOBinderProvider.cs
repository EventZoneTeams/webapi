using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Repositories.Models;
using WebAPI.Controllers;

namespace WebAPI.ModelBinder
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