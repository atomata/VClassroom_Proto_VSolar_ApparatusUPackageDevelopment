# HexUN
Framework classes for the HexUN Unity process

### Installation
To add this to your project make sure the following is in your package.json

```json
{
    "dependencies" : 
    {
        "com.hex.hexcs" : "https://github.com/hexthedev/HexCS.git",
        "com.hex.hexcs.data" : "https://github.com/hexthedev/HexCS-Data.git",
        "com.hex.hexun" : "https://github.com/hexthedev/HexUN.git"
    }
}
```

# Overview
Ok time to actual document some of this stuff



## Monobehaviours
There are a few things I find annoying about Monobehaviours that I've tried to fix in a generic way by providing some new classes to replace them. 

### `MonoEnhanced : MonoBehaviour`
Exists to extend MonoBehaviour functionality. 
* Offer EventBindings object to handle event subscriptions using HexCS `EventBindings`
* Allows functions to be added to `CallAfterStart` and `CallAfterAwake` to guarentee that a function is called after the class is Awake/Started
  * Because of this, you need to override `MonoStart` and `MonoAwake` instead of Start, Awake

### `MonoData : MonoEnhanced`
This is a simple way to add and remove arbitray type data to a `MonoEnhanced`
* Can add, remove, get arbitrary data
* Adds Editor functionality to add arbitrary So data

## Visual Objects
Exists to provide a framework from creating objects that only need rendering once per visual frame. 

### `AVisualObject : MonoData`
* `Initialize()` to force render
* All `Render()` to render any changes this frame
* Actual render work is only applied in `LateUpdate()`. **Note: This detail might change, but I now have a place to change when things render for all visual objects**


## Facade
Facades exist as a way to make it seem like everything is done from one class, even though behind the seems theres a lot more going on. Easier for humans. 

## `AVisualFacade : AVisualObject`
A visual object facade.
* Hides children in normal scene view and some chosen siblings
* In Prefab view, shows everything so that prefab view is the place to do the actual visual creation


