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