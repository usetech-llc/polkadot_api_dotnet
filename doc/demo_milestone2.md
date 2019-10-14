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
TBD
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
