using Shop.Application.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Services;
public interface IOrderQueueProcessor
{
    /// <returns><see langword="true"/> when the message was handled and can be acknowledged.</returns>
    Task<bool> ProcessAsync(QueuedOrderDTO order, CancellationToken cancellationToken);
}
