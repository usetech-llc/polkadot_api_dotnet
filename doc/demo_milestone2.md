# Manual Demos - Milestone 2

Milestone 2 deliverables are tagged as [milestone2](https://github.com/usetech-llc/polkadot_api_dotnet/tree/milestone2)

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


## Deliverable 4

### Support following RPC methods

#### state_getKeys
```
# dotnet test --filter GetKeys
Test run for /src/PolkaTest/bin/Debug/netcoreapp2.2/PolkaTest.dll(.NETCoreApp,Version=v2.2)
Microsoft (R) Test Execution Command Line Tool Version 16.2.0-preview-20190606-02
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
2019-10-18 08:16:08.9086|INFO|Polkadot.Logger|Connected to wss://alex.unfrastructure.io/public/ws
2019-10-18 08:16:09.2710|INFO|Polkadot.Logger|Message body {
  "id": 1,
  "jsonrpc": "2.0",
  "method": "state_getMetadata",
  "params": []
}
2019-10-18 08:16:09.2710|INFO|Polkadot.Logger|Message 1 was sent
2019-10-18 08:16:10.2364|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":"0x6d65746103441873797374656d1853797374656d012c304163636f756e744e6f6e6365010130543a3a4163636f756e744964.....06f6c64206b657920697320737570706c6965642e","id":1}
2019-10-18 08:16:10.2434|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","result":"0x6d65746103441873797374656d1853797374656d012c304163636f756e744e6f6e6365010130543a3a4163636f756e744964205.....06f6c64206b657920697320737570706c6965642e","id":1}
2019-10-18 08:16:11.1379|INFO|Polkadot.Logger|FreeBalance hash function is xxHash
2019-10-18 08:16:11.1382|INFO|Polkadot.Logger|Balances module index: 5
2019-10-18 08:16:11.1382|INFO|Polkadot.Logger|Transfer call index: 0
2019-10-18 08:16:11.4731|INFO|Polkadot.Logger|Connection close

Test Run Successful.
Total tests: 1
     Passed: 1
 Total time: 6.4180 Seconds

```

#### state_getStorage (and alias state_getStorageAt)
```
TBD
```

#### state_getStorageHash (and alias state_getStorageHashAt)
```
TBD
```

#### state_getStorageSize (and alias state_getStorageSizeAt)
```
TBD
```

#### state_getChildKeys
```
TBD
```

#### state_getChildStorage
```
TBD
```

#### state_getChildStorageHash
```
TBD
```

#### state_getChildStorageSize
```
TBD
```

#### state_queryStorage
```
TBD
```


## Deliverable 5

### Address balance (RPC method state_subscribeStorage with appropriate storage address filter)
```
TBD
```

### Current era and epoch info (RPC method state_subscribeStorage with appropriate storage filters)
```
TBD
```


## Deliverable 6

### Support following RPC methods
#### author_submitExtrinsic
```
TBD
```

#### author_pendingExtrinsics
```
TBD
```

#### author_removeExtrinsic
```
TBD
```

### One transaction type is supported with dedicated API call - sending DOTs to another address
```
TBD
```

### Transaction is serialized and prepared (formatted) appropriately for signing
```
TBD
```

### Transaction can be signed with provided private key
```
TBD
```

### Cryptogram can be sent to the substrate node to be processed and included in the blockchain
```
TBD
```

### Support following WebSocket subscriptions with author_extrinsicUpdate
```
TBD
```


## Deliverable 7

### API and example full documentation

### Cleanup project files, ensure all tests pass

### API library is packaged as a zip archive with DLL files that can be used without compilation on Windows.
