using System;
using System.Threading.Tasks;
using HashidsNet;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class HashIdModelBinder : IModelBinder
{
    Hashids hashids = new Hashids("!2ZX5i3KVR@S79*MEGB&j^FLwapS2Kh7hfpc!^", 16);// Add salt 

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        var str = valueProviderResult.FirstValue;

        try{
            bindingContext.Result = ModelBindingResult.Success(hashids.Decode(str)[0]);
        }
        catch(IndexOutOfRangeException e) {
            System.Console.WriteLine(e);
        }

        return Task.CompletedTask;
    }
}