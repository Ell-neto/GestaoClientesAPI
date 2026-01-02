# 🚀 Gestão de Clientes API (Desafio) — .NET 9 + Clean Architecture + CQRS

Bem-vindo(a), sou o Manoel! 😄  
Este repositório contém uma API simples e funcional para **Cadastro e Consulta de Clientes**, construída em **.NET 9 (C# 13)** seguindo **Clean Architecture**, **CQRS** e boas práticas de organização e testes unitários, tudo de acordo com o desafio proposto!

---

## 🎯 Objetivo do Desafio

Implementar uma **feature slice** (fatia vertical) com:

✅ **Criar um Cliente** (`POST /clientes`)  
✅ **Consultar um Cliente por ID** (`GET /clientes/{id}`)

Com foco em:

- 🧱 Arquitetura limpa (Clean Architecture)
- 🔁 Separação de responsabilidades (SOLID)
- 🧠 Modelagem de domínio (Entidade + Value Object)
- 🧪 Testes unitários (xUnit)
- 🧾 Commits e evolução organizada no Git

---

## 🧩 Regras de Negócio Implementadas

### 👤 Entidade `Cliente`
- Possui **Id**, **NomeFantasia**, **Cnpj** e **Ativo**
- Protege invariantes do domínio:
  - ✅ `NomeFantasia` não pode ser vazio

### 🧾 Value Object `Cnpj`
- É um **Value Object**
- Valida:
  - ✅ 14 dígitos
  - ✅ não aceita sequência repetida (ex: 00000000000000)
  - ✅ valida dígitos verificadores (regra oficial)

---

## 🏗️ Arquitetura do Projeto (Clean Architecture)

> Regra de ouro: **dependências apontam para dentro** 🧠

📦 Estrutura de projetos:

- **GestaoClientes.Domain**  
  Regras de negócio e modelos do domínio (`Cliente`, `Cnpj`).

- **GestaoClientes.Application**  
  Casos de uso (CQRS), contratos e Handlers.
  - `CriarClienteCommand` + `CriarClienteCommandHandler`
  - `ObterClientePorIdQuery` + `ObterClientePorIdQueryHandler`

- **GestaoClientes.Infrastructure**  
  Implementações técnicas (detalhes).  
  - Repositório em memória `RepositorioClienteEmMemoria`

- **GestaoClientes.API**  
  Camada HTTP (Minimal API) expondo endpoints.

- **GestaoClientes.Tests**  
  Testes unitários (xUnit) focados na camada Application.

---

## 🧠 CQRS (Comandos e Consultas)

- ✍️ **Command**: altera estado
  - `CriarClienteCommand`

- 🔎 **Query**: apenas consulta
  - `ObterClientePorIdQuery`

📌 Cada operação tem seu **Handler** dedicado.

---

## 💾 Persistência (In Memory)

A implementação atual usa um repositório **em memória**, suficiente para o desafio:

- ✅ rápido e simples
- ✅ permite validar arquitetura e padrão de repositório
- ✅ valida CNPJ único

> Observação: foi projetado para ser facilmente substituído por um banco real (ex: NHibernate/SQLite) no futuro.

---

