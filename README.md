# BancoKRT.Api

API em `.NET 8` para gerenciamento de contas. Este repositório inclui orquestração de infraestrutura (SQL Server, Redis, Kafka) em `Docker/docker-compose.yml` e scripts de inicialização.

Requisitos
- Docker e Docker Compose
- .NET 8 SDK
- Git

Serviços providos pelo `Docker/docker-compose.yml`
- SQL Server: `localhost:1433` (banco: `BancoKrt`)
- Adminer (UI DB): `http://localhost:8081`
- Redis: `localhost:6379`
- Redis Insight: `http://localhost:5540`
- Zookeeper: `localhost:2181`
- Kafka: `localhost:29092`
- Kafka UI: `http://localhost:8080`

Valores padrão usados no compose
- SA password: `YourStrong!Passw0rd`
- String de conexão exemplo:
  `Server=localhost,1433;Database=BancoKrt;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;`

Executando com Docker (recomendado)
1. Clonar o repositório:
   `git clone https://github.com/Calebe1100/BancoKRT.Api.git`
2. Entrar no diretório do projeto:
   `cd BancoKRT.Api`
3. Subir a infraestrutura:
   `docker compose -f Docker/docker-compose.yml up -d --build`
   - O serviço `sqlserver-init` cria o banco `BancoKrt` e a tabela `Accounts` se necessário.
4. Rodar a API (opcional, se a imagem não iniciar automaticamente):
   - No Visual Studio: abrir a solução e executar.
   - Ou pela CLI:
     `dotnet run --project BancoKRT.Api`

Executando localmente (sem Docker)
1. Tenha SQL Server, Redis e Kafka disponíveis e ajuste `appsettings.json` com as conexões.
2. Restaurar e executar a API:
   `dotnet restore`
   `dotnet run --project BancoKRT.Api`

Comandos úteis
- Parar e remover containers/volumes:
  `docker compose -f Docker/docker-compose.yml down -v`
- Ver logs de um serviço (ex.: sqlserver):
  `docker compose -f Docker/docker-compose.yml logs -f sqlserver`

Observações
- A senha SA usada no compose é apenas para desenvolvimento. Troque por uma senha forte em produção.
- Verifique `appsettings.json` antes de executar localmente.
- UIs úteis: Adminer (`http://localhost:8081`), Redis Insight (`http://localhost:5540`), Kafka UI (`http://localhost:8080`).

Licença
- Use conforme as regras do repositório.
