using System.Collections.ObjectModel;
using System.Windows.Input;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using WpfApp.Commands;

namespace WpfApp.ViewModels;

public class SalesViewModel : ViewModelBase
{
    private readonly IBookRepository _bookRepository;
    private readonly ISaleRepository _saleRepository;
    private ObservableCollection<Book> _availableBooks = new();
    private ObservableCollection<SaleItem> _saleItems = new();
    private Book? _selectedBook;
    private int _quantity = 1;
    private string _customerName = string.Empty;
    private decimal _totalAmount;

    public SalesViewModel(IBookRepository bookRepository, ISaleRepository saleRepository)
    {
        _bookRepository = bookRepository;
        _saleRepository = saleRepository;

        LoadBooksCommand = new RelayCommand(async () => await LoadBooks());
        AddToSaleCommand = new RelayCommand(AddToSale, () => SelectedBook != null && Quantity > 0);
        RemoveFromSaleCommand = new RelayCommand<SaleItem>(RemoveFromSale);
        CompleteSaleCommand = new RelayCommand(
            async () => await CompleteSale(),
            () => SaleItems.Any()
        );
        ClearSaleCommand = new RelayCommand(ClearSale);
    }

    public ObservableCollection<Book> AvailableBooks
    {
        get => _availableBooks;
        set => SetProperty(ref _availableBooks, value);
    }

    public ObservableCollection<SaleItem> SaleItems
    {
        get => _saleItems;
        set
        {
            SetProperty(ref _saleItems, value);
            CalculateTotal();
        }
    }

    public Book? SelectedBook
    {
        get => _selectedBook;
        set => SetProperty(ref _selectedBook, value);
    }

    public int Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value);
    }

    public string CustomerName
    {
        get => _customerName;
        set => SetProperty(ref _customerName, value);
    }

    public decimal TotalAmount
    {
        get => _totalAmount;
        set => SetProperty(ref _totalAmount, value);
    }

    public ICommand LoadBooksCommand { get; }
    public ICommand AddToSaleCommand { get; }
    public ICommand RemoveFromSaleCommand { get; }
    public ICommand CompleteSaleCommand { get; }
    public ICommand ClearSaleCommand { get; }

    private async Task LoadBooks()
    {
        var books = await _bookRepository.GetAllAsync();
        AvailableBooks = new ObservableCollection<Book>(books.Where(b => b.StockQuantity > 0));
    }

    private void AddToSale()
    {
        if (SelectedBook == null || Quantity <= 0 || Quantity > SelectedBook.StockQuantity)
            return;

        var existingItem = SaleItems.FirstOrDefault(si => si.Book.Id == SelectedBook.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += Quantity;
            existingItem.TotalPrice = existingItem.Quantity * (decimal)existingItem.Book.Price;
        }
        else
        {
            var saleItem = new SaleItem
            {
                Book = SelectedBook,
                Quantity = Quantity,
                UnitPrice = (decimal)SelectedBook.Price,
                TotalPrice = Quantity * (decimal)SelectedBook.Price,
            };
            SaleItems.Add(saleItem);
        }

        CalculateTotal();
        Quantity = 1;
    }

    private void RemoveFromSale(SaleItem? saleItem)
    {
        if (saleItem != null)
        {
            SaleItems.Remove(saleItem);
            CalculateTotal();
        }
    }

    private async Task CompleteSale()
    {
        foreach (var saleItem in SaleItems)
        {
            var sale = new Sale
            {
                BookId = saleItem.Book.Id,
                Book = saleItem.Book,
                Quantity = saleItem.Quantity,
                UnitPrice = (double)saleItem.UnitPrice,
                TotalPrice = (double)saleItem.TotalPrice,
                SaleDate = DateTime.Now,
                CustomerName = string.IsNullOrWhiteSpace(CustomerName) ? null : CustomerName,
            };

            await _saleRepository.AddAsync(sale);

            // Update stock
            saleItem.Book.StockQuantity -= saleItem.Quantity;
            await _bookRepository.UpdateAsync(saleItem.Book);
        }

        ClearSale();
        await LoadBooks(); // Refresh available books
    }

    private void ClearSale()
    {
        SaleItems.Clear();
        CustomerName = string.Empty;
        TotalAmount = 0;
    }

    private void CalculateTotal()
    {
        TotalAmount = SaleItems.Sum(si => si.TotalPrice);
    }
}

public class SaleItem
{
    public Book Book { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
