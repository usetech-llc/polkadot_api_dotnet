# Manual Demos

Milestone 1 deliverables are marked by tag [milestone1](https://github.com/usetech-llc/polkadot_api_dotnet/tree/milestone1)

## Deliverable 1

### The project can be built with provided instructions on bare installation of Ubuntu Linux 16.04.

For convenience the Dockerfile is provided that installs all dependencies as needed, checks out the code for the API and builds it. In order to execute single API tests manually, please have docker CE installed, then clone this repository and run following commands (first one takes ~20 minutes to run for the first time):
```
$ docker build -t polkanet .
$ docker run -it --rm polkanet /bin/bash
```

Now you are connected to a running docker container with API built, tested, and ready. You can execute following commands to examine deliverables.

### Build project and run tests (Command line tool is provided to execute all milestone deliverables)

The milestone 1 deliverables are demonstrated all with one command through running tests that have verbal output. We will point out the particular fragments of this output that demonstrate deliverables.

```
# dotnet build
# dotnet test
```

### Connection:
- Connection to a public Substrate node (at URL wss://poc3-rpc.polkadot.io:443/) is established, which makes sending commands and receiving responses possible. Look for the following ouput:
```
TBD
```

- Connection is closed with API disconnect command when the test is done. Look for the following ouput:
```
TBD
```


#### Node’s TLS certificate is verified during connection process

The unit test TBD hides file 'ca-chain.cert.pem' that contains trusted root CA certificate making it impossible to verify node's TLS certificate. Look for the following output:
```
TBD
```

### Basic data can be read from the node, deserialized to appropriate C# struct and returned from API, which includes following Test-RPC commands:

#### chain_getBlockHash
```
2019-09-26 13:49:58.3327|INFO|Polkadot.Logger|Message body {
  "id": 2,
  "jsonrpc": "2.0",
  "method": "chain_getBlockHash",
  "params": []
}
```

#### chain_getRuntimeVersion
```
TBD
```

#### state_getMetadata
```
2019-09-26 13:49:56.5603|INFO|Polkadot.Logger|Message body {
  "id": 1,
  "jsonrpc": "2.0",
  "method": "state_getMetadata",
  "params": []
}
```

#### system_properties, system_chain, system_name, system_version
```
TBD
```

### Building instructions, initialization, and library usage documented

- Building: See [root level README.md](https://github.com/usetech-llc/polkadot_api_cpp/blob/master/README.md)
- TBD: Lib usage: See doc/html folder


## Deliverable 2 - Non-Parameterized Calls

### Support following RPC methods:

#### chain_getHeader
```
TBD
```

#### chain_getBlock
```
TBD
```

#### chain_getFinalizedHead
```
TBD
```

#### system_health
```
TBD
```

#### system_peers
```
TBD
```

#### system_networkState
```
TBD
```

## Deliverable 3 - Non-Parameterized Subscriptions

### API can be subscribed to WebSocket endpoint for updates on chain state
Method state_subscribeStorage demonstrates this:
```
TBD
```

### Following data can be read from the chain. Updates for this data arrive via WS subscription, are deserialized to an appropriate C# object, and returned from the API via a delegate call:

```
TBD - code snippet
```

#### Current block (chain_subscribeNewHead)
```
TBD
```

#### state_runtimeVersion
```
TBD
```

#### API can be unsubscribed from WebSocket endpoint
```
TBD
```

#### Subscribing/unsubscribing and data structures for current block and runtime version are documented
```
TBD - reference to docs
```
