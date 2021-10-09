using System.Collections;
using System.Collections.Generic;
using Atomata.VSolar.Apparatus.WebGL;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ApparatusUPackageDevelopment_WebGL_Connection_Tests
{
    [Test]
    public void ApparatusUPackageDevelopment_WebGL_Connection_Tests_FetchIdentifier_Earth()
    {
        GameObject proxyObject = new GameObject();
        ApparatusContainer_WebGL container = proxyObject.AddComponent<ApparatusContainer_WebGL>();
        string json = "\"Id\": {\"Type\": 1,\"Identifier\": \"earth\"},\"Transforms\": [{\"Position\": {\"x\": 0.0,\"y\": 0.0,\"z\": 0.0},\"Rotation\": {\"x\": 0.0,\"y\": 0.0,\"z\": 0.0,\"w\": 1.0},\"Scale\": {\"x\": 1.0,\"y\": 1.0,\"z\": 1.0}}],\"Children\": [{\"Type\": 0,\"Identifier\": \"earth\"}],\"Metadata\": {\"Paths\": [\"earth\",\"earth/earth\",\"earth/earth/event\",\"earth/earth/delta\",\"earth/earth/example\"],\"Data\": [\"0@identifier:earth\",\"1@identifier:earth\",\"2@identifier:event\",\"2@input:bool:split_active\",\"2@input:bool:mantle_active\",\"3@identifier:delta\",\"3@input:vec3:position\",\"3@input:vec3:rotation\",\"3@input:vec3:scale\",\"3@input:vec3:position_delta\",\"3@input:vec3:rotation_delta\",\"3@input:vec3:scale_delta\",\"3@input:bool:isLocal\",\"4@identifier:example\",\"4@input:vec3:position\",\"4@input:vec3:rotation\",\"4@input:vec3:scale\",\"4@input:vec3:position_delta\",\"4@input:vec3:rotation_delta\",\"4@input:vec3:scale_delta\",\"4@input:bool:isLocal\"]}}";
        container.LoadFromJSON(json);
        Assert.AreEqual(container._identifier, "earth");
    }

    [Test]
    public void ApparatusUPackageDevelopment_WebGL_Connection_Tests_FetchIdentifier_BlueSphere()
    {
        GameObject proxyObject = new GameObject();
        ApparatusContainer_WebGL container = proxyObject.AddComponent<ApparatusContainer_WebGL>();
        string json = "{\"Id\":{\"Type\":1,\"Identifier\": \"blue-sphere\"},\"Transforms\":[{\"Position\":{\"x\":0.0,\"y\":0.0,\"z\":0.0},\"Rotation\":{\"x\":0.0,\"y\":0.0,\"z\":0.0,\"w\":1.0},\"Scale\":{\"x\":1.0,\"y\":1.0,\"z\":1.0}}],\"Children\":[{\"Type\":0,\"Identifier\":\"blue-sphere\"}],\"Metadata\":{\"Paths\":[\"blue-example\",\"blue-example/blue-sphere\",\"blue-example/blue-sphere/id\",\"blue-example/blue-sphere/id\"],\"Data\":[\"0@identifier:blue-example\",\"1@identifier:blue-sphere\",\"2@identifier:id\",\"2@input:void:VisibleOn\",\"2@input:void:VisibleOff\",\"3@identifier:id\",\"3@input:vec3:position\",\"3@input:vec3:rotation\",\"3@input:vec3:scale\",\"3@input:vec3:position_delta\",\"3@input:vec3:rotation_delta\",\"3@input:vec3:scale_delta\",\"3@input:bool:isLocal\"]}}";
        container.LoadFromJSON(json);
        Assert.AreNotEqual(container._identifier, "earth");
        Assert.AreEqual(container._identifier, "blue-sphere");
    }
}
