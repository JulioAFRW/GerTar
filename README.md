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

Model binding/DTOs: não use entidades de EF diretamente na API — use DTOs/VMs para entrada/saída.

Validação: usar validação estruturada (FluentValidation / DataAnnotations) em vez de checks manuais.

Erros e códigos HTTP: usar IActionResult/ActionResult com retornos mais apropriados e padronizar erros (ProblemDetails/ProblemDetailsFactory).

Concurrency/Transações: tratar concorrência e usar transações quando apropriado.

Segurança: autorização/autenticação, input validation, evitar exposição de detalhes (InnerException).

Eficiência EF: evitar consultas desnecessárias, projetar queries, usar AsNoTracking para leitura, evitar OrderByDescending para recuperar o registro recém-inserido.

API design RESTful: endpoints verbos/semântica, usar body para POST/PUT em vez de parâmetros na rota para recursos complexos.

Observability/telemetria: logging estruturado, metrics, tracing, health checks.

Infra/Cloud: containerização, CI/CD, secrets management, escalabilidade, banco gerenciado, monitoramento.
