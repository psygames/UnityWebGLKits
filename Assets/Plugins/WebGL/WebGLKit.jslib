var KitsLibrary =
{
	///////////////////////
	/// Interval Fields ///
	///////////////////////

	$callbacks:
	{
		onOpenFileCallback: null,

		_unknown: null
	},

	//////////////////////////
	/// Internal Functions ///
	//////////////////////////

	$Internal_StringToPtr: function(str)
	{
		var length = lengthBytesUTF8(str) + 1;
		var buffer = _malloc(length);
		stringToUTF8(str, buffer, length);
		return buffer;
	},

	$Internal_ArrayBufferToPtr: function(array_buffer)
	{
		var array = new Uint8Array(array_buffer);
		var buffer = _malloc(array.length);
		HEAPU8.set(array, buffer);
		return buffer;
	},

	$Internal_Log: function(str)
	{
		console.log("[JSLIB]: " + str);
	},

	$Internal_OnOpenFile: function()
	{
		var inputObj = document.getElementById('_open_file_dialog_input');
		var selectedFile = inputObj.files[0];
		inputObj.value = null;
		if (!selectedFile)
		{
			// Internal_Log("selected file is null");
			return;
		}

		var name = selectedFile.name;
		// Internal_Log("selected file is " + name);

		var reader = new FileReader();
		reader.onload = function()
		{
			// Internal_Log("loadend file: " + name);
			var namePtr = Internal_StringToPtr(name);
			var dataPtr = Internal_ArrayBufferToPtr(reader.result);
			var length = reader.result.byteLength;
			try
			{
				Runtime.dynCall('viii', callbacks.onOpenFileCallback, [ namePtr, dataPtr, length]);
			}
			finally
			{
				reader.onload = null;
				reader = null;
				_free(namePtr);
				_free(dataPtr);
			}
		};
		reader.readAsArrayBuffer(selectedFile);
	},

	///////////////////
	///    APIs     ///
	///////////////////

	UploadFile: function(callback)
	{
		// Internal_Log("Call UploadFile");
		callbacks.onOpenFileCallback = callback;
		var inputObj = document.getElementById('_open_file_dialog_input');
		if(inputObj === null)
		{
			// Internal_Log("inputObj is null");
		  inputObj = document.createElement('input');
			inputObj.setAttribute('id', '_open_file_dialog_input');
			inputObj.setAttribute('type', 'file');
			inputObj.setAttribute("style", 'visibility:hidden');

		 	// WebGL instance renamed from gameInstance to unityInstance in 2019.1
		 	if(typeof(unityInstance) != "undefined")
			{
				inputObj.setAttribute("onchange", 'unityInstance.Module.asmLibraryArg.Internal_OnOpenFile();');
			}
			else
			{
				inputObj.setAttribute("onchange", 'gameInstance.Module.asmLibraryArg.Internal_OnOpenFile();');
			}
			document.body.appendChild(inputObj);
		}
		inputObj.click();

		return 1;
	},

	Log: function(stringPtr)
	{
		var str = Pointer_stringify(stringPtr);
		Internal_Log(str);
	},

	Alert: function(stringPtr)
	{
		var str = Pointer_stringify(stringPtr);
		alert(str);
	},

	EvalJs: function(stringPtr)
	{
		var jsCode = Pointer_stringify(stringPtr);
		try
		{
			Internal_Log("Eval Js Code: \n" + jsCode);
			var result = eval(jsCode);
			if(typeof(result) == "undefined")
				return null;
			result = result.toString();
			var resultPtr = Internal_StringToPtr(result);
			//todo: _free ptr ???
			return resultPtr;
    }
		catch(exception)
		{
	  	alert(exception);
    }
		return null;
	},

	SetResolution: function(width, height)
	{
		// change canvas size
    document.getElementById("#canvas").style.width = width + "px";
    document.getElementById("#canvas").style.height = height + "px";
    document.getElementById("gameContainer").style.width = width + "px";
    document.getElementById("gameContainer").style.height = height + "px";
	},

	_UNKNOWN: function(){}
};

autoAddDeps(KitsLibrary, '$Internal_StringToPtr');
autoAddDeps(KitsLibrary, '$Internal_ArrayBufferToPtr');
autoAddDeps(KitsLibrary, '$Internal_Log');
autoAddDeps(KitsLibrary, '$Internal_OnOpenFile');
autoAddDeps(KitsLibrary, '$callbacks');
mergeInto(LibraryManager.library, KitsLibrary);
