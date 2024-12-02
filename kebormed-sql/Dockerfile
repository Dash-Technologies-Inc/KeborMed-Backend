FROM mcr.microsoft.com/mssql/server:2019-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=AdminStrong@Passw0rd

USER root

RUN apt-get update && \
    apt-get install -y curl apt-transport-https software-properties-common && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    add-apt-repository "$(curl -s https://packages.microsoft.com/config/ubuntu/20.04/prod.list)" && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

ENV PATH=$PATH:/opt/mssql-tools/bin

COPY database/init.sql /init.sql

COPY entrypoint.sh /usr/src/app/entrypoint.sh
RUN chmod +x /usr/src/app/entrypoint.sh

CMD ["/bin/bash", "/usr/src/app/entrypoint.sh"]
