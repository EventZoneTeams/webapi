using EventZone.Domain.DTOs.EventPackageDTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repositories.Models;
using System.Text.Json;
using WebAPI.Controllers;

namespace EventZone.WebAPI.ModelBinder
{
    public class DtoFormBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var values = valueProviderResult.Values;

            if (values.Count == 0)
            {
                return Task.CompletedTask;
            }

            try
            {
                var result = new List<ProductQuantityDTO>();
                foreach (var value in values)
                {
                    Console.WriteLine($"Received value: {value}");
                    var item = JsonSerializer.Deserialize<ProductQuantityDTO>(value);
                    if (item != null)
                        result.Add(item);
                }
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            catch (JsonException ex)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                bindingContext.ModelState.AddModelError(modelName, $"Invalid JSON: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
}