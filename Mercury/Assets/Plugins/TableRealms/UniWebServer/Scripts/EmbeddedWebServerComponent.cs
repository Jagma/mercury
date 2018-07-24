using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System;

namespace UniWebServer
{

    public class EmbeddedWebServerComponent : MonoBehaviour
    {
        public bool startOnAwake = true;
        public bool disposeWithObject = true;
        public int port = 8079;
        public int workerThreads = 2;
        public bool processRequestsInMainThread = true;
        public bool logRequests = true;

        WebServer server;
        Dictionary<string, IWebResource> resources = new Dictionary<string, IWebResource> ();

        void Start ()
        {
            if (processRequestsInMainThread)
                Application.runInBackground = true;
            server = new WebServer (port, workerThreads, processRequestsInMainThread);
            server.logRequests = logRequests;
            server.HandleRequest += HandleRequest;
            if (startOnAwake) {
                server.Start ();
            }
        }

        private void OnDestroy() {
            if (disposeWithObject) {
                Debug.LogError("Closing Server");
                server.Stop();
                server.Dispose();
            }
        }

        void OnApplicationQuit ()
        {
            if (!disposeWithObject) {
                server.Stop();
                server.Dispose();
            }
        }

        void Update ()
        {
            if (server.processRequestsInMainThread) {
                server.ProcessRequests ();    
            }
        }

        void HandleRequest (Request request, Response response)
        {
            String key = request.uri.LocalPath.ToLower();
            if (resources.ContainsKey (key)) {
                try {
                    resources [key].HandleRequest (request, response);
                } catch (Exception e) {
                    response.statusCode = 500;
                    response.Write (e.Message);
                }
            } else {
                response.statusCode = 404;
                response.message = "Not Found.";
                response.Write (request.uri.LocalPath + " not found.");
            }
        }

        public void AddResource (string path, IWebResource resource)
        {
            if (!path.StartsWith("/")) {
                path = "/" + path;
            }
            Debug.Log("WebResrouce:"+path);
            resources [path.ToLower()] = resource;
        }

    }



}