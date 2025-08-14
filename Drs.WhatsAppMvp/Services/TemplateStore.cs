using DrsWhatsAppMvp.Models;

namespace DrsWhatsAppMvp.Services
{
    /// <summary>
    /// A simple in-memory store for managing WhatsApp message templates.
    /// Supports CRUD operations: Create, Read, Update, Delete.
    /// </summary>
    public class TemplateStore
    {
        // Internal dictionary to store templates by their name (used as the key)
        private readonly Dictionary<string, TemplateRecord> _templates = new();

        /// <summary>
        /// Returns all templates currently stored.
        /// </summary>
        public IEnumerable<TemplateRecord> All() => _templates.Values;

        /// <summary>
        /// Retrieves a single template by its name.
        /// </summary>
        /// <param name="name">The unique name of the template</param>
        /// <returns>The matching TemplateRecord, or null if not found</returns>
        public TemplateRecord? Get(string name) =>
            _templates.TryGetValue(name, out var tpl) ? tpl : null;

        /// <summary>
        /// Adds or updates a template.
        /// If a template with the same name exists, it will be overwritten.
        /// </summary>
        /// <param name="tpl">The TemplateRecord to add or update</param>
        public void Upsert(TemplateRecord tpl) => _templates[tpl.Name] = tpl;

        /// <summary>
        /// Deletes a template by name.
        /// </summary>
        /// <param name="name">The name of the template to delete</param>
        /// <returns>True if the template was deleted; false if it wasn't found</returns>
        public bool Delete(string name) => _templates.Remove(name);
    }
}
