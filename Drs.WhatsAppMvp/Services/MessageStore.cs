using DrsWhatsAppMvp.Models;

namespace DrsWhatsAppMvp.Services;

public class MessageStore
{
    private readonly List<MessageRecord> _msgs = new();

    public void Add(MessageRecord msg) => _msgs.Add(msg);

    public IEnumerable<MessageRecord> All() => _msgs;
}
