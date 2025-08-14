using DrsWhatsAppMvp.Models;
using DrsWhatsAppMvp.Services;
using DrsWhatsAppMvp.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrsWhatsAppMvp.Controllers;

[ApiController]
[Route("api/v1/messages")]
public class MessageController : ControllerBase
{
    private readonly MessageStore _messages;
    private readonly TemplateRenderer _renderer;
    private readonly TemplateStore _templates;

    public MessageController(MessageStore messages, TemplateRenderer renderer, TemplateStore templates)
    {
        _messages = messages;
        _renderer = renderer;
        _templates = templates;
    }

    /// <summary>
    /// Renders and sends a templated WhatsApp message.
    /// </summary>
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
    {
        if (dto.CandidateId == Guid.Empty)
            return BadRequest("CandidateId is required.");

        // Check if template is a name or raw string
        var templateContent = dto.Template;

        if (!dto.Template.Contains("{{"))
        {
            var stored = _templates.Get(dto.Template);
            if (stored is null)
                return NotFound($"Template '{dto.Template}' not found.");

            templateContent = stored.Content;
        }

        // Render template using Fluid engine
        var renderedText = await _renderer.RenderAsync(templateContent, dto.Variables);

        // Store message
        var message = new MessageRecord(
            Guid.NewGuid(),
            dto.CandidateId,
            "outbound",
            dto.Template,
            renderedText,
            DateTimeOffset.UtcNow
        );

        _messages.Add(message);

        // Audit log for traceability
        Audit.Log(HttpContext, "SEND_MESSAGE", dto.CandidateId.ToString(), true, renderedText);

        return Ok(new
        {
            message.MessageId,
            status = "queued",
            rendered = renderedText
        });
    }

    /// <summary>
    /// Returns all messages (inbound/outbound) for a given candidate.
    /// </summary>
    [HttpGet("{candidateId}")]
    public IActionResult GetMessages(Guid candidateId)
    {
        if (candidateId == Guid.Empty)
            return BadRequest("CandidateId is required.");

        var results = _messages.All()
            .Where(m => m.CandidateId == candidateId)
            .OrderBy(m => m.Timestamp)
            .ToList();


        return Ok(results);
    }
    
}
