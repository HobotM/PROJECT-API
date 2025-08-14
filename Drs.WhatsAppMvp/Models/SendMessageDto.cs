public record SendMessageDto(
    Guid CandidateId,
    string Template,
    Dictionary<string, object> Variables,
    string? CorrelationId = null
);
