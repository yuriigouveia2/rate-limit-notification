#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . .

RUN dotnet publish -c Release -o out RateLimitNotification

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .
EXPOSE 80
ENV ASPNETCORE_URLS=http://+80
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

RUN echo "dotnet RateLimitNotification.Api.dll" >> entrypoint.sh && chmod a+x entrypoint.sh

ENTRYPOINT ["sh", "entrypoint.sh"]
# ENTRYPOINT ["tail", "-f", "/dev/null"]
