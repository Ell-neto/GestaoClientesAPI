using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Application.Clientes.DTOs;

public record ClienteDto(Guid Id, string NomeFantasia, string Cnpj, bool Ativo);
