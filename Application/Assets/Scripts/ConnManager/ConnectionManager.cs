using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using UnityEngine;
using Assets.Scripts.Configs;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
#if WINDOWS_UWP
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
//using Windows.Security.Cryptography;
//using Windows.Foundation;
#endif

namespace Assets.Scripts.ConnManager
{
    class ConnectionManager: MonoBehaviour
    {
        public static ConnectionManager instance;

        public delegate void CallbackEventHandler(byte[] sender);
        public event CallbackEventHandler Callback;

#if WINDOWS_UWP
        private static Windows.Networking.Sockets.MessageWebSocket messageWebSocket;
        private static Task connectTask;
#endif

        public ConnectionManager()
        {
            if (instance == null) 
            {
                instance = this;
            }
        }

        /*
         * Please BE AWARE that this function should be ONLY called once in the MainDataController; otherwise, the event binding or other exception can be existed for current version.
         */
        public void Connect() 
        {
            string urlStr = Configs.Global.WEBSOCKETS_PROTOCOL + "://" + Configs.Global.SERVER_IP_ADDR + ":" + Configs.Global.WEBSOCKETS_PORT.ToString();
            //Debug.Log(urlStr);

#if WINDOWS_UWP
            if (messageWebSocket != null ) {
                Debug.Log("Connection was already established. Reusing.");
                return;
            }
            
            messageWebSocket = new Windows.Networking.Sockets.MessageWebSocket();
            messageWebSocket.Control.MessageType = Windows.Networking.Sockets.SocketMessageType.Utf8;
            messageWebSocket.MessageReceived += WebSocket_MessageReceived;
            messageWebSocket.Closed += WebSocket_Closed;

            try
            {
                connectTask = messageWebSocket.ConnectAsync(new Uri(urlStr)).AsTask();
            }
            catch (Exception ex)
            {
                Windows.Web.WebErrorStatus webErrorStatus = Windows.Networking.Sockets.WebSocketError.GetStatus(ex.GetBaseException().HResult);
                // Add additional code here to handle exceptions.
                Debug.Log("*******: Error when establishing the connection.");
            }
#endif

        }

        public void SendData(string dataStr)
        {
#if WINDOWS_UWP
            try
            {
                connectTask.ContinueWith(_ => this.SendMessageUsingMessageWebSocketAsync(dataStr));
            }
            catch (Exception ex)
            {
                Windows.Web.WebErrorStatus webErrorStatus = Windows.Networking.Sockets.WebSocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.Log("*******: Error when sending data!");
            }
#endif
        }

        public void CloseConnection()
        {
            string exitMessage = "{\"id\":1, \"cmd\": \"exit\", \"para\": \"\"}";
            Debug.Log("Close Connection. " + exitMessage);
            SendData(exitMessage);
#if WINDOWS_UWP
            //IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(exitMessage, BinaryStringEncoding.Utf8);
            //messageWebSocket.SendFinalFrameAsync(buffer);
            messageWebSocket.Dispose();
#endif
        }


#if WINDOWS_UWP
        private async Task SendMessageUsingMessageWebSocketAsync(string message)
        {
            using (var dataWriter = new DataWriter(messageWebSocket.OutputStream))
            {
                dataWriter.WriteString(message);
                await dataWriter.StoreAsync();
                dataWriter.DetachStream();
            }
            Debug.Log("**********************Sending message using MessageWebSocket: " + message);
        }

        private void WebSocket_MessageReceived(Windows.Networking.Sockets.MessageWebSocket sender, Windows.Networking.Sockets.MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                using (DataReader dataReader = args.GetDataReader())
                {
                    dataReader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    var message = new byte[dataReader.UnconsumedBufferLength];
                    dataReader.ReadBytes(message);
                    // string message = dataReader.ReadString(dataReader.UnconsumedBufferLength);
                    Debug.Log("**********************Message received from MessageWebSocket: " + System.Text.Encoding.UTF8.GetString(message));
                    this.Callback(message);
                    // this.messageWebSocket.Dispose();
                }
            }
            catch (Exception ex)
            {
                Windows.Web.WebErrorStatus webErrorStatus = Windows.Networking.Sockets.WebSocketError.GetStatus(ex.GetBaseException().HResult);
                // Add additional code here to handle exceptions.
            }
        }

        private void WebSocket_Closed(Windows.Networking.Sockets.IWebSocket sender, Windows.Networking.Sockets.WebSocketClosedEventArgs args)
        {
            Debug.Log("**********************WebSocket_Closed; Code: " + args.Code + ", Reason: \"" + args.Reason + "\"");
            // Add additional code here to handle the WebSocket being closed.
        }
#endif


        /*
         * LoadImageTo
         * This function will load an image from a web server to an Image object. 
         * It needs to be execuated by StartCoroutine or something like that.
         * 
         * I don't recommend to use this function because the processing of creating the sprite can spend a lot of time depended on the size of Image.
         * 
         * @obj is an object that can be obtained by `GameObject.GetComponentInChildren<Image>()` and so on.
         * @imagePath contains the path and filename like `/DB/Wenbin/wenbin_face.jpg`
         */
        public IEnumerator LoadImageTo(Image obj, string imagePath, int width, int height)
        {
            Debug.Log("Load image from : " + "http://" + Global.SERVER_IP_ADDR + imagePath);

            using (UnityWebRequest webRequest = UnityWebRequest.Get("http://" + Global.SERVER_IP_ADDR + imagePath))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                }
                else
                {
                    Debug.Log("Image Data Received.");
                    // Load
                    Texture2D texture = new Texture2D(400, 300);
                    texture.LoadImage(webRequest.downloadHandler.data);

                    obj.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    obj.preserveAspect = true;
                    yield break;
                }
            }
        }

        /*
         * LoadRawImageTo
         * This function will load an image from a web server to an RawImage object. 
         * It needs to be execuated by StartCoroutine or something like that.
         * 
         * @obj is an object that can be obtained by `GameObject.GetComponentInChildren<RawImage>()` and so on.
         * @imagePath contains the path and filename like `/DB/Wenbin/wenbin_face.jpg`
         */
        public IEnumerator LoadRawImageTo(RawImage obj, string imagePath, int width, int height)
        {
            Debug.Log("Load image from : " + "http://" + Global.SERVER_IP_ADDR + imagePath);

            using (UnityWebRequest webRequest = UnityWebRequest.Get("http://" + Global.SERVER_IP_ADDR + imagePath))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                }
                else
                {
                    Debug.Log("Image Data Received.");
                    // Load
                    Texture2D texture = new Texture2D(width, height);
                    texture.LoadImage(webRequest.downloadHandler.data);
                    obj.texture = texture;

                    yield break;
                }
            }
        }
    }
}