using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ShippingService;

namespace PaymentService.Services
{
    public class PaymentService : PaymentServicce.PaymentServicceBase
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly ProductShipment.ProductShipmentClient _shippings;
        public PaymentService(ILogger<PaymentService> logger, ProductShipment.ProductShipmentClient shippings)
        {
            _logger = logger;
            _shippings = shippings;
        }

        public override async Task<MakePaymentReply> MakePayment(MakePaymentRequest request, ServerCallContext context)
        {
            var transactionId = Guid.NewGuid().ToString();
            _logger.LogInformation($"Make payment {transactionId}");
            await _shippings.SendOrderAsync(new SendOrderRequest
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Address = request.Address,
                OrderId = new Guid("A3CDAD9BF7FA4699AE38CB68278089FB").ToString()
            });

            return (new MakePaymentReply
            {
                TransactionId = transactionId
            });
        }

        public override async Task GetPaymentStatus(GetPaymentStatusRequest request, IServerStreamWriter<GetPaymentStatusResponse> responseStream, ServerCallContext context)
        {
            await responseStream.WriteAsync(
                new GetPaymentStatusResponse { Status = "Created" });
            await Task.Delay(500);
            await responseStream.WriteAsync(
                new GetPaymentStatusResponse { Status = "Validated" });
            await Task.Delay(1000);
            await responseStream.WriteAsync(
                new GetPaymentStatusResponse { Status = "Accepted" });
        }
    }
}
