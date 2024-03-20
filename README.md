# FolderSync
Este é um projeto de sincronização de pastas desenvolvido como parte de um processo seletivo para a vaga de QA Developer para a empresa Veeam .

## 🚀 Solicitação
```
Implemente um programa que sincronize duas pastas: origem e réplica. 
O programa deve manter uma cópia completa e idêntica da pasta de origem na pasta de réplica.

- A sincronização deve ser unidirecional: após o conteúdo de sincronização do
a pasta de réplica deve ser modificada para corresponder exatamente ao conteúdo da fonte
pasta;

- A sincronização deve ser realizada periodicamente.

- As operações de criação/cópia/remoção de arquivos devem ser registradas em um arquivo e na
saída do console;

- Caminhos de pasta, intervalo de sincronização e caminho do arquivo de log devem ser fornecidos
usando os argumentos da linha de comando;

- É indesejável usar bibliotecas de terceiros que implementem pastas
sincronização;

- É permitido (e recomendado) usar bibliotecas externas implementando outros
algoritmos bem conhecidos. Por exemplo, não faz sentido implementar ainda
outra função que calcula MD5 se você precisar para a tarefa – é
perfeitamente aceitável usar uma biblioteca de terceiros (ou integrada)
```

## 📋 Pré-requisitos

Resolva a tarefa de teste escrevendo um programa em uma destas linguagens de programação:</br>
<code>Python
C/C++
C#</code>

Além da linguagem utilizada (C#) foi utilizado linha de comando para inserir os parâmetros do usuário.

## 🔧 Instalação e Uso

<ol>
  <li>Clone o repositório:</li>
  <code>git clone https://github.com/GiovaniDamian/FolderSync.git</code>
  <li>Navegue até o diretório do projeto:</li>
  <code>cd FolderSync</code>
  <li>Compile o projeto:</li>
  <code>dotnet build</code>
  <li>Execute o aplicativo:</li>
  <code>dotnet run</code>
</ol>


## 🛠️ Construído com

* [C#]([https://maven.apache.org/](https://learn.microsoft.com/pt-br/dotnet/csharp/))
* [Visual Studio]([http://www.dropwizard.io/1.0.2/docs/](https://visualstudio.microsoft.com/pt-br/))

## 🖇️ Colaboração

https://github.com/VeeamHub
<a href="https://www.veeam.com/br">Veeam</a>

## 📦 Implantação

<h3>Solicitação inicial do programa:</h3></br>
<img height="360em" src="https://github.com/GiovaniDamian/FolderSync/assets/60575219/65fcac49-9221-451b-88f4-776e14622f17"/></br>
<h3>Ao informar os repositórios será realizada a sincronição com a periodicidade informada:</h3></br>
<img height="360em" src="https://github.com/GiovaniDamian/FolderSync/assets/60575219/4aaf42f9-2620-49c9-92f9-41f493112c8b"/></br>
<h3>Cada ação será salva no arquivo Log:</h3></br>
<img height="360em" src="https://github.com/GiovaniDamian/FolderSync/assets/60575219/54d77302-b58c-44d6-8ab4-11c6b2155999"/>
obs.: Se não houver log File Path o programa criará.

## 📄 Licença

Este projeto é licenciado sob a <a href="LICENSE">MIT License</a>.

## ⚙️ Executando os testes
Os testes automatizados são realizados utilizando uma função chamada TestSynchronizeFolders(), onde são configuradas as condições iniciais, executada a função a ser testada (SynchronizeFolders()), e então são verificados os resultados para garantir que o comportamento do programa está de acordo com o esperado.

A função de teste segue um padrão geral:

Configuração: São preparados os dados e o ambiente necessários para o teste. Isso pode incluir a criação de diretórios, arquivos e a definição de outras condições iniciais.

Execução: A função a ser testada é invocada com os dados e parâmetros de entrada necessários. Neste caso, a função SynchronizeFolders() é chamada para sincronizar os diretórios especificados.

Verificação: São realizadas as verificações para garantir que o resultado da execução corresponda ao esperado. Isso pode incluir a verificação da existência de arquivos, do conteúdo dos arquivos, entre outras coisas.

Limpeza: Após o teste, são feitas as ações necessárias para limpar o ambiente e restaurar as condições iniciais, garantindo que os testes subsequentes não sejam afetados por resultados anteriores.

Além disso, ao final dos testes automatizados, é apresentado um resumo do que foi realizado e o usuário é solicitado a pressionar uma tecla para fechar o programa.
<img height="360em" src="https://github.com/GiovaniDamian/FolderSync/assets/60575219/7aa35137-db48-4487-85e2-a0917fc8951d"/>

---
⌨️ por [Giovani Damian]([https://gist.github.com/lohhans](https://github.com/GiovaniDamian)https://github.com/GiovaniDamian) 😊
