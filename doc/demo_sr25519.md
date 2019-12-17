# Manual Demos - Milestone 2

SR25519 deliverables are located in master branch of this repository.

### Build and run Docker image

```
$ docker build -t polkanet .
$ docker run -it --rm polkanet /bin/bash
```

Now you are connected to a running docker container with API built. You can execute following commands to examine deliverables.

### Build project and run tests

```
# dotnet build
# dotnet test
# dotnet test --filter <test name>
```

### Sign message and verify using Polkadot Web UI 

First, run sr25519 test to get a signature for a message. Both Kusama and Alexander use the same version of signature, so there is no need to test on both.

```
# dotnet test --filter sr25519
Merlin/Merlin.cs(255,21): warning CS0414: The field 'TranscriptRng._pointer' is assigned but its value is never used [/src/Schnorrkel/Schnorrkel.csproj]
Test run for /src/PolkaTest/bin/Debug/netcoreapp2.2/PolkaTest.dll(.NETCoreApp,Version=v2.2)
Microsoft (R) Test Execution Command Line Tool Version 16.2.0-preview-20190606-02
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
Message: 0x2F0ABEF86B361DC87DA38517203AED9265DAD0CB
Signature: 0x4A09B50FCF63535753B739258A7A979AA4AE353036A1A27905B3BCB5C7F6247D36B311F0C8CC15E275BD3921023B5D07F148C4A7734408CDCFF211E29FD7C901

```

Next, copy the information below as well as the newly generated message and signature and paste in the UI to verify:

```
URL: https://polkadot.js.org/apps/#/toolbox/verify
Address: HRXczFqEHbehYTvdBxX1K62QaPhJywEy5BKxHdJnE8wfHH1
Message: 0x2F0ABEF86B361DC87DA38517203AED9265DAD0CB
Signature example: 0x4A09B50FCF63535753B739258A7A979AA4AE353036A1A27905B3BCB5C7F6247D36B311F0C8CC15E275BD3921023B5D07F148C4A7734408CDCFF211E29FD7C901
```

### Make a transfer that uses C# version of SR25519

The code in master branch already uses C# version of sr25519, so it is sufficient to run transfer test. The following code around line 583 in Application.cs file shows that C# version of sr25519 (Sr25519v011.SignSimple) is used instead of Rust version:

```
var message = signaturePayloadBytes.AsMemory().Slice(0, (int)payloadLength).ToArray();
var sig = Sr25519v011.SignSimple(te.Signature.SignerPublicKey, secretKeyVec, message);
```

In order to run the test, execute the following:

```
# cd /src/SignAndSendTransferTest
# dotnet build
# dotnet run <sender address> <recipient address> <amount in fDOTs> <sender private key (hex)>
```

Expect output such as:
```
...
2019-10-18 12:47:26.1334|INFO|Polkadot.Logger|Message body {
  "id": 5,
  "jsonrpc": "2.0",
  "method": "author_submitAndWatchExtrinsic",
  "params": [
    "0x250281FF5E8135DC17F025CA044780631EF89.....D79DF6ACA044DF12BA9B727110FEBF95BFF2D0C0104"
  ]
}
...

2019-10-18 12:47:26.4346|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","method":"author_extrinsicUpdate","params":{"result":"ready","subscription":45421}}
{
  "result": "ready",
  "subscription": 45421
}
...
```

Also, feel free to check the address on [Polkascan](https://polkascan.io).


