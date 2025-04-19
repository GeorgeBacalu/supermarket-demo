using Supermarket.Core.ViewModels.Commands;
using Supermarket.Core.Dtos.Request;
using Supermarket.Core.Services.Interfaces;
using System.Windows.Input;
using Supermarket.Core.Views.Auth;
using Supermarket.Core.Views.Users;
using Supermarket.Core.Views.Categories;
using Supermarket.Core.Views.Manufacturers;
using Supermarket.Core.Views.Offers;
using Supermarket.Core.Views.Products;
using Supermarket.Core.Views.Stocks;
using Supermarket.Core.ViewModels.Users;
using Supermarket.Core.ViewModels.Categories;
using Supermarket.Core.ViewModels.Manufacturers;
using Supermarket.Core.ViewModels.Offers;
using Supermarket.Core.ViewModels.Products;
using Supermarket.Core.ViewModels.Receipts;
using Supermarket.Core.ViewModels.Stocks;
using System;
using Supermarket.Core.Views.Receipts;

namespace Supermarket.Core.ViewModels
{
    public class MainVM : BaseVM
    {
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IOfferService _offerService;
        private readonly IProductService _productService;
        private readonly IReceiptService _receiptService;
        private readonly IStockService _stockService;
        public event Action OnClose;

        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand AccessRegisterCommand { get; }
        public ICommand AccessLoginCommand { get; }
        public ICommand AccessUsersCommand { get; }
        public ICommand AccessCategoriesCommand { get; }
        public ICommand AccessManufacturersCommand { get; }
        public ICommand AccessOffersCommand { get; }
        public ICommand AccessProductsCommand { get; }
        public ICommand AccessReceiptsCommand { get; }
        public ICommand AccessStocksCommand { get; }

        private RegisterRequest _registerPayload;
        public RegisterRequest registerPayload 
        { 
            get => _registerPayload; 
            set 
            { 
                _registerPayload = value; 
                OnPropertyChanged(nameof(registerPayload)); 
            } 
        }

        private LoginRequest _loginPayload;
        public LoginRequest loginPayload 
        { 
            get => _loginPayload; 
            set 
            { 
                _loginPayload = value; 
                OnPropertyChanged(nameof(loginPayload));
            }
        }

        public MainVM(IUserService userService, 
            ICategoryService categoryService, 
            IManufacturerService manufacturerService, 
            IOfferService offerService,
            IProductService productService,
            IReceiptService receiptService,
            IStockService stockService)
        {
            _userService = userService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _offerService = offerService;
            _productService = productService;
            _receiptService = receiptService;
            _stockService = stockService;

            RegisterCommand = new RelayCommand(o => Register());
            LoginCommand = new RelayCommand(o => Login());
            AccessRegisterCommand = new RelayCommand(o => AccessRegister());
            AccessLoginCommand = new RelayCommand(o => AccessLogin());

            AccessUsersCommand = new RelayCommand(o => AccessUsers());
            AccessCategoriesCommand = new RelayCommand(o => AccessCategories());
            AccessManufacturersCommand = new RelayCommand(o => AccessManufacturers());
            AccessOffersCommand = new RelayCommand(o => AccessOffers());
            AccessProductsCommand = new RelayCommand(o => AccessProducts());
            AccessReceiptsCommand = new RelayCommand(o => AccessReceipts());
            AccessStocksCommand = new RelayCommand(o => AccessStocks());

            registerPayload = new RegisterRequest();
            loginPayload = new LoginRequest();
        }

        private void AccessRegister() => new RegisterView(this).Show();

        private void AccessLogin() => new LoginView(this).Show();

        private void AccessUsers() => new UsersView(new UsersVM(_userService)).Show();

        private void AccessCategories() => new CategoriesView(new CategoriesVM(_categoryService)).Show();

        private void AccessManufacturers() => new ManufacturersView(new ManufacturersVM(_manufacturerService)).Show();

        private void AccessOffers() => new OffersView(new OffersVM(_offerService, _productService)).Show();

        private void AccessProducts() => new ProductsView(new ProductsVM(_productService, _categoryService, _manufacturerService)).Show();

        private void AccessReceipts() => new ReceiptsView(new ReceiptsVM(_receiptService, _productService, _userService)).Show();

        private void AccessStocks() => new StocksView(new StocksVM(_stockService, _productService)).Show();

        private void Register()
        {
            _userService.Register(registerPayload);
            ShowSupermarketView();
            OnClose?.Invoke();
        }

        private void Login()
        {
            _userService.Login(loginPayload);
            ShowSupermarketView();
            OnClose?.Invoke();
        }

        private void ShowSupermarketView()
        {
            var supermarketView = new SupermarketView();
            supermarketView.DataContext = this;
            supermarketView.Show();
        }
    }
}