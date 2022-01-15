FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS builder

COPY ./ ./

WORKDIR ./AnkiBot

RUN dotnet build --runtime alpine-x64

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine

COPY --from=builder /AnkiBot/bin/Debug/net5.0/alpine-x64 ./bot

CMD ["./bot/AnkiBot"]