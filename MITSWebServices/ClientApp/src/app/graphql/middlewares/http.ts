import { environment } from '../../../environments/environment';
import { HttpLinkModule, HttpLink, } from 'apollo-angular-link-http';

const API_ENDPOINT = environment.graphql_endpoint;

function createHttpLink(httpLink: HttpLink) {
  const http = httpLink.create({
    uri: API_ENDPOINT
  });

  return http;
}

export { createHttpLink };