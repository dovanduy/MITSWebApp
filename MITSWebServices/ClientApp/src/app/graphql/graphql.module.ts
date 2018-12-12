import { NgModule } from '@angular/core';
import { ApolloModule, APOLLO_OPTIONS, Apollo } from 'apollo-angular';
import { HttpLinkModule, HttpLink, } from 'apollo-angular-link-http';
import { from } from 'apollo-link';
import { InMemoryCache } from 'apollo-cache-inmemory';

import { createAuthMiddleware } from './middlewares/auth';
import { AuthService } from '../core/services/auth.service';
import { environment } from '../../environments/environment';

const uri = environment.graphql_endpoint; // <-- add the URL of the GraphQL server here

export function createApollo(httpLink: HttpLink, authService: AuthService ) {
  const auth = createAuthMiddleware(authService);
  const http = httpLink.create({ uri });

  return {
    link: from([http]) ,
    cache: new InMemoryCache(),
  };
}

@NgModule({
  exports: [ApolloModule, HttpLinkModule],
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory: createApollo,
      deps: [HttpLink],
    },
  ],
})
export class GraphQLModule {}
