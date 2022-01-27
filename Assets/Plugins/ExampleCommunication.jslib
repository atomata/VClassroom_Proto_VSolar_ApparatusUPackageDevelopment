mergeInto(LibraryManager.library, {
    OnAtomataSceneInitalized: function()
    {
        if(window.onAtomataSceneInitalize)
        {
            window.onAtomataSceneInitalize();
        }
    }
});