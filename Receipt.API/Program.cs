using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Receipt.API.DTOs.Requests;
using Receipt.API.DTOs.Validators.Requests;
using Receipt.API.DTOs.Validators.ViewModels;
using Receipt.API.DTOs.ViewModels;
using Receipt.API.Logic.Handlers;
using Receipt.Models.Data;
using Receipt.Models.Storage;
using Receipt.Models.Storage.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IReceiptHandler, ReceiptHandler>();
builder.Services.AddScoped<IRepository<ReceiptItem>, ReceiptItemRepository>();
builder.Services.AddScoped<IRepository<Receipt.Models.Data.Receipt>, ReceiptRepository>();
builder.Services.AddSingleton<ReceiptContext>();

builder.Services.AddScoped<IValidator<ProcessReceiptRequest>, ProcessReceiptRequestValidator>();
builder.Services.AddScoped<IValidator<ReceiptItemViewModel>, ReceiptItemViewModelValidator>();
builder.Services.Configure<ApiBehaviorOptions>(apiBehaviorOptions => {
    apiBehaviorOptions.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();