import { NgModule } from "@angular/core";
import { ApolloModule, APOLLO_OPTIONS, Apollo } from "apollo-angular";
import { HttpLinkModule, HttpLink } from "apollo-angular-link-http";
import { from } from "apollo-link";
import { InMemoryCache } from "apollo-cache-inmemory";
import { setContext } from "apollo-link-context";
import { GraphQLRequest } from "apollo-link";

import { AuthService } from "../core/services/auth.service";
import { environment } from "../../environments/environment";

// const uri = environment.graphql_endpoint; // <-- add the URL of the GraphQL server here

// export function createApollo(httpLink: HttpLink, authService: AuthService ) {
//   const auth = createAuthMiddleware(authService);
//   const http = httpLink.create({ uri });

//   return {
//     link: from([auth]) ,
//     cache: new InMemoryCache(),
//     defaultHttpLink: false
//   };
// }
const API_ENDPOINT = environment.graphql_endpoint;

@NgModule({
  exports: [ApolloModule, HttpLinkModule],
  providers: [
    // {
    //   provide: APOLLO_OPTIONS,
    //   useFactory: createApollo,
    //   deps: [HttpLink],
    // },
  ]
})
export class GraphQLModule {
  constructor(apollo: Apollo, authService: AuthService, httpLink: HttpLink) {
    const auth = setContext((operation: GraphQLRequest, prevContext: any) => {
      const jwt: string = authService.getJwt();

      if (!jwt) {
        console.log("No JWT");
        return {};
      } else {
        console.log("Jwt Found");
        return {
          headers: { Authorization: `Bearer ${jwt}` }
        };
      }
    });

    const http = httpLink.create({
      uri: API_ENDPOINT
    });

    apollo.create({
      link: from([auth, http]),
      cache: new InMemoryCache()
    });
  }
}
