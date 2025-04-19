using Supermarket.Core.Dtos.Common;
using Supermarket.Core.Services.Interfaces;
using Supermarket.Core.ViewModels.Commands;
using Supermarket.Core.Views.Receipts;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;

namespace Supermarket.Core.ViewModels.Receipts
{
    public class ReceiptsVM : BaseVM
    {
        private readonly IReceiptService _receiptService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public ObservableCollection<ReceiptDto> ReceiptDtos { get; set; }
        public string SearchKey { get; set; }
        public event Action OnClose;

        public ICommand AddReceiptCommand { get; }
        public ICommand UpdateReceiptCommand { get; }
        public ICommand DeleteReceiptCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand CloseCommand { get; }

        public ReceiptsVM(IReceiptService receiptService, IProductService productService, IUserService userService)
        {
            _receiptService = receiptService;
            _userService = userService;
            _productService = productService;
            ReceiptDtos = new ObservableCollection<ReceiptDto>(_receiptService.GetAll());
            AddReceiptCommand = new RelayCommand(o => AddReceipt());
            UpdateReceiptCommand = new RelayCommand(o => UpdateReceipt(o as ReceiptDto));
            DeleteReceiptCommand = new RelayCommand(o => DeleteReceipt(o as ReceiptDto));
            ViewDetailsCommand = new RelayCommand(o => ViewDetails(o as ReceiptDto));
            SearchCommand = new RelayCommand(o => GetByKey());
            CloseCommand = new RelayCommand(o => OnClose?.Invoke());
        }

        private void AddReceipt()
        {
            var saveReceiptView = new SaveReceiptView(new SaveReceiptVM(_receiptService, _productService, _userService));
            saveReceiptView.ShowDialog();
            RefreshReceipts();
        }

        private void UpdateReceipt(ReceiptDto receiptDto)
        {
            var saveReceiptView = new SaveReceiptView(new SaveReceiptVM(_receiptService, _userService, receiptDto));
            saveReceiptView.ShowDialog();
            RefreshReceipts();
        }

        private void DeleteReceipt(ReceiptDto receiptDto)
        {
            _receiptService.DeleteById(receiptDto.Id);
            RefreshReceipts();
        }

        private void ViewDetails(ReceiptDto receiptDto)
        {
            var receiptDetailsView = new ReceiptDetailsView(new ReceiptDetailsVM(receiptDto));
            receiptDetailsView.ShowDialog();
        }

        private void GetByKey()
        {
            ReceiptDtos = new ObservableCollection<ReceiptDto>(_receiptService.GetByKey(SearchKey));
            OnPropertyChanged(nameof(ReceiptDtos));
        }

        private void RefreshReceipts()
        {
            ReceiptDtos = new ObservableCollection<ReceiptDto>(_receiptService.GetAll());
            OnPropertyChanged(nameof(ReceiptDtos));
        }
    }
}