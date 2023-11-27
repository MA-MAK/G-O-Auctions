## general
### logins
admin
qwer1234

## git
### opret feature branch
git flow feature start <branchname>

### push til feature branch
git add .
git commit -m "commit besked"
#### 1. gang
git push -u origin <branchname>
#### efterfølgende
git push

### afslut feature branch
git flow feature finish <branchname>
(merger ind i, og skifter automatisk tilbage til develop branch)
git push

## docker
### network
docker network create GOnet

### DevEnv
docker compose up -d
docker compose up --scale <service>=<number> -d

### build > tag > push container image
cd <service folder>
docker build -t auctionservice_image -f Dockerfile .
docker tag auctionservice_image asnielsen789/auctionservice
docker push asnielsen789/auctionservice

## dotnet
### logging
dotnet add package NLog
dotnet add package NLog.Web.AspNetCore
(add NLog.config, copy from AuctionService)
(add reference to NLog.config in .csproj)
(add using, config and try/catch in program.cs)
dotnet add package NLog.Targets.Loki
(add loki and grafana services to docker compose)
(go to grafana website)
(add datasource: url http://loki:3100)
(create dashboard with filter: app=monitoring)


