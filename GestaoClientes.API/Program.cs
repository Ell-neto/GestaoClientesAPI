using GestaoClientes.Application.Clientes.Comandos;
using GestaoClientes.Application.Clientes.Consultas;
using GestaoClientes.Application.Configuracao;
using GestaoClientes.Infrastructure.Configuracao;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AdicionarAplicacao()
    .AdicionarInfraestrutura();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/clientes", async (
    CriarClienteRequest request,
    CriarClienteCommandHandler handler,
    CancellationToken ct) =>
{
    var resultado = await handler.ExecutarAsync(
        new CriarClienteCommand(request.NomeFantasia, request.Cnpj),
        ct);

    if (!resultado.Sucesso)
        return Results.BadRequest(new { erros = resultado.Erros });

    var id = resultado.Valor!;
    return Results.Created($"/clientes/{id}", new { id });
});

app.MapGet("/clientes/{id:guid}", async (
    Guid id,
    ObterClientePorIdQueryHandler handler,
    CancellationToken ct) =>
{
    var dto = await handler.ExecutarAsync(new ObterClientePorIdQuery(id), ct);
    return dto is null ? Results.NotFound() : Results.Ok(dto);
});

app.Run();

public record CriarClienteRequest(string NomeFantasia, string Cnpj);
