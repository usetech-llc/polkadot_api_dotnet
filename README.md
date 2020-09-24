# polkadot_api_dotnet

## Milestones

- [Milestone 1](https://github.com/usetech-llc/polkadot_api_dotnet/blob/master/doc/demo_milestone1.md)
- [Milestone 2](https://github.com/usetech-llc/polkadot_api_dotnet/blob/master/doc/demo_milestone2.md)

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

### Generating Keys

There is a modified version of subkey that will output the private key in 64-byte format required for this library. Here is how to get the keys:

```
$ git clone github.com:usetech-llc/substrate.git
$ cd bin/utils/subkey
$ cargo build
$ cargo run -- generate
    Finished dev [unoptimized + debuginfo] target(s) in 0.78s
     Running `/home/greg/Development/src/polkadot/substrate_ut/target/debug/subkey generate`
Secret phrase `maple rather inject cage food unable enter economy adapt mandate novel start` is account:
  Network ID/version: substrate
  Secret seed:        0xce456c3f305bf2c31868b1f51eb297e8b304551f1e456758cfa555d0040b31f7
  64-byte secret:     0x1B04EA5667F6D63B7D405503651F56129B64ED148336F35EECCA53716B872B0B542E9D846415B636BB1741C22C8BCA827CEF9D170B9E671E36B60345D18B3894
  Public key (hex):   0xa805c199eaa3d4d3f0865691e94b3ce4a508ee68aef6d7b42642d0d5d389667d
  Account ID:         0xa805c199eaa3d4d3f0865691e94b3ce4a508ee68aef6d7b42642d0d5d389667d
  SS58 Address:       5Fs1cSCX2ZDvtfC6wvrWWFoQDRdcESLuoTyYofdE1AM7LVhD
```

### Special Thanks

We thank Gautam Dhameja for sharing the source code for C-Rust bindings for SR25519 Rust library, which enabled our end-to-end testing and validation of our sr25519 implementations.