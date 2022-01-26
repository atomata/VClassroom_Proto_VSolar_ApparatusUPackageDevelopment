mergeInto(LibraryManager.library, {
    AtomataSceneInitalized: function()
    {
        if(window.onAtomataSceneInitalize)
        {
            window.onAtomataSceneInitalize();
        }
    }
});