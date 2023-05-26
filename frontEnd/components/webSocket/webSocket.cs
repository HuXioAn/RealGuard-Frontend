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
        private string? currentToken = null;
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

                if(wsServiceAuth(msg)){
                    //pass
                    
                    //reply 下发token

                    authList.Add(socket);
                }else{
                    //fail
                    //reply

                }
            }else{
                //已认证
                var request = wsRequestParse(msg);
                if(null == request)return;

                //check token


                switch (request.request)
                {
                    case "" :

                    break;

                    default:

                    break;
                }

            }
        }

        private bool wsServiceAuth(string msg){




            return false;
        }


        private wsRequest? wsRequestParse(string requestStr){
            //用于首先解析行为
            var request = JsonSerializer.Deserialize<wsRequest>(requestStr);
            if(request != null)return request;
            else
            return null;
        }

        public string wsTokenGen(){
            if(authList.Count() == 0){
                //gen new

                ;
            }else{
                //return old 
                if(currentToken != null)return currentToken;
                else throw new Exception("[!]Token error");
            }
        }



    }


    


}