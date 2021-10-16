# ATF-Apparatus
Apparatus are the 3D building blocks of Atomata experiences. They represent 3D virtual objects designed to teach a certain educational subject.

## Usage by Containers
Containers are code that can load apparatus objects from JSON. They are used in applications to run an apparatus.

### Serialized Form
Apparatus come in a versioned, serialized form which can be used to load an apparatus. An appartus should be loaded by creating a hierarchy of `GameObjects` and adding the appropriate `ApparatusNodes` as components.   

### Requests
Apparatus use requests to ask for resources from the environment. Containers need to be able to resolve these requests. For example: Apparatus with `AssetNodes` request assets (as Prefabs) from their environment by Id.

### Connecting and Loading
Once `GameObject` Nodes are created, containers must call `Connect()` on the root node to connect the node tree, and eventuall `Load()` to load all external resources the nodes refer to. 

## Usage by External to Container
### Triggers
Apparatus objects contain triggers which can be read and understood from the Metadata section of the apparatus json. These triggers are REST api style uri calls.  