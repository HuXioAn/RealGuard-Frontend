using System;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace realWebSocketServer{

    public class wsReply{

        public string state{set; get;} = "";

        public int requestId{set; get;}


    }


    public class wsReplyAuth : wsReply{
        /*
            state: authOk authFail
            requestId: 重复web端发来的id
            username: 重复认证收到的用户名，失败为空
            token: 认证通过后分发的token，失败为空
        */

        public string username{set; get;} = "";
        public string token{set; get;} = "";

    }


    public class wsReplyRegister : wsReply{
        /*
            state: ready
            requestId: 重复web端发来的id
            name: 重复收到的录入名称
        */

        public string name{set; get;} = "";
    }


    public class wsReplySnap : wsReply{
        /*
            state: ok fail
            requestId: 重复web端发来的id
            picBase64: 图片base64文本 失败情况下为errMsg
            picId: 代表图片的id，由前端给出
            dist: 对于拍摄的照片的验证，在特殊情况（错误、第一张、未检测到）下为-1，其余应0~1
        */

        public string picBase64{set; get;} = "";
        public int picId{set; get;}
        public float dist{set; get;}
    }

    public class wsReplyCheck : wsReply{
        /*
            state: ok fail
            requestId: 重复web端发来的id
            picId: 代表图片的id，由前端拍摄时给出
            result: 重复web端发来的判决结果 accept reject
        */

        public int picId{set; get;}
        public string result{set; get;} = "";
    }


    public class wsReplyOver : wsReply{
        /*
            state: ok fail
            requestId: 重复web端发来的id
        */
    }



}