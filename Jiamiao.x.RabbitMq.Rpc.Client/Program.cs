using System;

namespace Jiamiao.x.RabbitMq.Rpc.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== RPC.Client ==========");

            var rpcClient = new RpcClient();
            Console.WriteLine(" [x] Requesting Fib(30)");
            var response = rpcClient.Call("30");
            Console.WriteLine($" [.] Got {response}");
            rpcClient.Close();
            Console.WriteLine(" Press anyKey to exit.");
            Console.ReadKey();
        }
    }
}
