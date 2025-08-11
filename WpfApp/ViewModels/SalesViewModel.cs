using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DataAccessLayer.Models;
using WpfApp.Services;

namespace WpfApp.ViewModels;

public class SalesViewModel : ViewModelBase
{
    private readonly IBookService _bookService;
    private ObservableCollection<Book> _books = new();
    private Book? _selectedBook;
    private string _selectedBookText = "No book selected";
    private string _salePriceText = "";
    private string _quantityText = "1";
    private bool _isSellButtonEnabled = false;
    private string _statusText = "Select a book to sell.";

    public SalesViewModel(IBookService bookService)
    {
        _bookService = bookService;
        SellBookCommand = new RelayCommand(
            async () => await SellBookAsync(),
            () => IsSellButtonEnabled
        );
        LoadBooksCommand = new RelayCommand(async () => await LoadAvailableBooksAsync());

        Task.Run(async () => await LoadAvailableBooksAsync());
    }

    public ObservableCollection<Book> Books
    {
        get => _books;
        set => SetProperty(ref _books, value);
    }

    public Book? SelectedBook
    {
        get => _selectedBook;
        set
        {
            if (SetProperty(ref _selectedBook, value))
            {
                UpdateSelectedBookInfo();
            }
        }
    }

    public string SelectedBookText
    {
        get => _selectedBookText;
        set => SetProperty(ref _selectedBookText, value);
    }

    public string SalePriceText
    {
        get => _salePriceText;
        set => SetProperty(ref _salePriceText, value);
    }

    public string QuantityText
    {
        get => _quantityText;
        set => SetProperty(ref _quantityText, value);
    }

    public bool IsSellButtonEnabled
    {
        get => _isSellButtonEnabled;
        set
        {
            if (SetProperty(ref _isSellButtonEnabled, value))
            {
                ((RelayCommand)SellBookCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }

    public ICommand SellBookCommand { get; }
    public ICommand LoadBooksCommand { get; }

    private async Task LoadAvailableBooksAsync()
    {
        try
        {
            var books = await _bookService.GetAllBooksAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Books.Clear();
                foreach (var book in books.OrderBy(b => b.Title))
                {
                    Books.Add(book);
                }

                var inStockCount = books.Count(b => b.StockQuantity > 0);
                var outOfStockCount = books.Count(b => b.StockQuantity == 0);

                StatusText =
                    $"{books.Count} books total ({inStockCount} in stock, {outOfStockCount} out of stock).";

                if (books.Count == 0)
                    StatusText = "No books in inventory.";
            });
        }
        catch (Exception ex)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    $"Error loading books: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                StatusText = "Error loading books.";
            });
        }
    }

    private void UpdateSelectedBookInfo()
    {
        if (SelectedBook != null)
        {
            SelectedBookText = $"{SelectedBook.Title} by {SelectedBook.Author}";
            SalePriceText = SelectedBook.Price.ToString("F2");
            QuantityText = "1";

            if (SelectedBook.StockQuantity > 0)
            {
                IsSellButtonEnabled = true;
                StatusText =
                    $"Ready to sell: {SelectedBook.Title} (Stock: {SelectedBook.StockQuantity})";
            }
            else
            {
                IsSellButtonEnabled = false;
                StatusText = $"Selected: {SelectedBook.Title} (OUT OF STOCK)";
            }
        }
        else
        {
            ClearSelection();
        }
    }

    private async Task SellBookAsync()
    {
        if (SelectedBook == null)
        {
            MessageBox.Show(
                "Please select a book to sell.",
                "No Selection",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
            return;
        }

        if (!double.TryParse(SalePriceText, out double salePrice) || salePrice <= 0)
        {
            MessageBox.Show(
                "Please enter a valid sale price.",
                "Invalid Price",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
            return;
        }

        if (!int.TryParse(QuantityText, out int quantity) || quantity <= 0)
        {
            MessageBox.Show(
                "Please enter a valid quantity (must be 1 or greater).",
                "Invalid Quantity",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
            return;
        }

        if (quantity > SelectedBook.StockQuantity)
        {
            MessageBox.Show(
                $"Cannot sell {quantity} books. Only {SelectedBook.StockQuantity} in stock.",
                "Insufficient Stock",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
            return;
        }

        var totalPrice = salePrice * quantity;
        var result = MessageBox.Show(
            $"Confirm sale of:\n\nBook: {SelectedBook.Title}\nAuthor: {SelectedBook.Author}\nQuantity: {quantity}\nPrice per book: {salePrice:C}\nTotal Price: {totalPrice:C}\n\nThis will reduce stock by {quantity}.",
            "Confirm Sale",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question
        );

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                IsSellButtonEnabled = false;
                StatusText = "Processing sale...";

                bool success = await _bookService.SellBookAsync(
                    SelectedBook.Id,
                    salePrice,
                    quantity
                );

                if (success)
                {
                    var remainingStock = SelectedBook.StockQuantity - quantity;
                    MessageBox.Show(
                        $"Book sold successfully!\n\nTitle: {SelectedBook.Title}\nQuantity Sold: {quantity}\nPrice per book: {salePrice:C}\nTotal Sale: {totalPrice:C}\nRemaining Stock: {remainingStock}",
                        "Sale Complete",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    ClearSelection();
                    await LoadAvailableBooksAsync();
                }
                else
                {
                    MessageBox.Show(
                        "Sale failed. The book may be out of stock or no longer available.",
                        "Sale Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    await LoadAvailableBooksAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error processing sale: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                IsSellButtonEnabled = true;
                StatusText = "Sale failed.";
            }
        }
        else
        {
            IsSellButtonEnabled = true;
            StatusText =
                $"Ready to sell: {SelectedBook.Title} (Stock: {SelectedBook.StockQuantity})";
        }
    }

    private void ClearSelection()
    {
        SelectedBook = null;
        SelectedBookText = "No book selected";
        SalePriceText = "";
        QuantityText = "1";
        IsSellButtonEnabled = false;
        StatusText = "Select a book to sell.";
    }

    public void RefreshView()
    {
        ClearSelection();
        Task.Run(async () => await LoadAvailableBooksAsync());
    }
}
