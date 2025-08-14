using DrsWhatsAppMvp.Models;
using DrsWhatsAppMvp.Services;
using DrsWhatsAppMvp.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrsWhatsAppMvp.Controllers
{
    /// <summary>
    /// API controller for managing WhatsApp message templates.
    /// </summary>
    [ApiController]
    [Route("api/v1/templates")]
    public class TemplateController : ControllerBase
    {
        // Dependency: in-memory store for templates
        private readonly TemplateStore _store;

        public TemplateController(TemplateStore store)
        {
            _store = store;
        }

        /// <summary>
        /// Returns a list of all available templates.
        /// </summary>
        [HttpGet]
        public IActionResult ListTemplates()
        {
            var result = _store.All();
           
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific template by name.
        /// </summary>
        /// <param name="name">The unique name of the template</param>
        [HttpGet("{name}")]
        public IActionResult GetTemplate(string name)
        {
            var tpl = _store.Get(name);
            var found = tpl is not null;

           
            return found ? Ok(tpl) : NotFound();
        }

        /// <summary>
        /// Creates or updates a template.
        /// </summary>
        /// <param name="tpl">The template record from the request body</param>
        [HttpPost]
        public IActionResult CreateOrUpdate([FromBody] TemplateRecord tpl)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(tpl.Name) || string.IsNullOrWhiteSpace(tpl.Content))
            {
                
                return BadRequest("Name and content are required.");
            }

            // Save or update in the store
            _store.Upsert(tpl);
           
            return Ok(tpl);
        }
    }
}
