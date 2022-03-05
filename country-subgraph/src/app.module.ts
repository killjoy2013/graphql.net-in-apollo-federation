import { join } from 'path';
import { Module } from '@nestjs/common';
import { GraphQLModule } from '@nestjs/graphql';
import { AppService } from './app.service';
import { CountryModule } from './country/country.module';
import {
  ApolloDriver,
  ApolloFederationDriver,
  ApolloFederationDriverConfig,
} from '@nestjs/apollo';

@Module({
  imports: [
    GraphQLModule.forRoot<ApolloFederationDriverConfig>({
      autoSchemaFile: join(process.cwd(), 'schema.gql'),
      driver: ApolloFederationDriver,
    }),
    CountryModule,
  ],
  controllers: [],
  providers: [AppService],
})
export class AppModule {}
