# Projeto DIO MAUI

Este é o projeto de um curso da DIO que fiz para me familiarizar com o MAUI. 

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

Para executar o projeto, siga as etapas abaixo:

1. Abra o terminal na pasta raiz do projeto.

2. Execute o seguinte comando para restaurar as dependências:

    ```shell
    dotnet restore
    ```

3. Execute o seguinte comando para iniciar o aplicativo:

    ```shell
    dotnet run
    ```

Isso iniciará o aplicativo MAUI em seu emulador ou dispositivo Android.

## Contribuição

Se você quiser contribuir para este projeto, sinta-se à vontade para abrir uma issue ou enviar um pull request.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).