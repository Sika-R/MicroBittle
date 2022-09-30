mergeInto(LibraryManager.library, {
	OpenPort: function () {
		htmlOpenSerialPort();
	},

	/*ReadLine: function () {
		return htmlReadFromPort();
	}*/

	SendLine: function (str){
		var text = Pointer_stringify(str);
		htmlSendSerialLine(text);
	}
});