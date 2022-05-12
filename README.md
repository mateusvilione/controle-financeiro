# Controle Financeiro

Projeto desenvolvido com o objetivo de gerenciar o controle dos gastos e ganhos recebidos de um usuário.

# Tecnologias

Teste Unitário
 - Xunit
 - Moq

Backend
- Dotnet
- EntityFrameworkCore
- DependencyInjection
- PostgreSQL
- Linq

# Getting Started

O projeto foi criado utilizando a versão 3.1 do dotnet core

1. Instalar a versão 3.1 do dotnet core
2. Instalar o cli do Entity Framework
3. Instalar o banco de dados PostgreSQL
4. Clonar o projeto
5. Ir para o diretório do projeto
6. E realizar os comando conforme **Build and Test**

## Build and Test
1. dotnet build
2. dotnet ef database update --project=Api
3. dotnet run
4. Acesse a rota http://localhost:5001/swagger/index.html
5. Para executar os testes de cobertura de código:

``` dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info /p:ExcludeByFile="*/Models/%2c/Model/%2c/Migrations/%2c/Enums/%2c/Migrations/%2c/DbContexts/%2c/Entities/%2c/devops/%2c/Program.cs%2c*/Startup.cs"```

## Healthcheck

Para verificar o healthcheck da aplicação e dos recursos utilizados:

- https://localhost:5001/health
- https://localhost:5001/health/details

## API

HOST: http://localhost:5001/v1