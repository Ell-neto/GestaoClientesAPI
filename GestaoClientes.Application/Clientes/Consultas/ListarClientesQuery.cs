using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Application.Clientes.Consultas;

public record ListarClientesQuery(int Pagina = 1, int TamanhoPagina = 10, bool? Ativo = null, string? Nome = null);
