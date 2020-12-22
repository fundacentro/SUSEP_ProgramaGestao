# Sistema_Programa_de_Gestao_Susep


## Subindo aplicação parcialmente com o Docker

Requerimentos:
* SDK do dotnet: https://download.visualstudio.microsoft.com/download/pr/3366b2e6-ed46-48ae-bf7b-f5804f6ee4c9/186f681ff967b509c6c9ad31d3d343da/dotnet-sdk-3.1.404-win-x64.exe
* Docker: https://docs.docker.com/docker-for-windows/install/

### `Susep.SISRH.WebApi`

```
# Copiar arquivos de configuração
cp '.\install\Arquivos de configuração\1. Susep.SISRH.WebApi\Settings\' '.\install\1. Susep.SISRH.WebApi\'
cp '.\install\Arquivos de configuração\1. Susep.SISRH.WebApi\web.config' '.\install\1. Susep.SISRH.WebApi\web.config'
# Altere os arquivos presentes em install\1. Susep.SISRH.WebApi\
#  Obs: Em connectionstrings.Homolog.json, DefaultConnection deve ser alterado para
#    "DefaultConnection": "Server=localhost;Database=master;User Id=sa;Password=P1ssw@rd;"
#  Veja mais exemplos de configuração em https://www.connectionstrings.com/sql-server/

cd install\1. Susep.SISRH.WebApi
$Env:ASPNETCORE_ENVIRONMENT="Homolog"; dotnet .\Susep.SISRH.WebApi.dll --server.urls=http://0.0.0.0:5000
```

### SQL Server, `Susep.SISRH.ApiGateway` e `Susep.SISRH.WebApi`

Em outro terminal:

```
docker-compose up
```