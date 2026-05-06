FROM mcr.microsoft.com/mssql/server:2022-latest

USER root

RUN mkdir -p /usr/config
WORKDIR /usr/config

COPY ./infra/sql/init-db.sql ./
COPY ./infra/sql/entrypoint.sh ./

RUN sed -i 's/\r$//' ./entrypoint.sh

RUN chmod +x ./entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]