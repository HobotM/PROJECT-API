namespace DrsWhatsAppMvp.Utils;

public static class Audit
{
    public static void Log(HttpContext ctx, string action, string resourceId, bool success)
    {
        var corr = ctx.Request.Headers["X-Correlation-Id"].ToString();
        Console.WriteLine($"AUDIT ts={DateTimeOffset.UtcNow:o} action={action} candidate={resourceId} ip={ctx.Connection.RemoteIpAddress} corr={corr} success={success}");
    }
}
