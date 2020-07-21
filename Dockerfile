FROM alpine:latest

WORKDIR /app

COPY ./dist/listat.exe /app

EXPOSE 80

ENTRYPOINT [ "./listat.exe" ]