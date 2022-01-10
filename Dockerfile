FROM mcr.microsoft.com/dotnet/sdk:5.0

COPY ./ ./

WORKDIR ./AnkiBot

RUN dotnet build

CMD ["./bin/Debug/net5.0/AnkiBot"]