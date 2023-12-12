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

### ProdEnv
docker compose -f docker-compose-backend.yml -f docker-compose-devops.yml -f docker-compose-services.yml up -d
docker compose -f docker-compose-backend.yml -f docker-compose-services.yml up -d
docker compose -f docker-compose-services.yml up -d

### build > tag > push container image
cd <service folder>
docker build -t auctionservice_image -f Dockerfile .
docker tag auctionservice_image asnielsen789/auctionservice
docker push asnielsen789/auctionservice

- AuctionService
cd AuctionService
docker build -t auctionservice_image -f Dockerfile .
docker tag auctionservice_image asnielsen789/auctionservice
docker push asnielsen789/auctionservice
cd ..

- BidService
cd BidService
docker build -t bidservice_image -f Dockerfile .
docker tag bidservice_image asnielsen789/bidservice
docker push asnielsen789/bidservice
cd ..

- ItemService
cd ItemService
docker build -t itemservice_image -f Dockerfile .
docker tag itemservice_image asnielsen789/itemservice
docker push asnielsen789/itemservice
cd ..

- AssessmentService
cd AssessmentService
docker build -t assessmentservice_image -f Dockerfile .
docker tag assessmentservice_image asnielsen789/assessmentservice
docker push asnielsen789/assessmentservice
cd ..

- SaleService
cd SaleService
docker build -t salesservice_image -f Dockerfile .
docker tag salesservice_image asnielsen789/salesservice
docker push asnielsen789/salesservice
cd ..

- CustomerService
cd CustomerService
docker build -t customerservice_image -f Dockerfile .
docker tag customerservice_image asnielsen789/customerservice
docker push asnielsen789/customerservice
cd ..

### rabbitMQ
docker pull rabbitmq:management

- local test: 
docker run -d --name dev-rabbit --hostname rabbitmq-dev -p 15672:15672 -p 5672:5672 rabbitmq:management

- otherwise add to docker compose

(tutorial for code: https://www.rabbitmq.com/tutorials/tutorial-one-python.html)

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




