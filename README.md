# MiniMercado SaaS - Backend API 🛒

Uma solução backend completa para sistemas de Pequenos Mercados e Pontos de Venda (PDV), construída com foco em **Clean Architecture**, **Domain-Driven Design (DDD)** e **Processamento Assíncrono**.

## 🏗️ Estrutura do Projeto

O projeto é dividido em quatro camadas principais, garantindo separação de preocupações e facilidade de testes:

1.  **MiniMercadoSaas.API**: 
    - Exposição de Endpoints REST.
    - Configuração de Middlewares (Autenticação, Exceções).
    - Injeção de Dependência e Configurações de Host.
2.  **MiniMercadoSaas.Application**:
    - **Casos de Uso**: Serviços que orquestram a lógica da aplicação.
    - **DTOs**: Objetos de transferência de dados para Request e Response.
    - **Validadores**: Implementações robustas com FluentValidation.
    - **Mappings**: Transformação entre entidades e DTOs.
3.  **MiniMercadoSaas.Domain**:
    - **Entidades**: Modelos de domínio ricos.
    - **Enums**: Definições de Status de Venda, Tipos de Movimentação e Roles.
    - **Contratos**: Definição de interfaces de repositório e eventos de mensageria.
4.  **MiniMercadoSaas.Infrastructure**:
    - **Persistência**: Implementação do EF Core com MySQL.
    - **Repositórios**: Implementação do padrão Repository para desacoplamento do banco.
    - **Mensageria**: Consumidores do MassTransit para processamento de eventos.

## 🚀 Stack Tecnológica

- **Linguagem**: C# (.NET 9.0)
- **Banco de Dados**: MySQL 8.0+
- **ORM**: Entity Framework Core
- **Mensageria**: MassTransit (RabbitMQ ou InMemory)
- **Segurança**:
    - **JWT**: Tokens de acesso para autenticação stateless.
    - **BCrypt.Net**: Hashing criptográfico de senhas (salting automático).
- **Validação**: FluentValidation (validações fluídas de regras de entrada).
- **Documentação**: Swagger (OpenAPI 3.0).

## 🔐 Segurança e Controle de Acesso

O sistema utiliza RBAC (**Role-Based Access Control**) com três níveis de acesso:
- `Admin`: Acesso total ao sistema, gerenciamento de usuários e configurações.
- `Gerente`: Gerenciamento de estoque, visualização de relatórios e permissão para cancelar vendas.
- `Operador`: Acesso ao PDV para abertura de vendas, adição de itens e finalização.

> [!IMPORTANT]
> Todas as senhas são armazenadas utilizando o algoritmo **BCrypt**, garantindo proteção contra ataques de dicionário e rainbow tables através de saltos criptográficos únicos.

## 🛠️ Funcionalidades e Fluxos

### 🛒 Fluxo de Venda (PDV)
1. **Abertura**: O operador inicia uma venda vinculada ao seu ID.
2. **Adição de Itens**: Adição de produtos validando estoque em tempo real. Cada item adicionado recalcula o total da venda.
3. **Finalização**: A venda é fechada, o estoque é subtraído e uma movimentação de auditoria é criada.
4. **Alerta de Estoque**: Se o estoque atingir o nível mínimo, um evento assíncrono é disparado.

### 📦 Gestão de Estoque
- **Auditoria**: Cada alteração no estoque gera um registro na tabela `MovimentacoesEstoque`, identificando quem fez a alteração, o motivo (venda, entrada manual, cancelamento) e a quantidade.
- **Entrada Manual**: Permite adicionar saldo ao estoque de produtos existentes.

## 📡 Mensageria (MassTransit)

O sistema utiliza o padrão **Pub/Sub** para notificações de estoque baixo. 
- **Publisher**: `VendaService` publica o evento `EstoqueBaixoEvent`.
- **Subscriber**: `EstoqueBaixoConsumer` recebe o evento sem bloquear o fluxo principal da venda.

## ⚙️ Guia de Instalação e Execução

### 1. Requisitos
- .NET 9.0 SDK
- MySQL Server 8.0
- Docker (Opcional, para RabbitMQ)

### 2. Configuração do Banco
Altere o `appsettings.json` com suas credenciais:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost; database=MiniMercadoSaasDB; user=root; Password=SUA_SENHA;"
}
```

### 3. Migrations
```bash
dotnet ef database update --project MiniMercadoSaas.Infrastructure --startup-project MiniMercadoSaas.API
```

### 4. Execução
```bash
dotnet run --project MiniMercadoSaas.API
```

---
Este projeto foi desenvolvido seguindo padrões de mercado para sistemas SaaS, focado em performance e manutenibilidade.
