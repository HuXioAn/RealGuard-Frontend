using System;
using static System.Console;
using System.Threading.Tasks;
using Grpc.Net.Client;
using realGuardRpc;


namespace grpcClient{

    public class grpcClient{
        private string? address;

        private auth.authClient client;
        private GrpcChannel channel;
        public grpcClient(string serverAddress){
            address = serverAddress;
            try{
                channel = GrpcChannel.ForAddress(address);
                client = new auth.authClient(channel);
            }catch(Exception e){

                throw new Exception("Unable to establish the grpc channel: " + e.Message);
            }

            
        }

        public async Task<auth_result> authRequstAsync(string irImgPath, byte[] depthData){
            var request = new auth_request { 
                                TimeStamp = (UInt64)DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds,
                                IrImg = Google.Protobuf.ByteString.CopyFrom(File.ReadAllBytes(irImgPath)),
                                DepthData = Google.Protobuf.ByteString.CopyFrom(depthData)
                                };
            try{
                return await client.do_authAsync(request);
            }catch(Exception e){
                throw new Exception("Error talking to backend:"+e.Message);
            }
            
        }


        public async Task<auth_result> authRequstAsync(Stream irImgStream, byte[] depthData){
            var request = new auth_request { 
                                TimeStamp = (UInt64)DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds,
                                IrImg = Google.Protobuf.ByteString.FromStream(irImgStream),
                                DepthData = Google.Protobuf.ByteString.CopyFrom(depthData)
                                };
            try{
                return await client.do_authAsync(request);
            }catch(Exception e){
                throw new Exception("Error talking to backend:"+e.Message);
            }
            
        }
    

    
    }


}