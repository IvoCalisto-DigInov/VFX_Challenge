using Microsoft.EntityFrameworkCore;
using VFX_Challenge.External;
using VFX_Challenge.Repositories;
using VFX_Challenge.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do DbContext com SQLite
builder.Services.AddDbContext<ExchangeRateDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro dos servi�os e reposit�rios
builder.Services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

// Configura��o do cliente HTTP para a API externa
builder.Services.AddHttpClient<IExternalExchangeRateApi, ExternalExchangeRateApi>(client =>
{
    client.BaseAddress = new Uri("https://www.alphavantage.co/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    // Adicione a chave da API nos cabe�alhos ou par�metros de consulta, se necess�rio
});

// Adiciona suporte a controladores
builder.Services.AddControllers();

// Configura��o do Swagger para documenta��o da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
