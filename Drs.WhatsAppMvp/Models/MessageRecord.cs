namespace DrsWhatsAppMvp.Models;

public record MessageRecord(
    Guid MessageId,
    Guid CandidateId,
    string Direction,           // "inbound" or "outbound"
    string? Template,           // used for outbound templated messages
    object? Content,            // raw message text or rendered content
    DateTimeOffset Timestamp    // when the message was processed
);
