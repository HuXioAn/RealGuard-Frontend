using System;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace realWebSocketServer{

    public class wsReply{

        public string state{set; get;}

        public int requestId{set; get;}


    }


    public class wsReplyAuth : wsReply{
        public string username{set; get;}
        public string token{set; get;}

    }


    public class wsReplyRegister : wsReply{
        public string name{set; get;}
    }


    public class wsReplySnap : wsReply{
        public string picBase64{set; get;}
        public int picId{set; get;}
        public float dist{set; get;}
    }

    public class wsReplyCheck : wsReply{
        public int picId{set; get;}
        public string result{set; get;}
    }


    public class wsReplyOver : wsReply{
        
    }



}