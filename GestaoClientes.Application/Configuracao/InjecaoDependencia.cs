using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Clientes.Comandos;
using GestaoClientes.Application.Clientes.Consultas;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoClientes.Application.Configuracao;

public static class InjecaoDependencia
{
    public static IServiceCollection AdicionarAplicacao(this IServiceCollection services)
    {
        services.AddScoped<CriarClienteCommandHandler>();
        services.AddScoped<ObterClientePorIdQueryHandler>();
        return services;
    }
}
