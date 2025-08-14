using DrsWhatsAppMvp.Models;
using DrsWhatsAppMvp.Services;
using DrsWhatsAppMvp.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrsWhatsAppMvp.Controllers;

[ApiController]
[Route("api/v1/webhooks")]
public class WebhookController : ControllerBase
{
    private readonly MessageStore _store;

    public WebhookController(MessageStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Receives an incoming message (reply) from WhatsApp.
    /// This simulates the webhook AEP/WhatsApp would call.
    /// </summary>
    [HttpPost("whatsapp")]
    public IActionResult Receive([FromBody] WhatsAppWebhookDto dto)
    {
        if (dto.CandidateId == Guid.Empty)
            return BadRequest("CandidateId is required.");

        // Create and store an inbound message
        var message = new MessageRecord(
            Guid.NewGuid(),
            dto.CandidateId,
            "inbound",
            null,              // template
            dto.Message,       // content
            DateTimeOffset.UtcNow
        );


        _store.Add(message);

        // Log for traceability
        Audit.Log(HttpContext, "RECEIVE_REPLY", dto.CandidateId.ToString(), true);

        return Accepted();
    }
}
