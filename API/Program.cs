using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.Configure<ApiBehaviorOptions>(apiBehaviorOptions => {
    apiBehaviorOptions.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();