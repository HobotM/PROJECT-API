using DrsWhatsAppMvp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MessageStore>();
builder.Services.AddSingleton<TemplateRenderer>();
builder.Services.AddSingleton<TemplateStore>();


var app = builder.Build();




app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));


// Seed demo templates for testing in Swagger
var templateStore = app.Services.GetRequiredService<TemplateStore>();

templateStore.Upsert(new DrsWhatsAppMvp.Models.TemplateRecord(
    "INTERVIEW_INVITE",
    "Hi {{firstName}}, your interview is on {{date}} at {{time}}. Location: {{location}}."
));

templateStore.Upsert(new DrsWhatsAppMvp.Models.TemplateRecord(
    "MISSING_DOCS",
    "Dear {{firstName}}, we`re still waiting for your {{documentName}}. Please upload it ASAP."
));

templateStore.Upsert(new DrsWhatsAppMvp.Models.TemplateRecord(
    "SUCCESSFUL_APPLICATION",
    "Congratulations {{firstName}}! You've passed the next stage. We'll contact you about what's next."
));

templateStore.Upsert(new DrsWhatsAppMvp.Models.TemplateRecord(
    "REJECTION_NOTICE",
    "Hi {{firstName}}, unfortunately your application for {{position}} was not successful this time."
));

templateStore.Upsert(new DrsWhatsAppMvp.Models.TemplateRecord(
    "GENERIC_FOLLOWUP",
    "Hi {{firstName}}, just checking in — let us know if you have any questions."
));


app.Run();
