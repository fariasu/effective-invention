# Notification Service

Este repositório contém um **serviço de notificações** desenvolvido com **ASP.NET Core**. O objetivo principal é gerenciar o envio de notificações por diferentes canais, como e-mail e SMS. Atualmente, a funcionalidade implementada é o envio de notificações por **e-mail**.

---

## Tecnologias Utilizadas

- **ASP.NET Core**: Framework principal para criação da API.
- **Entity Framework Core**: Persistência e manipulação de dados.
- **RabbitMQ** + **MassTransit**: Gerenciamento de filas e mensagens.
- **Redis**: Cache para controle de operações e dados temporários.
- **MediatR**: Organização de comandos e eventos com o padrão CQRS.
- **FluentMigrator**: Controle de versionamento e estrutura do banco de dados.
- **FluentValidation**: Validação de dados de entrada.
- **Swagger**: Documentação interativa para a API.

---

## Como Funciona

1. **Recepção de Solicitação**  
   - A API expõe um endpoint para receber dados de notificações.
   - As informações necessárias incluem:
     - Destinatário (e-mail)
     - Canal da mensagem (email, sms)
     - Mensagem

2. **Processamento Assíncrono**  
   - A solicitação é validada e enviada para uma fila no **RabbitMQ**.
   - O **MassTransit** gerencia o consumo da fila e o processamento das notificações.

3. **Envio de E-mails**  
   - As notificações são enviadas via e-mail utilizando o canal configurado.
   - Logs detalhados são gerados para cada envio.

4. **Cache com Redis**  
   - Utilizado para armazenar informações temporárias e otimizar o fluxo de notificações.

---

## Como Executar o Projeto

### Pré-requisitos

- **Docker** e **Docker Compose** instalados.

### Passos

1. Clone este repositório:
   ```bash
   git clone https://github.com/fariasu/notification-service.git
   cd notification-service
    ```
   
2. Suba os containers:
    ```bash
    docker-compose up -d
    ```
   
2. Baixe as dependências e inicie a aplicação:
    ```bash
    dotnet restore
    dotnet run
    ```

