FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

#EXPOSE 83
# EXPOSE 44352
ENV ASPNETCORE_URLS=http://+:5000

# Set the timezone for the container by setting the TZ environment variable
# to the desired timezone (in this case, America/New_York).
ENV TZ=Asia/Dhaka

# Create a symbolic link from /etc/localtime (used by applications to determine
# the system timezone) to the corresponding timezone file in /usr/share/zoneinfo
# on the host system. This ensures that the container is using the correct timezone.
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
\
# Set the timezone in /etc/timezone to ensure that it is used by all applications
# that rely on the timezone configuration.
    echo $TZ > /etc/timezone

# For HealthChecks
RUN apt-get update && apt-get install -y curl

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["quotely-dotnet-api.csproj", "./"]
RUN dotnet restore "quotely-dotnet-api.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "quotely-dotnet-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "quotely-dotnet-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "quotely-dotnet-api.dll"]