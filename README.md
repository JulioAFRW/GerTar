PREPARAÇÃO DO BANCO DE DADOS NO SQLSERVER:

Rodar o seguinte script:

###INICIO###

USE [GERTAR]

GO
/****** Object:  Table [dbo].[TB_PROJETO_TAREFAS]    Script Date: 09/11/2025 19:00:42 ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[TB_PROJETO_TAREFAS](

	[ID_PROJETO] [int] NOT NULL,
	
	[ID_TAREFA] [int] NOT NULL,
	
	[TITULO] [varchar](50) NOT NULL,
	
	[DESCRICAO] [varchar](2000) NULL,
	
	[PRIORIDADE] [varchar](10) NOT NULL,
	
	[VENCIMENTO] [datetime] NOT NULL,
	
	[COMENTARIO] [varchar](2000) NULL,
	
	[STATUS_TAREFA] [varchar](20) NOT NULL,
	
	[DT_ATUALIZACAO] [datetime] NOT NULL,
	
	[ID_USUARIO] [int] NOT NULL,
	
 CONSTRAINT [PK_TB_PROJETO_TAREFAS] PRIMARY KEY CLUSTERED 
 
(
	[ID_PROJETO] ASC,
	
	[ID_TAREFA] ASC
	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TB_PROJETO_TAREFAS_HIST]    Script Date: 09/11/2025 19:00:42 ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_PROJETO_TAREFAS_HIST](
	[ID_HISTORICO] [int] IDENTITY(1,1) NOT NULL,
	[ID_PROJETO] [int] NOT NULL,
	[ID_TAREFA] [int] NOT NULL,
	[TITULO] [varchar](50) NOT NULL,
	[DESCRICAO] [varchar](2000) NULL,
	[PRIORIDADE] [varchar](10) NOT NULL,
	[VENCIMENTO] [datetime] NOT NULL,
	[COMENTARIO] [varchar](2000) NULL,
	[STATUS_TAREFA] [varchar](20) NOT NULL,
	[DT_ATUALIZACAO] [datetime] NOT NULL,
	[ID_USUARIO] [int] NOT NULL,
 CONSTRAINT [PK_HISTORICO] PRIMARY KEY CLUSTERED 
(
	[ID_HISTORICO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_PROJETOS]    Script Date: 09/11/2025 19:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_PROJETOS](
	[ID_PROJETO] [int] IDENTITY(1,1) NOT NULL,
	[NM_PROJETO] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TB_PROJETOS] PRIMARY KEY CLUSTERED 
(
	[ID_PROJETO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_USUARIOS]    Script Date: 09/11/2025 19:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_USUARIOS](
	[ID_USUARIO] [int] IDENTITY(1,1) NOT NULL,
	[NM_USUARIO] [varchar](50) NOT NULL,
	[PERFIL] [varchar](20) NOT NULL,
 CONSTRAINT [PK_TB_USUARIOS] PRIMARY KEY CLUSTERED 
(
	[ID_USUARIO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TB_PROJETO_TAREFAS] ADD  DEFAULT (getdate()) FOR [DT_ATUALIZACAO]
GO

###ENCERRAMENTO###

CONEXÃO DO BANCO DE DADOS (appsettings.json):
  (Ajustar o nome do servidor e a forma de autenticação)
  "ConnectionStrings": {
    "SqlServerConnectionString": "Data Source=DIGITOTAL-001;Initial Catalog=GERTAR;Integrated Security=True;Encrypt=False"
  }

API CODING:
Instruções para execução via terminal:
1) Navegue até o diretório do projeto : Abra o terminal e use o comando cd para mudar para o diretório onde seu projeto está localizado.
   cd caminho/para/WebAPI
2) Restaurar as dependências : Execute o comando a seguir para garantir que todas as dependências do projeto sejam restauradas.
   dotnet restore
3) Executar a aplicação: Execute o seguinte comando:
   dotnet run

Uma vez que a API esteja em execução, o teste pode ser feito usando ferramentas como cURL, Postman ou mesmo um navegador, acessando os endpoints apropriados, como http://localhost:5000/api/[controller].

REFINAMENTO:

Perguntas a serem dirigidas ao PO visando o refinamento para futuras implementações ou melhorias:

Quais são as operações de negócio permitidas por projeto e por tarefa (criar, editar, arquivar, reabrir, excluir fisicamente vs soft delete)?

Qual o ciclo de vida de um projeto e de uma tarefa? Quais estados possíveis e transições permitidas?

Existem regras de validação de negócio (por ex. uma tarefa não pode ser concluída se subtarefas abertas existirem)?

Quando uma alteração deve gerar eventos/notifications (e-mails, webhooks, mensagens)? Ex.: mudança de status, atribuição, prazo alterado.

Regras de propriedade: quem pode criar/editar/atribuir/arquivar projetos e tarefas?

PONTOS DE MELHORIA NO PROJETO:

Separação de responsabilidades: mover lógica de negócio/validação para services e/ou camadas de domínio; controllers devem orquestrar apenas.

Model binding/DTOs: evitar usar entidades de EF diretamente na API — usar DTOs/VMs para entrada/saída.

Validação: usar validação estruturada (FluentValidation / DataAnnotations) em vez de checks manuais.

Erros e códigos HTTP: usar IActionResult/ActionResult com retornos mais apropriados e padronizar erros (ProblemDetails/ProblemDetailsFactory).

Concurrency/Transações: tratar concorrência e usar transações quando apropriado.

Segurança: autorização/autenticação, input validation, evitar exposição de detalhes (InnerException).

Eficiência EF: evitar consultas desnecessárias, projetar queries, usar AsNoTracking para leitura, evitar OrderByDescending para recuperar o registro recém-inserido.

API design RESTful: endpoints verbos/semântica, usar body para POST/PUT em vez de parâmetros na rota para recursos complexos.

Observability/telemetria: logging estruturado, metrics, tracing, health checks.

Infra/Cloud: containerização, CI/CD, secrets management, escalabilidade, banco gerenciado, monitoramento.
