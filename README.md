# polkadot_api_dotnet

## Requirements

### Windows

- Visual Studio Community 2019 (Pro or Enterprise will also work)
- SDK: .NET Core 2.2

### Linux

- [Dotnet Core 2.2](https://dotnet.microsoft.com/download/linux-package-manager/ubuntu16-04/sdk-current)

## Building

### Windows

Please use Ctrl+Shift+B in Visual Studio :)

### Linux

```
git clone https://github.com/usetech-llc/polkadot_api_dotnet && cd polkadot_api_dotnet
$ dotnet build
$ dotnet test
```

### Docker

```
git clone https://github.com/usetech-llc/polkadot_api_dotnet
$ docker build -t polkanet .
$ docker run -it --rm polkanet /bin/bash
# dotnet build
# dotnet test
```

### Building of documentation

Edit Polkadot.csproj file and add "build;" in the docfx section like here:
```
<PackageReference Include="docfx.console" Version="2.45.1">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

Now when project is rebuilt, the docuemntation will be updated.
