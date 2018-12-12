import { setContext } from 'apollo-link-context';
import { GraphQLRequest } from 'apollo-link';

import { AuthService } from '../../core/services/auth.service';

function createAuthMiddleware(authService: AuthService) {
    const authMiddleware = setContext((operation: GraphQLRequest, prevContext: any) => {
        const jwt: string = authService.getJwt();

        if (!jwt) {
            return {};
        } else {
            return {
                headers: { Authorization: `Bearer ${jwt}` }
            };
        }
    });
    return authMiddleware;

}

export { createAuthMiddleware };