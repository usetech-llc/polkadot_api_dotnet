FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS base
WORKDIR /src

COPY ["./Polkadot/", "Polkadot/"]
RUN dotnet restore Polkadot/Polkadot.csproj
COPY . .

COPY ["./PolkaTest/", "PolkaTest/"]
RUN dotnet restore PolkaTest/PolkaTest.csproj
COPY . .
 
WORKDIR "/src/Polkadot"
RUN dotnet build "Polkadot.csproj" -c Release -o /app

ENTRYPOINT ["dotnet", "Polkadot.dll"]

WORKDIR "/src/PolkaTest"
RUN dotnet test