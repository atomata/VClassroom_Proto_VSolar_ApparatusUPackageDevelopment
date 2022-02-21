using System.Collections;
using System.Collections.Generic;
using Unity.Serialization.Json;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [ContextMenu("Do")]
    void Do()
    {
        YehNode n = new YehNode
        {
            Name = "root",
            x = 7,
            Children = new []
            {
                new YehNode()
                {
                    Name = "ch1",
                    x = 3,
                    Children = new YehNode[0]
                },
                new YehNode()
                {
                    Name = "ch2",
                    x = 3,
                    Children = new YehNode[0]
                }
            }
        };
        
        Debug.Log(JsonSerialization.ToJson(n));
    }
    
    
}


public abstract class MyNode
{
    public string Name;
    public MyNode[] Children;
}

public class YehNode : MyNode
{
    public int x;
    
}
