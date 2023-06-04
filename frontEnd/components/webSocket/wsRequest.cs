using System;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace realWebSocketServer{

    public class wsRequest{

        public string request{set; get;} = "";

        public int requestId{set; get;}

        public string? token{set; get;}

    }


    public class wsRequestAuth : wsRequest{
        /*
            request : auth
            requestId : 由web端生成
            token : 留空字符串
        */
        public string username{set; get;} = "";
        public string password{set; get;} = "";

    }


    public class wsRequestRegister : wsRequest{
        /*
            request : register
            requestId : 由web端生成
            token : 被分到的token
        */
        public string name{set; get;} = "";
        public string studentId{set; get;} = "";

    }


    public class wsRequestSnap : wsRequest{
        /*
            request : snap
            requestId : 由web端生成
            token : 被分到的token
        */

    }


    public class wsRequestCheck : wsRequest{
        /*
            request : check
            requestId : 由web端生成
            token : 被分到的token
            picId : 在snap的reply中由前端给出的picId，表征上一张照片
            result : 由用户给出的判别 通过accept 不通过reject
        */
        public int picId{set; get;}
        public string result{set; get;} = ""; //accept reject
    }


    public class wsRequestOver : wsRequest{
        /*
            request : over
            requestId : 由web端生成
            token : 被分到的token
        */
        
    }



}