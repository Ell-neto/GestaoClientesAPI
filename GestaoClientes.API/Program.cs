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

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new
        {
            titulo = "Erro interno",
            detalhe = "Ocorreu um erro inesperado. Tente novamente."
        });
    });
});


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

app.MapPut("/clientes/{id:guid}", async (
    Guid id,
    AtualizarClienteRequest request,
    AtualizarClienteCommandHandler handler,
    CancellationToken ct) =>
{
    var resultado = await handler.ExecutarAsync(
        new AtualizarClienteCommand(id, request.NomeFantasia, request.Ativo),
        ct);

    if (!resultado.Sucesso)
        return Results.NotFound(new { erros = resultado.Erros });

    return Results.NoContent();
});

// Prefiro definir valores padrão para paginação: int pagina = 1, int tamanhoPagina = 10
app.MapGet("/clientes", async (
    int pagina,
    int tamanhoPagina,
    bool? ativo,
    string? nome,
    ListarClientesQueryHandler handler,
    CancellationToken ct) =>
{
    var lista = await handler.ExecutarAsync(new ListarClientesQuery(pagina, tamanhoPagina, ativo, nome), ct);
    return Results.Ok(lista);
})
.WithName("ListarClientes");

app.MapDelete("/clientes/{id:guid}", async (
    Guid id,
    DesativarClienteCommandHandler handler,
    CancellationToken ct) =>
{
    var resultado = await handler.ExecutarAsync(new DesativarClienteCommand(id), ct);

    if (!resultado.Sucesso)
        return Results.NotFound(new { erros = resultado.Erros });

    return Results.NoContent();
});


app.Run();

public record CriarClienteRequest(string NomeFantasia, string Cnpj);

public record AtualizarClienteRequest(string NomeFantasia, bool Ativo);
