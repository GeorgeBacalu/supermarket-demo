using Supermarket.Core.Dtos.Common;
using System;
using System.Collections.Generic;

namespace Supermarket.Core.Services.Interfaces
{
    public interface IReceiptService
    {
        IList<ReceiptDto> GetAll();

        IList<ReceiptDto> GetByKey(string key);

        ReceiptDto GetById(Guid id);

        ReceiptDto Add(ReceiptDto receiptDto);

        ReceiptDto UpdateById(ReceiptDto receiptDto, Guid id);

        ReceiptDto DeleteById(Guid id);

        IList<SoldProductDto> AddProductToReceipt(Guid receiptId, Guid productId, int quantity);
    }
}