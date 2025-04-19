using Supermarket.Core.Dtos.Common;
using Supermarket.Core.Services.Interfaces;
using Supermarket.Core.ViewModels.Commands;
using System.Collections.ObjectModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;

namespace Supermarket.Core.ViewModels.Receipts
{
    public class SaveReceiptVM : BaseVM
    {
        private readonly IReceiptService _receiptService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public ReceiptDto ReceiptDto { get; set; }
        public string Title { get; set; }

        public ObservableCollection<UserDto> Users { get; set; }
        public ObservableCollection<SoldProductDto> SoldProducts { get; set; }
        public ObservableCollection<ProductDto> Products { get; set; }

        private UserDto selectedUser;
        public UserDto SelectedUser 
        { 
            get => selectedUser; 
            set 
            { 
                selectedUser = value; 
                ReceiptDto.IssuerId = value?.Id ?? Guid.Empty; 
                OnPropertyChanged(nameof(SelectedUser)); 
            } 
        }

        private ProductDto selectedProduct;
        public ProductDto SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                SelectedProduct.Id = value?.Id ?? Guid.Empty;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        private int selectedProductQuantity;
        public int SelectedProductQuantity
        {
            get => selectedProductQuantity;
            set
            {
                selectedProductQuantity = value;
                OnPropertyChanged(nameof(SelectedProductQuantity));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddProductCommand { get; }

        public SaveReceiptVM(IReceiptService receiptService, IProductService productService, IUserService userService)
        {
            _receiptService = receiptService;
            _userService = userService;
            _productService = productService;
            ReceiptDto = new ReceiptDto();
            Users = new ObservableCollection<UserDto>(_userService.GetAll());
            Products = new ObservableCollection<ProductDto>(_productService.GetAll());
            SaveCommand = new RelayCommand(o => Save());
            CancelCommand = new RelayCommand(o => Cancel());
            AddProductCommand = new RelayCommand(o => AddProductToReceipt());
            Title = "Add Receipt";
        }

        public SaveReceiptVM(IReceiptService receiptService, IUserService userService, ReceiptDto receiptDto)
        {
            _receiptService = receiptService;
            _userService = userService;
            ReceiptDto = receiptDto;
            Users = new ObservableCollection<UserDto>(_userService.GetAll());
            SelectedUser = Users.FirstOrDefault(user => user.Id == ReceiptDto.IssuerId);
            SaveCommand = new RelayCommand(o => Save());
            CancelCommand = new RelayCommand(o => Cancel());
            Title = "Update Receipt";
        }

        private void AddProductToReceipt()
        {
            IList<SoldProductDto> soldProductDtos = _receiptService.AddProductToReceipt(ReceiptDto.Id, SelectedProduct.Id, SelectedProductQuantity);
            SoldProducts = new ObservableCollection<SoldProductDto>(soldProductDtos);
        }

        private void Save()
        {
            if (ReceiptDto.Id == Guid.Empty)
                _receiptService.Add(ReceiptDto);
            else
                _receiptService.UpdateById(ReceiptDto, ReceiptDto.Id);
            Cancel();
        }

        private void Cancel() => Application.Current.Windows[Application.Current.Windows.Count - 1].Close();
    }
}