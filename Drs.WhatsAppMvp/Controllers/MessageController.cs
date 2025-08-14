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

    public MessageController(MessageStore messages, TemplateRenderer renderer)
    {
        _messages = messages;
        _renderer = renderer;
    }

    /// <summary>
    /// Renders and sends a templated WhatsApp message.
    /// </summary>
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
    {
        if (dto.CandidateId == Guid.Empty)
            return BadRequest("CandidateId is required.");

        // Render template using Fluid engine
        var renderedText = await _renderer.RenderAsync(dto.Template, dto.Variables);

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
        Audit.Log(HttpContext, "SEND_MESSAGE", dto.CandidateId.ToString(), true);

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
