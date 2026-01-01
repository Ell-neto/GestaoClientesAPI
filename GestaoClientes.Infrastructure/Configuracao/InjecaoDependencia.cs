using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestaoClientes.Application.Abstracoes;
using GestaoClientes.Infrastructure.Persistencia;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoClientes.Infrastructure.Configuracao;

public static class InjecaoDependencia
{
    public static IServiceCollection AdicionarInfraestrutura(this IServiceCollection services)
    {
        services.AddSingleton<IRepositorioCliente, RepositorioClienteEmMemoria>();
        return services;
    }
}
