using System;
using System.Collections;
using System.Collections.Generic;
using Atomata.VSolar.Apparatus;
using UnityEngine;

public class AtomataSceneDebugger : MonoBehaviour
{
    public AtomataSceneManager manager;
    
    public string Input;
    
    [ContextMenu("LoadApparatus")] void LoadApparatus() => manager.LoadApparatus(Input);
    [ContextMenu("UnloadApparatus")] void UnloadApparatus() => manager.UnloadApparatus();
    [ContextMenu("LoadSkybox")] void LoadSkybox() => manager.LoadSkybox(Input);
    [ContextMenu("Trigger")] void Trigger() => manager.Trigger(Input);
    [ContextMenu("ReturnCamera")] void ReturnCamera() => manager.ReturnCamera();
    [ContextMenu("Debug")] void Debug() => manager.Dbg("printtree");
}
