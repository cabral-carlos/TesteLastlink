# Enterprise API - LastLink

## API em C# (.NET) - Guia de Configuração e Execução


### Especificações
- Visual Studio Code (https://code.visualstudio.com/) ou no mínimo Visual Studio 2019
- Caso utilize VS Code, extensão c# (https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
- .NET 5 (https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/) (para clonar o projeto)

---
### Clonando o repositório
Como o git instalado, crie uma pasta em seu computador onde ficará o projeto. Logo após isso, abra o terminal e vá até o caminho da sua pasta e digite o comando `git clone https://github.com/cabral-carlos/TesteLastlink`

---
### Dependências
- "Microsoft.AspNetCore.Mvc.Versioning" Version="5.0.0"
- "Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer" Version="5.0.0"
- "Microsoft.Data.Sqlite.Core" Version="5.0.0" 
- "Microsoft.EntityFrameworkCore" Version="5.0.0"
- "Microsoft.EntityFrameworkCore.Design" Version="5.0.0"
- "Microsoft.EntityFrameworkCore.Sqlite" Version="5.0.0" 
- "Microsoft.EntityFrameworkCore.InMemory" Version="5.0.0" 
- "Microsoft.EntityFrameworkCore.Tools" Version="5.0.0"
- "Swashbuckle.AspNetCore" Version="5.6.3"

Para instalar no VSCode, utilize as instruções desse link
https://code.visualstudio.com/docs/csharp/package-management

Para instalar no Visual Studio, utilize as instruções desse link
https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio

---
### Configurações

#### VSCode
Depois das dependências instaladas, execute os comandos via terminal:
```bash
dotnet restore
```
```bash
dotnet ef database update
``` 

#### Visual Studio
Execute o comando no _Package Manager Console_:
```bash
update-database
``` 

---
### Execução

#### VSCode
Execute no terminal:
```bash
dotnet run
```

Verifique no terminal essas linhas, aqui mostra a informação em qual rota estará a aplicação
```
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
```

Acesse no navegador a documentação via swagger, como no exemplo abaixo:\
http://localhost:5000/swagger/

#### Visual Studio
Defina o projeto da API como **projeto de inicialização**.
Pressione **F5** para executar em modo _debug_ ou **Ctrl+F5** para executar sem este modo.

O projeto via IIS Express deverá abrir uma página no navegador da documentação via swagger, como no exemplo abaixo:\
https://localhost:44362/swagger/index.html