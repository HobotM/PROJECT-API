using System.Collections.Generic;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;

namespace DrsWhatsAppMvp.Services
{
    /// <summary>
    /// Uses Fluid template engine to render message templates with variables.
    /// </summary>
    public class TemplateRenderer
    {
        // FluidParser is responsible for parsing Liquid-style templates
        private readonly FluidParser _parser = new();

        /// <summary>
        /// Renders a template string by replacing placeholders (e.g., {{name}}) with actual values.
        /// </summary>
        /// <param name="template">The template string containing placeholders</param>
        /// <param name="variables">Key-value pairs used to replace placeholders</param>
        /// <returns>The rendered string with all variables filled in</returns>
        public async Task<string> RenderAsync(string template, Dictionary<string, object> variables)
        {
            // Try to parse the input template. If it fails, throw an error.
            if (!_parser.TryParse(template, out IFluidTemplate parsedTemplate, out var error))
            {
                throw new InvalidOperationException($"Template parse error: {error}");
            }

            // Create a new template rendering context
            var context = new TemplateContext();
            var options = context.Options;

            // Add all variables to the context so they can be used in the template
            foreach (var kv in variables)
            {
                context.SetValue(kv.Key, FluidValue.Create(kv.Value, options));
            }

            // Render the final string using the context with variable values
            return await parsedTemplate.RenderAsync(context);
        }
    }
}
