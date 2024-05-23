# Projeto DIO MAUI

Este é o projeto de um curso da DIO que fiz para me familiarizar com o .NET MAUI.

O .NET MAUI usa as tecnologias mais recentes para criar aplicativos nativos no Windows, macOS, iOS e Android, abstraindo-os em uma estrutura comum criada no .NET. Conheci essa tecnologia no VS Summit de 2023, trabalhava há algum tempo já no desenvolvimento web com C#/.NET, achei incrível a possibilidade de atuar em outros segmentos sem ter que me adaptar a outra linguagem.

O projeto em questão se trata de um gerenciador de tarefas, com funcionalidades como:
- CRUD das tarefas
- Acesso à câmera e opção de anexar fotos às tarefas
- Acesso ao GPS e opção de adicionar localizações às tarefas
- Opção de verificar no Maps cada localização registrada
- Adicionar comentários às tarefas

Segue gif demonstrando a navegação e as funcionalidades:
![Apresentação da navegação e funcionalidades do app](./Resources/Images/navegacao-app.gif)

## Pré-requisitos

Antes de começar, certifique-se de ter os seguintes requisitos instalados em sua máquina:

- .NET 8 SDK
- Templates do MAUI compatíveis com o .NET 8
- Android Studio com as SDKs necessárias para o MAUI

## Instalação

Siga as etapas abaixo para configurar o ambiente de desenvolvimento:

1. Instale o .NET 8 SDK. Você pode encontrar as instruções de instalação em [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0).

2. Instale os templates do MAUI compatíveis com o .NET 8. Você pode usar o seguinte comando no terminal:

    ```shell
    dotnet new install Microsoft.Maui.Templates.net8::8.0.40
    ```

3. Instale o Android Studio e configure as SDKs necessárias para o MAUI. Você pode encontrar as instruções de instalação em [developer.android.com/studio](https://developer.android.com/studio).

4. Instale o Xcode versão 15 ou superior caso deseje fazer deploy para iOS. Você pode encontrar o Xcode na App Store ou no site oficial da Apple.

## Executando o projeto

Para executar o projeto, já tendo um emulador iniciado na sua máquina ou com seu celular vinculado para debug, siga as etapas abaixo:

1. Abra o terminal na pasta raiz do projeto.

2. Execute o seguinte comando para restaurar as dependências:

    ```shell
    dotnet restore
    ```

3. Execute o seguinte comando para iniciar o aplicativo:

    ```shell
    dotnet build -t:Run -f net8.0-android
    ```

Isso iniciará o aplicativo MAUI em seu emulador ou dispositivo Android.

Caso já tenha emuladores configurados, mas deseje que sua inicialização seja feita programaticamente, execute o script abaixo:

```shell
./run.sh
```

O script `run.sh` irá verificar os emuladores disponíveis na sua máquina, iniciar um emulador e então executar a aplicação nele. Gostaria de ter implementado a criação de um novo emulador via command line caso não existisse nenhum, mas não consegui resolver isso ainda.

## Dependências

- sqlite-net-pcl
