using Supermarket.Core.Dtos.Common;
using Supermarket.Core.Mappings;
using Supermarket.Core.Repositories.Interfaces;
using Supermarket.Core.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Supermarket.Core.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISoldProductRepository _soldProductRepository;

        public ReceiptService(IReceiptRepository receiptRepository, IUserRepository userRepository, ISoldProductRepository soldProductRepository)
            => (_receiptRepository, _userRepository, _soldProductRepository) = (receiptRepository, userRepository, soldProductRepository);

        public IList<ReceiptDto> GetAll() => _receiptRepository.GetAll().ToDtos();

        public IList<ReceiptDto> GetByKey(string key) => _receiptRepository.GetByIssuerNameContains(key).ToDtos();

        public ReceiptDto GetById(Guid id) => _receiptRepository.GetById(id).ToDto();

        public ReceiptDto Add(ReceiptDto receiptDto) => _receiptRepository.Add(receiptDto.ToEntity(_userRepository, _soldProductRepository)).ToDto();

        public ReceiptDto UpdateById(ReceiptDto receiptDto, Guid id) => _receiptRepository.UpdateById(receiptDto.ToEntity(_userRepository, _soldProductRepository), id).ToDto();

        public ReceiptDto DeleteById(Guid id) => _receiptRepository.DeleteById(id).ToDto();

        public IList<SoldProductDto> AddProductToReceipt(Guid receiptId, Guid productId, int quantity)
        {
            var receipt = _receiptRepository.GetById(receiptId);
            var soldProduct = _soldProductRepository.GetById(productId);
            soldProduct.Quantity = quantity;
            soldProduct.Subtotal = soldProduct.Product.Price * quantity;
            receipt.SoldProducts.Add(soldProduct);
            receipt.Total += soldProduct.Subtotal;
            _receiptRepository.UpdateById(receipt, receiptId);
            return receipt.SoldProducts.ToDtos();
        }
    }
}