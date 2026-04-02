using Microsoft.EntityFrameworkCore;
using OscarFilmeApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco In-Memory e Controllers
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("OscarDb"));

// Serviços do Swagger (Atenção ao passo 3 da sua lista)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
