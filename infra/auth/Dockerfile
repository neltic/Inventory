FROM quay.io/keycloak/keycloak:latest

COPY ./infra/auth/realm-export.json /opt/keycloak/data/import/realm-export.json

CMD ["start-dev", "--import-realm"]