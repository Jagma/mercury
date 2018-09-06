mergeInto(LibraryManager.library, {
  
	TableRealmsConnectToClient: function(ptrGameObjectName, ptrUrl) {
		var url=(" "+Pointer_stringify(ptrUrl)).slice(1);
		var gameObjectName=(" "+Pointer_stringify(ptrGameObjectName)).slice(1);

		console.log("TableRealms(HTML): Adding websocket ["+url+"]");
		
		if (typeof clientWebSockets === 'undefined') {
			// console.log("TableRealms(HTML): Creating clientWebSockets");
			clientWebSockets={};
		}
	
		var webSocket = new WebSocket(url);
		// console.log("TableRealms(HTML): Setting clientWebSockets["+url+"]");
		clientWebSockets[url]=webSocket;
		
		webSocket.onopen = function (event) {
			console.log("TableRealms(HTML): Connected to "+url);
			SendMessage(gameObjectName, 'ClientUrlOpen', url);
		}
		
		webSocket.onmessage = function (event) { 
			//console.log('TableRealms(HTML): Message from server '+ event.data);
			SendMessage(gameObjectName, 'RecieveMessage', url+"|"+event.data);
		}
		
		webSocket.onclose = function (event) {
			console.log("TableRealms(HTML): closed connection to "+url);
			SendMessage(gameObjectName, 'CloseClientConnection', url);
		}

		webSocket.onerror = function (event) {
			console.log("TableRealms(HTML): error on connection to "+url);
			SendMessage(gameObjectName, 'CloseClientConnection', url);
		}
	
		/*
		Start a web connection
		
		Any incoming data must be delivered to 
		SendMessage(gameObjectName, 'RecieveMessage', url+"|"+message);
		
		If we get a disconnect or we dc while in progress send the following
		SendMessage(gameObjectName, 'CloseClientConnection', url);
		
		?? How do we write a function to send data after this?
		*/
	},

	TableRealmsSendToClient: function(ptrUrl, ptrMessage) {
		var url=(" "+Pointer_stringify(ptrUrl)).slice(1);
		var message=(" "+Pointer_stringify(ptrMessage)).slice(1);
		
		// console.log("TableRealms(HTML): Sending message '"+message+"' to "+url);
		clientWebSockets[url].send(message);
	},

	TableRealmsStarted: function() {
		tableRealmsState="Started";
	},

	IsTableRealmsEmbedded: function() {
		if (typeof TableRealms !== "undefined") {
			return 1;
		} else {
			return 0;
		}
	},
	
	TableRealmsGetProtocol: function() {
	    var returnStr = "Unknown";
	
		if (typeof TableRealms !== "undefined") {
			returnStr=TableRealms.getProtocol();
		}
		
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},
	
	TableRealmsProxyListCallInternal: function (gameObjectName, sessionId, serviceUrl){
		var request;

		if (sessionId === null || sessionId===""){
			request={};
		}else{
			request={"session":sessionId};
		}

		var xhttp = new XMLHttpRequest();
		xhttp.open("POST", serviceUrl+"/api/proxy/list", true);
		xhttp.setRequestHeader("Content-type", "application/json");

		xhttp.onload = function() {
			if (xhttp.status === 200) {
				var response=JSON.parse(xhttp.responseText);
				// console.log(response)
				
				if (sessionId === null || sessionId===""){
					SendMessage(gameObjectName, 'SetSessionId', response.session);
				}
				
				if (response.urls !== null){
					var arrayLength = response.urls.length;
					for (var i = 0; i < arrayLength; i++) {
						SendMessage(gameObjectName, 'AddUrl', response.urls[i]);
					}
				}
			}
			else if (xhttp.status !== 200) {
				console.log("TableRealms(HTML): Error received from list call")
			}
		};

		xhttp.send(JSON.stringify(request));	
	},

	TableRealmsAmIHost: function(ptrGameObjectName) {
		var gameObjectName=(" "+Pointer_stringify(ptrGameObjectName)).slice(1);
		if (typeof TableRealms !== "undefined" && TableRealms.amITheHost()) {
			console.log("TableRealms(HTML): I am host");
			SendMessage(gameObjectName, "YouAreTheHost");
		}else{
			console.log("TableRealms(HTML): I am not host");
		}
	},
	
	TableRealmsProxyList: function(ptrGameObjectName, ptrSessionId, ptrServiceUrl) {
		var gameObjectName=(" "+Pointer_stringify(ptrGameObjectName)).slice(1);
		var sessionId=(" "+Pointer_stringify(ptrSessionId)).slice(1);
		var serviceUrl=(" "+Pointer_stringify(ptrServiceUrl)).slice(1);
		
		if (typeof TableRealms !== "undefined") {
			var url=TableRealms.getLocalUrl();
			if ((typeof url !== "undefined") && url !== null && url !== ""){
				console.log("TableRealms(HTML): Local websocket url ["+url+"]");
				SendMessage(gameObjectName, 'AddUrl', url);
			}
			if (TableRealms.amITheHost()){
				sessionId=TableRealms.getSession();
				if (sessionId !== null && sessionId !== "" && TableRealms.amITheHost()){
					console.log("TableRealms(HTML): Polling for new connection as a host")

					var request;
					request={"session":sessionId};

					var xhttp = new XMLHttpRequest();
					xhttp.open("POST", serviceUrl+"/api/proxy/list", true);
					xhttp.setRequestHeader("Content-type", "application/json");

					xhttp.onload = function() {
						if (xhttp.status === 200) {
							var response=JSON.parse(xhttp.responseText);
							// console.log(response)
							
							if (response.urls !== null){
								var arrayLength = response.urls.length;
								for (var i = 0; i < arrayLength; i++) {
									console.log("TableRealms(HTML): Got new  connection as a host " + response.urls[i])
									SendMessage(gameObjectName, 'AddUrl', response.urls[i]);
								}
							}
						}
						else if (xhttp.status !== 200) {
							console.log("TableRealsm(HTML): Error received from list call")
						}
					};

					xhttp.send(JSON.stringify(request));			
				}
			}
		}else{
			var request;

			if (sessionId === null || sessionId===""){
				request={};
			}else{
				request={"session":sessionId};
			}

			var xhttp = new XMLHttpRequest();
			xhttp.open("POST", serviceUrl+"/api/proxy/list", true);
			xhttp.setRequestHeader("Content-type", "application/json");

			xhttp.onload = function() {
				if (xhttp.status === 200) {
					var response=JSON.parse(xhttp.responseText);
					// console.log(response)
					
					if (sessionId === null || sessionId===""){
						SendMessage(gameObjectName, 'SetSessionId', response.session);
					}
					
					if (response.urls !== null){
						var arrayLength = response.urls.length;
						for (var i = 0; i < arrayLength; i++) {
							SendMessage(gameObjectName, 'AddUrl', response.urls[i]);
						}
					}
				}
				else if (xhttp.status !== 200) {
					console.log("TableRealms(HTML): Error received from list call")
				}
			};

			xhttp.send(JSON.stringify(request));			
		}
	}

});