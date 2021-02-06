using System;
using System.Collections;
using System.Collections.Generic;
using NativeWebSocket;
using Unity.Collections;
using UnityEngine;

namespace ndmg.UniversalGameServer
{
    public delegate void OnMessageHandler(string data);

    public class UniversalGameServerClient : MonoBehaviour
    {
        WebSocket websocket;


        [Header("Settings")] public string url = "ws://localhost:8080/";

        public bool connectOnStart = true;

        [HideInInspector] public bool connected = false;


        public Dictionary<string, List<OnMessageHandler>> messageHandlers =
            new Dictionary<string, List<OnMessageHandler>>();


        public async void Disconnect()
        {
            await websocket.Close();
        }

        public async void Connect()
        {
            websocket = new WebSocket(url);

            websocket.OnOpen += () => { Debug.Log("Connection open!"); };

            websocket.OnError += (e) => { Debug.Log("Error! " + e); };

            websocket.OnClose += (e) => { Debug.Log("Connection closed!"); };

            websocket.OnMessage += (bytes) =>
            {
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                var unpackedMessage = Utility.UnpackMessage(message);


                if (messageHandlers.ContainsKey(unpackedMessage[0]))
                {
                    foreach (var handler in messageHandlers[unpackedMessage[0]])
                    {
                        handler.Invoke(unpackedMessage[1]);
                    }
                }
            };

            await websocket.Connect();
        }

        private async void OnApplicationQuit()
        {
            await websocket.Close();
        }

        public void Start()
        {
            if (connectOnStart)
            {
                Connect();
            }
        }

        public void On(string ev, OnMessageHandler messageHandler)
        {
            if (messageHandlers.ContainsKey(ev))
            {
                messageHandlers[ev].Add(messageHandler);
            }
            else
            {
                messageHandlers.Add(ev, new List<OnMessageHandler>());

                messageHandlers[ev].Add(messageHandler);
            }
        }

        void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
#endif
        }


        public async void Send(string ev, string data)
        {
            if (websocket.State == WebSocketState.Open)
            {
                await websocket.SendText(ev + " " + data);
            }
        }
    }
}