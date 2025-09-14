# 🚀 Hackathon FIAP - Fase 5

## 📦 Subindo os containers

Clone este repositório e, no diretório onde está o
`docker-compose.yaml`, execute:

``` bash
docker-compose up --build
```

------------------------------------------------------------------------

## 🗄️ Configuração inicial do MongoDB

Após o container do **MongoDB** subir pela primeira vez, é necessário
criar o usuário da aplicação (isso deve ser feito **apenas uma vez**):

``` bash
docker exec -it videochecker_mongo mongosh -u root -p rootpass --authenticationDatabase admin
```

No shell do Mongo, execute:

``` javascript
use admin

db.system.users.find({}, {user:1, db:1, roles:1}).pretty()

use filesdb
db.createUser({
  user: 'appuser',
  pwd: 'apppass',
  roles: [{ role: "readWrite", db: "filesdb" }]
})

db.auth("appuser","apppass")
```

Se tudo estiver correto, o retorno será:

``` json
{ ok: 1 }
```

------------------------------------------------------------------------

## ✅ Resumo

1.  Suba os containers com `docker-compose up --build`.
2.  Crie o usuário `appuser` no banco `filesdb` (somente na primeira
    execução).
3.  Confirme a autenticação (`{ ok: 1 }`).

Pronto! O ambiente estará configurado para rodar a aplicação. 🎉
