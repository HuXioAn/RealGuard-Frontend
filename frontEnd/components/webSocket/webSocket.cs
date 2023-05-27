using Fleck;
using System;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;
using realGuardFrontEnd;


namespace realWebSocketServer
{
    public class realWebSocketServer{

        private List<IWebSocketConnection> aliveList =  new List<IWebSocketConnection>();
        private List<IWebSocketConnection> authList =  new List<IWebSocketConnection>();
        private string? currentToken = null;
        private WebSocketServer? wsServer;

        private string wsAddress;

        public bool registering = false;

        private string registerName = "";
        private string registerId = "";

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

                var request = wsRequestParse(msg);
                if(null == request)return;

                if(wsServiceAuth(msg)){
                    //pass
                    
                    //reply 下发token
                    var token = wsTokenGen();
                    var authReply = new wsReplyAuth{
                        state = "authOk",
                        requestId = request.requestId,
                        username = "focus_realguard",
                        token = token
                    };
                    var authReplyAuth = JsonSerializer.Serialize<wsReplyAuth>(authReply);
                    socket.Send(authReplyAuth);
                    authList.Add(socket);
                }else{
                    //fail
                    //reply
                    var authReply = new wsReplyAuth{
                        state = "authFail",
                        requestId = request.requestId,
                        username = "",
                        token = ""
                    };
                    var authReplyJson = JsonSerializer.Serialize<wsReplyAuth>(authReply);
                    socket.Send(authReplyJson);
                }
            }else{
                //已认证
                var request = wsRequestParse(msg);
                if(null == request)return;

                //check token
                if(!wsTokenCheck(request.token!))return;

                switch (request.request)
                {
                    case "register" :
                        if(registering == true)return;
                        
                        var registerRequest = JsonSerializer.Deserialize<wsRequestRegister>(msg);
                        if(registerRequest == null)return;

                        registering = true;
                        registerName = registerRequest.name;
                        registerId = registerRequest.studentId;

                        var registerReply = new wsReplyRegister{
                            state = "ready",
                            requestId = request.requestId,
                            name = registerName
                        };
                        var registerReplyJson = JsonSerializer.Serialize<wsReplyRegister>(registerReply);
                        socket.Send(registerReplyJson);

                        break;
                    case "snap" :
                        if(registering == false)return;

                        var snapRequest = JsonSerializer.Deserialize<wsRequestSnap>(msg);
                        if(snapRequest == null)return;

                        //snap
                        var picStream = realGuardFrontEnd.frontEnd.snap();
                        //check the pic with backend
                        var dist = realGuardFrontEnd.frontEnd.registerCheck(picStream, registerName, registerId);

                        //stream -> base64
                        var picByte = new byte[picStream.Length];
                        if(picStream.Read(picByte, 0, picByte.Length) > 0){
                            var picBase64 = Convert.ToBase64String(picByte);
                        }else return;
                        
                 

                        break;
                    case "check" :
                        if(registering == false)return;

                        var checkRequest = JsonSerializer.Deserialize<wsRequestCheck>(msg);
                        if(checkRequest == null)return;

                        //check


                        var checkReply = new wsReplyCheck{
                            state = "ok",
                            requestId = request.requestId,
                            picId = checkRequest.picId,
                            result = checkRequest.result
                        };
                        var checkReplyJson = JsonSerializer.Serialize<wsReplyCheck>(checkReply);
                        socket.Send(checkReplyJson);

                        break;

                    case "over" :
                        if(registering == false)return;

                        registering = false;
                        
                        var overReply = new wsReplyOver{
                            state = "over",
                            requestId = request.requestId,
                        };
                        var overReplyJson = JsonSerializer.Serialize<wsReplyOver>(overReply);
                        socket.Send(overReplyJson);

                        break;

                    default:
                        return;
                    
                }

            }
        }

        private bool wsServiceAuth(string msg){

            var request = wsRequestParse(msg);
            if(null == request)return false;

            if(request.request != "auth")return false;
            else{
                var authRequest = JsonSerializer.Deserialize<wsRequestAuth>(msg);
                if(null == authRequest)return false;

                if(authRequest.username == "focus_realguard" && authRequest.password == "crazy_thursday")
                return true;
                else return false;

            }


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
            // if(authList.Count() == 0){
            //     //gen new

            //     ;
            // }else{
            //     //return old 
            //     if(currentToken != null)return currentToken;
            //     else throw new Exception("[!]Token error");
            // }
            return "huxiaoan";
        }

        public bool wsTokenCheck(string token){

            if(token == "huxiaoan")return true;
            else return false;
        }



    }


    


}