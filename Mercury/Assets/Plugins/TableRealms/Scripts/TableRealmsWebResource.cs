using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniWebServer;

public class TableRealmsHTTPResponse : IWebResource {
    private byte[] data;
    private string contentType;
    private bool binary = false;

    public TableRealmsHTTPResponse(string contentType, byte[] data, bool binary) {
        this.contentType = contentType;
        this.data = data;
        this.binary = binary;
    }

    public void HandleRequest(Request request, Response response) {
        response.statusCode = 200;
	    response.message = "OK.";
        response.binary = binary;
        response.headers=new Headers();
        response.headers.Add("Content-Type", contentType);
        response.stream.Write(data, 0, data.Length);
    }
}
