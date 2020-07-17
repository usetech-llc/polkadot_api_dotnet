FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS base
WORKDIR /src

COPY ["./sr25519-dotnet.lib/", "sr25519-dotnet.lib/"]
RUN dotnet restore sr25519-dotnet.lib/sr25519-dotnet.lib.csproj
COPY . .

COPY ["./Polkadot/", "Polkadot/"]
RUN dotnet restore Polkadot/Polkadot.csproj
COPY . .

COPY ["./PolkaTest/", "PolkaTest/"]
RUN dotnet restore PolkaTest/PolkaTest.csproj
COPY . .
RUN ls
COPY ["./Polkadot/ca-chain.cert.pem", "PolkaTest/bin/Debug/netcoreapp2.2/ca-chain.cert.pem"]

WORKDIR "/src/Polkadot"
RUN dotnet build "Polkadot.csproj" -c Release -o /app

WORKDIR "/src/PolkaTest"