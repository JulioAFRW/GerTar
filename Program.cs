using Microsoft.EntityFrameworkCore;
using GERTAR.Modelos;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Scaffold-DbContext "Data Source=DIGITOTAL-001;Initial Catalog=GERTAR;Integrated Security=True;Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Modelos2 -UseDatabaseNames -Force -Tables TB_PROJETO_TAREFAS_HIST
builder.Services.AddDbContext<GERTARContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionString")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
