# Manual Demos

Milestone 1 deliverables are marked by tag [milestone1](https://github.com/usetech-llc/polkadot_api_dotnet/tree/milestone1)

## Deliverable 1

### The project can be built with provided instructions on bare installation of Ubuntu Linux 16.04.

For convenience the Dockerfile is provided that installs all dependencies as needed, checks out the code for the API and builds it. In order to execute single API tests manually, please have docker CE installed, then clone this repository and run following commands:
```
$ docker build -t polkanet .
$ docker run -it --rm polkanet /bin/bash
```

Now you are connected to a running docker container with API built. You can execute following commands to examine deliverables.

### Build project and run tests (Command line tool is provided to execute all milestone deliverables)

The milestone 1 deliverables can be demonstrated all with one command through running tests that have verbal output. Though separate commands may be run with parameter `dotnet --filter <test name>`. We will point out the particular fragments of this output that demonstrate deliverables as well as suggest running separate tests when it makes sense.

```
# dotnet build
# dotnet test
```

### Connection

#### Connection to a public Substrate node (at URL wss://alex.unfrastructure.io/public/ws) is established, which makes sending commands and receiving responses possible. Look for the following output:
```
2019-09-27 13:44:50.0041|INFO|Polkadot.Logger|Connected to wss://alex.unfrastructure.io/public/ws
```

#### Connection is closed with API disconnect command when the test is done. Look for the following output at the end of each test:
```
2019-09-27 13:44:53.0167|INFO|Polkadot.Logger|Connection close
```


#### Node’s TLS certificate is verified during connection process

Certificate validation happens in standard libraries for .NET, so we only test what happens if certificate file cannot be found.
The unit test BadCertificate hides file 'ca-chain.cert.pem' that contains trusted root CA certificate making it impossible to verify node's TLS certificate:

```
$ dotnet test --filter BadCertificate
```

Here is the code fragment from this unit test that demonstrates that it expects the library to throw a FileNotFoundException:
```csharp
Assert.Throws<FileNotFoundException>(() =>
{
    using (IApplication app = PolkaApi.GetAppication())
    {
        app.Connect();

        app.Disconnect();
    }
});
```

### Basic data can be read from the node, deserialized to appropriate C# struct and returned from API, which includes following Test-RPC commands:

#### chain_getBlockHash
```
$ dotnet test --filter GetBlock

...

2019-09-27 14:01:24.7272|INFO|Polkadot.Logger|Message body {
  "id": 2,
  "jsonrpc": "2.0",
  "method": "chain_getBlockHash",
  "params": [
    2
  ]
}
2019-09-27 14:01:24.7272|INFO|Polkadot.Logger|Message 2 was sent
2019-09-27 14:01:25.0153|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":"0x37096ff58d1831c2ee64b026f8b70afab1942119c022d1dcfdbdc1558ebf63fa","id":2}
2019-09-27 14:01:25.0153|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","result":"0x37096ff58d1831c2ee64b026f8b70afab1942119c022d1dcfdbdc1558ebf63fa","id":2}
```

#### chain_getRuntimeVersion
```
$ dotnet test --filter GetRuntimeVersion
...

TBD
```

#### state_getMetadata
```
$ dotnet test --filter GetBlock

...

2019-09-27 14:01:27.0670|INFO|Polkadot.Logger|Message body {
  "id": 3,
  "jsonrpc": "2.0",
  "method": "state_getMetadata",
  "params": []
}
2019-09-27 14:01:27.0670|INFO|Polkadot.Logger|Message 3 was sent
2019-09-27 14:01:28.2864|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":"0x6d65746103441873797374656d1853797374656d012c304163636f756e744e6f6e6365010130543a3a4163636f756e744964.....06f6c64206b657920697320737570706c6965642e","id":3}
2019-09-27 14:01:28.2864|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","result":"0x6d65746103441873797374656d1853797374656d012c304163636f756e744e6f6e6365010130543a3a4163636f756e744964205.....06f6c64206b657920697320737570706c6965642e","id":3}
```

#### system_properties, system_chain, system_name, system_version
```
$ dotnet test --filter GetSystemInfo

...

2019-09-27 15:01:08.0398|INFO|Polkadot.Logger|Message body {
  "id": 2,
  "jsonrpc": "2.0",
  "method": "system_name",
  "params": []
}
2019-09-27 15:01:08.0398|INFO|Polkadot.Logger|Message 2 was sent
2019-09-27 15:01:08.3443|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":"parity-polkadot","id":2}
2019-09-27 15:01:08.3443|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","result":"parity-polkadot","id":2}
2019-09-27 15:01:08.6508|INFO|Polkadot.Logger|Message body {
  "id": 3,
  "jsonrpc": "2.0",
  "method": "system_chain",
  "params": []
}
2019-09-27 15:01:08.6508|INFO|Polkadot.Logger|Message 3 was sent
2019-09-27 15:01:08.9629|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":"Alexander","id":3}
2019-09-27 15:01:08.9629|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","result":"Alexander","id":3}
2019-09-27 15:01:09.2683|INFO|Polkadot.Logger|Message body {
  "id": 4,
  "jsonrpc": "2.0",
  "method": "system_version",
  "params": []
}
2019-09-27 15:01:09.2683|INFO|Polkadot.Logger|Message 4 was sent
2019-09-27 15:01:09.5724|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":"0.4.4","id":4}
2019-09-27 15:01:09.5724|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","result":"0.4.4","id":4}
2019-09-27 15:01:09.8912|INFO|Polkadot.Logger|Message body {
  "id": 5,
  "jsonrpc": "2.0",
  "method": "system_properties",
  "params": []
}
2019-09-27 15:01:09.8912|INFO|Polkadot.Logger|Message 5 was sent
2019-09-27 15:01:10.1954|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":{"tokenDecimals":15,"tokenSymbol":"DOT"},"id":5}
2019-09-27 15:01:10.1954|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","result":{"tokenDecimals":15,"tokenSymbol":"DOT"},"id":5}
2019-09-27 15:01:10.6057|INFO|Polkadot.Logger|Connection close
```

### Building instructions, initialization, and library usage documented

The documentation is built with docfx tool and exists in the repository as a static website. (See doc/html folder). Open this file in the browser locally as a starting point: /doc/html/docfx/api/Polkadot.Api.IApplication.html

- Building: See [root level README.md](https://github.com/usetech-llc/polkadot_api_cpp/blob/master/README.md)

## Deliverable 2 - Non-Parameterized Calls

### Support following RPC methods:

#### chain_getHeader
```
$ dotnet test --filter GetBlock

2019-09-27 14:01:34.3478|INFO|Polkadot.Logger|Message body {
  "id": 7,
  "jsonrpc": "2.0",
  "method": "chain_getHeader",
  "params": [
    "0x37096ff58d1831c2ee64b026f8b70afab1942119c022d1dcfdbdc1558ebf63fa"
  ]
}
2019-09-27 14:01:34.3478|INFO|Polkadot.Logger|Message 7 was sent
2019-09-27 14:01:34.6471|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":{"digest":{"logs":["0x037c295a0f00000000ccb5103b9934c8b5a2fa9bd520092e1062dc43415aa8b712fd38d89e6450c5b.....a9c17fd7703a3001cfda9c17723469fa356594bc"},"id":7}
```

#### chain_getBlock
```
$ dotnet test --filter GetBlock

2019-09-27 14:01:33.6794|INFO|Polkadot.Logger|Message body {
  "id": 6,
  "jsonrpc": "2.0",
  "method": "chain_getBlock",
  "params": [
    "0x37096ff58d1831c2ee64b026f8b70afab1942119c022d1dcfdbdc1558ebf63fa"
  ]
}
2019-09-27 14:01:33.6794|INFO|Polkadot.Logger|Message 6 was sent
2019-09-27 14:01:33.9672|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":{"block":{"extrinsics":["0x01000003e8f81c5c","0x010b0000"],"header":{"digest":{"logs":["0x037c295a0f000.....7723469fa356594bc"}},"justification":null},"id":6}

```

#### chain_getFinalizedHead
```
$ dotnet test --filter GetBlock

2019-09-27 14:01:29.1789|INFO|Polkadot.Logger|Message body {
  "id": 4,
  "jsonrpc": "2.0",
  "method": "chain_getFinalizedHead",
  "params": []
}
2019-09-27 14:01:29.1789|INFO|Polkadot.Logger|Message 4 was sent
2019-09-27 14:01:29.4806|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":"0xd245466d766605bfe82b0c531eefabe7b00fcc6ed16bb5f7d52b91f604c86463","id":4}
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
Test WssubscribeAll demonstrates this:
```
$ dotnet test --filter WssubscribeAll

...

2019-09-27 14:15:47.9828|INFO|Polkadot.Logger|Message body {
  "id": 2,
  "jsonrpc": "2.0",
  "method": "chain_subscribeNewHead",
  "params": []
}

...

2019-09-27 14:15:48.3288|INFO|Polkadot.Logger|Subscribed with subscription ID: 9152
```

### Following data can be read from the chain. Updates for this data arrive via WS subscription, are deserialized to an appropriate C# object, and returned from the API via a delegate call (decided to do it as a lambda and callback, this is a better design).

See this code fragment from WssubscribeAll unit test:
```csharp
int subId = app.SubscribeBlockNumber((blockNumber) =>
{
    blockNum = blockNumber;
    messagesCount++;
    output.WriteLine($"Last block number        : {blockNumber}");
});
```

#### Current block (chain_subscribeNewHead)
```
$ dotnet test --filter WssubscribeAll

...

2019-09-27 14:15:47.9828|INFO|Polkadot.Logger|Message body {
  "id": 2,
  "jsonrpc": "2.0",
  "method": "chain_subscribeNewHead",
  "params": []
}

...

2019-09-27 14:15:48.3056|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","method":"chain_newHead","params":{"result":{"digest":{"logs":["0x04617572612101edae970f00000000ac22b39dd5470c37ab7.....1c3dff924c59814230f1fb8849"},"subscription":9152}}
```

#### state_runtimeVersion
```
$ dotnet test --filter WssubscribeAll

...

2019-09-27 14:15:48.6348|INFO|Polkadot.Logger|Message body {
  "id": 3,
  "jsonrpc": "2.0",
  "method": "state_subscribeRuntimeVersion",
  "params": []
}

...

2019-09-27 14:15:48.9333|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","method":"state_runtimeVersion","params":{"result":{"apis":[["0xdf6acb689907609b",2],["0x37e397fc7c91f5e4",1],["0x4.....polkadot","specVersion":112},"subscription":9153}}

```

#### API can be unsubscribed from WebSocket endpoint
```
$ dotnet test --filter WssubscribeAll

...

2019-09-27 14:15:56.1410|INFO|Polkadot.Logger|Message body {
  "id": 4,
  "jsonrpc": "2.0",
  "method": "chain_unsubscribeNewHead",
  "params": [
    9152
  ]
}
2019-09-27 14:15:56.1410|INFO|Polkadot.Logger|Message 4 was sent
2019-09-27 14:15:56.4396|INFO|Polkadot.Logger|WS Received Message: {"jsonrpc":"2.0","result":true,"id":4}
2019-09-27 14:15:56.4396|INFO|Polkadot.Logger|Message received: {"jsonrpc":"2.0","result":true,"id":4}
2019-09-27 14:15:56.4396|INFO|Polkadot.Logger|Unsubscribed from subscription ID: 9152
2019-09-27 14:15:56.4471|INFO|Polkadot.Logger|operation canceled: 9152

```

#### Subscribing/unsubscribing and data structures for current block and runtime version are documented

Documentation is placed in doc/html folder as a static website. Open these references in the browser locally to see sections about subscribing/unsibscribing:

```
/doc/html/docfx/api/Polkadot.Api.IApplication.html#Polkadot_Api_IApplication_SubscribeBlockNumber_System_Action_System_Int64__
/doc/html/docfx/api/Polkadot.Api.IApplication.html#Polkadot_Api_IApplication_UnsubscribeBlockNumber_System_Int32_

/doc/html/docfx/api/Polkadot.Api.IApplication.html#Polkadot_Api_IApplication_SubscribeRuntimeVersion_System_Action_Polkadot_Data_RuntimeVersion__
/doc/html/docfx/api/Polkadot.Api.IApplication.html#Polkadot_Api_IApplication_UnsubscribeRuntimeVersion_System_Int32_
```

... or open this file in the browser locally: /doc/html/docfx/api/Polkadot.Api.IApplication.html

And then scroll down to SubscribeBlockNumber, SubscribeRuntimeVersion, UnsubscribeBlockNumber, UnsubscribeRuntimeVersion.
