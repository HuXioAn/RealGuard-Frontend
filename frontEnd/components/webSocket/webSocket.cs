using Fleck;
using System;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace realWebSocketServer
{
    public class realWebSocketServer{

        private List<IWebSocketConnection> aliveList =  new List<IWebSocketConnection>();
        private List<IWebSocketConnection> authList =  new List<IWebSocketConnection>();
        private WebSocketServer? wsServer;

        private string wsAddress;

        public realWebSocketServer(string address){
            wsAddress = address;
            wsServer = new WebSocketServer(wsAddress);
            
            wsServer.Start(socket => {
                socket.OnOpen = () => {
                    aliveList.Add(socket);
                };
                socket.OnMessage = (message) => {
                    wsMessage(socket,message);
                };
                socket.OnClose = () => 
                {
                    aliveList.Remove(socket);
                    if(authList.Contains(socket))authList.Remove(socket);
                };
                socket.OnError = (e) => {
                    aliveList.Remove(socket);
                    if(authList.Contains(socket))authList.Remove(socket);
                    Console.WriteLine("[!]WS Server error: "+e.Message+" CLOSED!");
                };
            });


        }

        private void wsMessage(IWebSocketConnection socket,string msg){
            //服务业务逻辑

            //auth
            if(!authList.Contains(socket)){
                //未认证 只能认证

            }else{
                //已认证

            }
        }

        private bool wsServiceAuth(string msg){




            return false;
        }


    }


    


}