mergeInto(LibraryManager.library, {

    LogToBrowser: function(str){
        console.log(Pointer_stringify(str));
    },
	
	AssetBundleRequestToBrowser: function(){
        console.log("Uhh");
    }

});