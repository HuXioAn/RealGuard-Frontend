using System;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace realWebSocketServer{

    public class wsRequest{

        public string request{set; get;}

        public int requestId{set; get;}

        public string? token{set; get;}

    }


    public class wsRequestAuth : wsRequest{
        public string username{set; get;}
        public string password{set; get;}

    }


    public class wsRequestRegister : wsRequest{
        public string name{set; get;}
        public string studentId{set; get;}

    }


    public class wsRequestSnap : wsRequest{

    }


    public class wsRequestCheck : wsRequest{
        public int picId{set; get;}
        public string result{set; get;} //accept reject
    }


    public class wsRequestOver : wsRequest{
        
    }



}