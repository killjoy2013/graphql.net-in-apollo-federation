# This is a proof of concept repo trying to use a Graphql.net 4 webapi as a subgraph inside an Apollo Server Graphql Federation

Modules are;

**_federation-gateway (http://localhost:3000/graphql) :_** Apollo federaion gateway. Created with NestJS / NodeJS

**_country-subgraph (http://localhost:3001/graphql) :_** Created with NestJS / NodeJS

**_food-subgraph (http://localhost:3002/graphql) :_** Created with NestJS / NodeJS

**_starwars-subgraph (http://localhost:3003/graphql) :_** Created with GraphQL.Net 4.7.1

Running a `npm cache clean -f` before installing npm dependencies strongly advised.

1 - Inside the directories **federation-gateway**, **country-subgraph** and **food-subgraph** run `npm install`

2 - Inside **starwars-subgraph/WebApi** run `dotnet restore` & `dotnet build`

3 - Initially federation-gateway uses **country** & **food** subgraphs. Inside their directories run `yarn start:dev`. You can check their individual graphs navigating to `http://localhost:3001/graphql` & `http://localhost:3002/graphql`

4 - Check federation-gateway;

_federation-gateway/src/app.module.ts_

```ts
      gateway: {
        buildService({ name, url }) {
          return new RemoteGraphQLDataSource({
            url,
          });
        },
        supergraphSdl: new IntrospectAndCompose({
          subgraphs: [
            {
              name: 'country',
              url: 'http://localhost:3001/graphql',
            },
            {
              name: 'food',
              url: 'http://localhost:3002/graphql',
            },
            // {
            //   name: 'starwars',
            //   url: 'http://localhost:3003/graphql',
            // },
          ],
        }),
      },
```

starwars subgraph is commented out for now. Start gateway by running `yarn start:dev`

5 - Navigate to `http://localhost:3000/graphql`. You can see queries & mutations form both `country` & `food` subgraphs. They can be run, but they don't have actual implementations.

![two-subgraphs](https://dev-to-uploads.s3.amazonaws.com/uploads/articles/q1noxzt07kvuyawwyzet.png)

6 - Let's try to add starwars subgraph to gateway. In `starwars-subgraph\WebApi` run `dotnet run`. You can see the individual graph in `http://localhost:3003/graphiql-ui`

![starwars](https://dev-to-uploads.s3.amazonaws.com/uploads/articles/qghrcqsf1ki26p2hwogn.png)

7 - Now, comment out `starwars` part in `federation-gateway/src/app.module.ts` under subgraphs.

8- When you save `federation-gateway/src/app.module.ts` file, gateway will restart automatically. Below errors happens;

```ts
(node:3892) UnhandledPromiseRejectionWarning: Error: Couldn't load service definitions for "starwars"
    at C:\murat\graphql.net-apollo-federation\federation-gateway\node_modules\@apollo\gateway\src\supergraphManagers\IntrospectAndCompose\loadServicesFromRemoteEndpoint.ts:77:15
    at processTicksAndRejections (internal/process/task_queues.js:95:5)
    at async Promise.all (index 2)
    at loadServicesFromRemoteEndpoint (C:\murat\graphql.net-apollo-federation\federation-gateway\node_modules\@apollo\gateway\src\supergraphManagers\IntrospectAndCompose\loadServicesFromRemoteEndpoint.ts:81:30)
    at IntrospectAndCompose.updateSupergraphSdl (C:\murat\graphql.net-apollo-federation\federation-gateway\node_modules\@apollo\gateway\src\supergraphManagers\IntrospectAndCompose\index.ts:95:20)
    at IntrospectAndCompose.initialize (C:\murat\graphql.net-apollo-federation\federation-gateway\node_modules\@apollo\gateway\src\supergraphManagers\IntrospectAndCompose\index.ts:65:30)
    at ApolloGateway.initializeSupergraphManager (C:\murat\graphql.net-apollo-federation\federation-gateway\node_modules\@apollo\gateway\src\index.ts:456:22)
    at ApolloGateway.load (C:\murat\graphql.net-apollo-federation\federation-gateway\node_modules\@apollo\gateway\src\index.ts:351:7)
    at SchemaManager.start (C:\murat\graphql.net-apollo-federation\federation-gateway\node_modules\apollo-server-core\src\utils\schemaManager.ts:111:22)
    at ApolloServer._start (C:\murat\graphql.net-apollo-federation\federation-gateway\node_modules\apollo-server-core\src\ApolloServer.ts:359:24)
```
