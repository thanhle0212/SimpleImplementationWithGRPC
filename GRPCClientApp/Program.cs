using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using PaymentService;

namespace GRPCClientApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var paymentClient = new PaymentServicce.PaymentServicceClient(channel);

            Console.WriteLine("Welcome to the gRPC client");
            var reply = await paymentClient.MakePaymentAsync(new MakePaymentRequest
            {
                ProductId = "P0001",
                Quantity = 1,
                Address = "Viet Nam"
            });
            Console.WriteLine($"Made payment: {reply.TransactionId}");

            using var statusReplies = paymentClient.GetPaymentStatus(new GetPaymentStatusRequest() { TransactionId = reply.TransactionId });
            while (await statusReplies.ResponseStream.MoveNext())
            {
                var statusReply = statusReplies.ResponseStream.Current.Status;
                Console.WriteLine($"Payment status: {statusReply}");
            }
        }
    }
}
