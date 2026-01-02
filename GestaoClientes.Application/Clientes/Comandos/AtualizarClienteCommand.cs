using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Application.Clientes.Comandos;

public record AtualizarClienteCommand(Guid Id, string NomeFantasia, bool Ativo);

