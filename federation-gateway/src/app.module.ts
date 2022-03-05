import { IntrospectAndCompose, RemoteGraphQLDataSource } from '@apollo/gateway';
import { ApolloGatewayDriver } from '@nestjs/apollo';
import { Module } from '@nestjs/common';
import { GraphQLModule } from '@nestjs/graphql';
import { ApolloServerPluginLandingPageGraphQLPlayground } from 'apollo-server-core';
import { AppService } from './app.service';

@Module({
  imports: [
    GraphQLModule.forRoot({
      driver: ApolloGatewayDriver,
      playground: false,
      plugins: [
        ApolloServerPluginLandingPageGraphQLPlayground({
          settings: {
            'editor.theme': 'dark',
          },
        }),
      ],
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
          ],
        }),
      },
    }),
  ],
  providers: [AppService],
})
export class AppModule {}
