using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace ShippingService
{
    public class ShippingService : ProductShipment.ProductShipmentBase
    {
        private readonly ILogger<ShippingService> _logger;
        public ShippingService(ILogger<ShippingService> logger)
        {
            _logger = logger;
        }

        public override async Task<SendOrderReply> SendOrder(SendOrderRequest request, ServerCallContext context)
        {
            this._logger.LogInformation($"Sent order {request.OrderId}");
            return new SendOrderReply
            {
                Ok = true
            };
        }
    }
}
