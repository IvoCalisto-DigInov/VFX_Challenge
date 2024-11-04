using Microsoft.EntityFrameworkCore;
using Serilog;
using VFX_Challenge.External;
using VFX_Challenge.Repositories;
using VFX_Challenge.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do DbContext 
builder.Services.AddDbContext<ExchangeRateDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VFX_ChallengeDB")));

// Registro dos servi�os e reposit�rios
builder.Services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

// Configura��o do cliente HTTP para a API externa
builder.Services.AddHttpClient<IExternalExchangeRateApi, ExternalExchangeRateApi>();

// Adiciona suporte a controladores
builder.Services.AddControllers();

// Configura��o do Swagger para documenta��o da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog using the settings from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read configuration
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging

var app = builder.Build();

// Apply pending migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ExchangeRateDbContext>();
    dbContext.Database.Migrate();
}

// Configura��o do pipeline de middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
