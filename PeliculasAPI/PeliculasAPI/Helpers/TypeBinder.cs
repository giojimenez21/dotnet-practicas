using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace PeliculasAPI.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nameProperty = bindingContext.ModelName;
            var valueProvider = bindingContext.ValueProvider.GetValue(nameProperty);
            if (valueProvider == ValueProviderResult.None) return Task.CompletedTask;
            try
            {
                var value = JsonConvert.DeserializeObject<T>(valueProvider.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(value);
            }
            catch (Exception)
            {
                bindingContext.ModelState.TryAddModelError(nameProperty, "Invalid value for this type");
            }

            return Task.CompletedTask;
        }
    }
}
