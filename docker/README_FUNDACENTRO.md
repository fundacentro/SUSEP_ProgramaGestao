# SISPG FUNDACENTRO - Docker Compose
## Instalação
**Pré Requisitos:** 
1. Docker 20.10
2. Docker-compose 1.29.2
3. Git 2.25

A instalação foi validada no sistema operacional ```Ubuntu 20.04.3 LTS``` mas não deve haver problemas com outras distribuições, porém não foi validado.

### Realizando clone do Repositório
```bash
cd /opt
git clone https://github.com/fundacentro/SUSEP_ProgramaGestao.git
git checkout docker-codigo-fonte
cd /opt/Sistema_Programa_de_Gestao_Susep/docker
```
### Efetuando Docker Build e Push
A SUSEP disponibiliza uma imagem oficial do produto, no entanto precisamos adicionar uma nova layer alterando o SECLEVEL do openssl, está alteração foi necessária para contornar erros de conexão com o banco de dados.

**Esse step só precisa ser executado uma vez, ou quando for necessário realizar outras modificações.**

**Lembre-se de alterar a tag da imagem em futuras modificações para manter o versionamento da imagens**
```bash
cat <<EOF > Dockerfile.seclevel
FROM ghcr.io/srmourasilva/sistema_programa_de_gestao_susep/sgd:latest
USER root
RUN sed -i 's/SECLEVEL=2/SECLEVEL=1/g' /etc/ssl/openssl.cnf
USER 1000
EOF
```
```bash
docker build -t registryfundacentro/susep_programagestao:v1 -f Dockerfile.seclevel .
docker login -u registryfundacentro -p '***'
docker push registryfundacentro/susep_programagestao:v1
```

## Configuração
### Preparando o docker-compose
Neste passo iremos configurar o arquivo ```docker-compose.yml``` contido no repositório baixado no step **Baixando Repositório da SUSEP** com os valores referentes ao ambiente onde o sistema está sendo publicado.

Configurando compose file para utilizar a imagem que foi compilada no step anterior.
```bash
cd /opt/Sistema_Programa_de_Gestao_Susep/docker
sed -i 's/ghcr.io\/srmourasilva\/sistema_programa_de_gestao_susep\/sgd:latest/registryfundacentro\/susep_programagestao:v1/g' docker-compose.yml
```
Alterar no arquivo ```docker.compose.yaml``` contido no diretório ```/opt/Sistema_Programa_de_Gestao_Susep/docker``` as seguintes configurações especificas do ambiente:
```yaml
- ASPNETCORE_ENVIRONMENT=Homolog
- ConnectionStrings__DefaultConnection=Server=db,1433;Database=programa_gestao;User Id=sa;Password=P1ssw@rd;
- emailOptions__EmailRemetente=no-reply@me.gov.br
- emailOptions__NomeRemetente=Programa de Gestão - ME
- emailOptions__SmtpServer=smtp.me.gov.br
- emailOptions__Port=25
- ldapOptions__Configurations__0__Url=
- ldapOptions__Configurations__0__Port=389
- ldapOptions__Configurations__0__BindDN=CN=Fulano de tal,CN=Users,DC=orgao
- ldapOptions__Configurations__0__BindPassword=
- ldapOptions__Configurations__0__SearchBaseDC=CN=Users,DC=orgao
- ldapOptions__Configurations__0__SearchFilter=(&(objectClass=user)(objectClass=person)(sAMAccountName={0}))
- ldapOptions__Configurations__0__CpfAttributeFilter=
- ldapOptions__Configurations__0__EmailAttributeFilter=
```
### Iniciando a aplicação
Aṕos ajustar os valores dos parametros no arquivo, inicie a aplicação

**OBS:** Os comandos ```docker-compose``` devem ser executados a partir do diretório ```/opt/Sistema_Programa_de_Gestao_Susep/docker```
```bash
docker-compose -f docker-compose.yml up -d
```
Para verificar se todos os containers subiram corretamente execute:
```bash
docker-compose -f docker-compose.yml ps
```
Output esperado
```bash
Name        Command               State       Ports                             
--------------------------------------------------------------------------------------------------
docker_api-gateway_1   dotnet Susep.SISRH.ApiGate ...   Up                                              
docker_traefik_1       /entrypoint.sh --providers ...   Up      80/tcp, 0.0.0.0:80->8000/tcp,:::80->8000/tcp, 0.0.0.0:443->8443/tcp,:::443->8443/tcp
docker_web-api_1       dotnet Susep.SISRH.WebApi.dll    Up 
docker_web-app_1       dotnet Susep.SISRH.WebApp.dll    Up 
```
Para verificar os logs dos modulos execute:
```bash
docker logs -f <nome_servico>
```
O **nome_servico** é o campo name do output do comando ```docker-compose -f docker-compose.yml ps```

Ex: docker logs -f docker_web-app_1

### Validando aplicação
Neste ponto a aplicação já está acessível no endereço IP ou nome DNS do servidor em que os containers estão rodando.

Ex: http://sispghomolog.fundacentro.gov.br

### Alterando configurações
Caso seja necessário realizar alguma alteração no arquivo docker-compose ou qualquer arquivo que faça parte da aplicação, é preciso executar um **restart** dos containers.

Para isso execute:
```bash
docker-compose -f docker-compose.yml down
docker-compose -f docker-compose.yml up -d
```
### Maiores informações
Para obter maiores informações do funcionamento da aplicação, acesse o [README.md](https://github.com/fundacentro/SUSEP_ProgramaGestao/tree/docker-codigo-fonte/docker) do diretório docker.
