mergeInto(LibraryManager.library, {
  
	TableRealmsConnectToClient: function(ptrGameObjectName, ptrUrl) {
		var url=(" "+Pointer_stringify(ptrUrl)).slice(1);
		var gameObjectName=(" "+Pointer_stringify(ptrGameObjectName)).slice(1);

		// console.log("TableRealms(HTML): Adding websocket ["+url+"]");
		
		if (typeof clientWebSockets === 'undefined') {
			// console.log("TableRealms(HTML): Creating clientWebSockets");
			clientWebSockets={};
		}
	
		var webSocket = new WebSocket(url);
		// console.log("TableRealms(HTML): Setting clientWebSockets["+url+"]");
		clientWebSockets[url]=webSocket;6
		
		webSocket.onopen = function (event) {
			// console.log("TableRealms(HTML): Connected to "+url);
			SendMessage(gameObjectName, 'ClientUrlOpen', url);
		}
		
		webSocket.onmessage = function (event) { 
			// console.log('TableRealms(HTML): Message from server ', event.data);
			SendMessage(gameObjectName, 'RecieveMessage', url+"|"+event.data);
		}
		
		webSocket.onclose = function (event) {
			SendMessage(gameObjectName, 'CloseClientConnection', url);
		}

		webSocket.onerror = function (event) {
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
	
	TableRealmsProxyList: function(ptrGameObjectName, ptrSessionId, ptrServiceUrl) {
		var gameObjectName=(" "+Pointer_stringify(ptrGameObjectName)).slice(1);
		var sessionId=(" "+Pointer_stringify(ptrSessionId)).slice(1);
		var serviceUrl=(" "+Pointer_stringify(ptrServiceUrl)).slice(1);
	
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
				console.log("TableRealsm: Error received from list call")
			}
		};

		xhttp.send(JSON.stringify(request));
	}

});