using System.Collections.Generic;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;

namespace DrsWhatsAppMvp.Services;

public class TemplateRenderer
{
    private readonly FluidParser _parser = new();

    public async Task<string> RenderAsync(string template, Dictionary<string, object> variables)
    {
        if (!_parser.TryParse(template, out IFluidTemplate parsedTemplate, out var error))
        {
            throw new InvalidOperationException($"Template parse error: {error}");
        }

        var context = new TemplateContext();
        var options = context.Options;

        foreach (var kv in variables)
        {
           
            context.SetValue(kv.Key, FluidValue.Create(kv.Value, options));
        }

        return await parsedTemplate.RenderAsync(context);
    }
}
