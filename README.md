# Projeto Controle-lancamento - .NET Core

Bem-vindo ao Projeto Cashflowmanagement - .NET Core! 
Projeto teste para atender a seguinte descrição:

"Um comerciante precisa controlar o seu fluxo de caixa diário com os lançamentos (débitos e créditos), também precisa de um relatório que disponibilize o saldo diário consolidado."

## Itens

- [Documentação do projeto](https://github.com/glauberfergf/controle-lancamento/tree/main/Documentacao)
- [Collection Postman](https://github.com/glauberfergf/controle-lancamento/tree/main/Postman)
- [Script Banco de dados](https://github.com/glauberfergf/controle-lancamento/tree/main/mysql-init)
- [Solution do projeto](https://github.com/glauberfergf/controle-lancamento/blob/main/CashFlowManagement/CashFlowManagement.sln)

## Características

Com essa descrição e com a liberdade de escolher a forma de desenvolver o projeto, tecnologias e estruturas, optei pelos seguintes itens:

- .Net Core 8 e C# - Utilizamos a versão 8 por ser a versão LTS atual, oferecendo estabilidade e suporte a longo prazo;
- API ASP.NET Core - Usamos o framework de API Rest do .Net Core para criar uma API robusta;
- Worker Service - Implementamos um Worker Service para processar tarefas em segundo plano;
- RabbitMQ - Serviço de mensageria de código aberto, para comunicação entre componentes;
- Mysql - Sistema de gerencimaneto de Banco de dados;
- Docker - Solução de virtualização;
- Aplicação desenvolvida em camadas, com as seguintes estruturas e objetivos/papeis:
	- Domain - Camada responsável pelo dominio da solução, entidades, regras, interfaces, etc...
	- Infrastructure - Camada responsável por interagir diretamente com componentes de infraestrutura (RabbitMQ, banco de dados, etc...).
	- Services - Camada responsável por conter interações externas sem refletir/corromper diretamente a lógica de negocios interna.
	- Application - Camada responsável por manter a lógica de negocio, orquestrar o fluxo da aplicação.
	- Presentation - Camada responsável por conter a(s) parte(s) da solução que são executadas.
	- CrossCutting - Camada responsável por conter aspectos transversais da aplicação facilitando a configurações entre camadas.
	- Test - Camada responsável por conter os testes automatizados da solução.

## Fluxo da solução (Sucesso):

- Para enviar uma order de pagamento para a API Payment (Post)
- A API recebe a requisição, converte o DTO enviado utilizando o AutoMapper e envia para o RabbitMQ para ser processado
- Depois de enviar para o RabbitMQ retorna o status 200 informando que foi recebido a order de pagamento
- A aplicação worker que está conectada ao RabbitMQ recebe a mensagem que foi enviada pela API
- É feito a a transformação da mensagem para o objeto do dominio e enviado para o banco de dados
- Usuário realiza outra chamada de request (GET) para tirar um relatório passando um objeto de filtro com alguns parametros
- A API recebe o request de consulta de relatório
- Realiza a ação passando pelas camadas de Application e Repository para conseguir acessar o banco de dados
- É realizado uma consulta no banco de dados que foi gerada na camada anterior
- Encontrando os dados é feito uma conversão do objeto do banco para um DTO antes de retornar para o usuário que solicitou 

## Desenho da Solução

![Desenho](https://github.com/glauberfergf/controle-lancamento/raw/main/Documentacao/cashflowmanagement.drawio.png)


## Pré-requisitos

Antes de começar, verifique se você atende aos seguintes requisitos:

- .Net Core versão 8 instalado
- Ferramentas necessárias: Docker, Mysql Workbench (ou equivalente se desejar)
- Clone o repositório para sua máquina local:
		git clone https://github.com/glauberfergf/controle-lancamento.git
	- Entra no diretorio do projeto
	- Abrir o terminal/CMD na pasta do projeto e rodar os seguintes comandos;
		docker run -d --name mysql-container -e MYSQL_ROOT_PASSWORD=senha_forte! -p 3306:3306 mysql:latest

		docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
	- Conectar no banco de dados e executar o arquivo que está na pasta /mysql-init
	
## Como Executar o Projeto

Siga estas etapas para executar o projeto em sua máquina:

1. Clone o repositório para sua máquina local:
   git clone https://github.com/glauberfergf/controle-lancamento.git
   
2. Entra no diretorio do projeto

3. Abrir a solution da aplicação conferir se a configuração para rodar multiplas aplicações está habilitado e rodar.
Irá executar a API e o Worker

5. Na pasta Postman que está na raiz, encontra-se a collection do Postman com as chamadas e exemplos para utilizar
   	- Executar a chamada "CreateAsync - SendToQueue"
   	- Executar a chamda "GetByFilter" alterando a data para retornar o relatório com os lançamentos criados

7. Na pasta mysql-init, encontra-se o script de criação do banco de dados e tabela utilizada na solução (script possui validação). Ao subir a solução pelo docker-compose já irá ser executado o script.

8. Na pasta Documentacao, encontra-se o desenho da aplicação que foi feito utilizando o draw.io

9. As aplicações irão abrir nos seguintes endereços:
	- RabbitMQ: http://localhost:15672/
	- Mysql: Server=localhost
	- Cashflowmanagement.API: http://localhost:5085/swagger/index.html
	
Certifique-se de que todos os pré-requisitos estejam instalados e siga estas etapas para executar o projeto em sua máquina.
