using System.Collections.ObjectModel;
using System.Windows.Input;
using DataAccessLayer.Models;
using WpfApp.Services;

namespace WpfApp.ViewModels;

public class BookManagementViewModel : ViewModelBase
{
    private readonly IBookService _bookService;
    private readonly IDialogService _dialogService;
    private ObservableCollection<Book> _books = new();
    private Book? _selectedBook;
    private string _title = "";
    private string _author = "";
    private string _isbn = "";
    private string _price = "";
    private string _stock = "";
    private bool _isUpdateEnabled = false;
    private bool _isDeleteEnabled = false;

    public BookManagementViewModel(IBookService bookService, IDialogService dialogService)
    {
        _bookService = bookService;
        _dialogService = dialogService;
        AddCommand = new RelayCommand(async () => await AddBookAsync());
        UpdateCommand = new RelayCommand(
            async () => await UpdateBookAsync(),
            () => IsUpdateEnabled
        );
        DeleteCommand = new RelayCommand(
            async () => await DeleteBookAsync(),
            () => IsDeleteEnabled
        );
        ClearCommand = new RelayCommand(ClearForm);

        LoadBooksAsync();
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
                if (value != null)
                {
                    PopulateForm(value);
                    IsUpdateEnabled = true;
                    IsDeleteEnabled = true;
                }
                else
                {
                    IsUpdateEnabled = false;
                    IsDeleteEnabled = false;
                }
            }
        }
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Author
    {
        get => _author;
        set => SetProperty(ref _author, value);
    }

    public string Isbn
    {
        get => _isbn;
        set => SetProperty(ref _isbn, value);
    }

    public string Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    }

    public string Stock
    {
        get => _stock;
        set => SetProperty(ref _stock, value);
    }

    public bool IsUpdateEnabled
    {
        get => _isUpdateEnabled;
        set
        {
            if (SetProperty(ref _isUpdateEnabled, value))
            {
                ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public bool IsDeleteEnabled
    {
        get => _isDeleteEnabled;
        set
        {
            if (SetProperty(ref _isDeleteEnabled, value))
            {
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand AddCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ClearCommand { get; }

    private async Task LoadBooksAsync()
    {
        try
        {
            var books = await _bookService.GetAllBooksAsync();

            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                $"Error loading books: {ex.Message}",
                "Error"
            );
        }
    }

    private void PopulateForm(Book book)
    {
        Title = book.Title;
        Author = book.Author;
        Isbn = book.Isbn;
        Price = book.Price.ToString("F2");
        Stock = book.StockQuantity.ToString();
    }

    private void ClearForm()
    {
        Title = "";
        Author = "";
        Isbn = "";
        Price = "";
        Stock = "";
        SelectedBook = null;
        IsUpdateEnabled = false;
        IsDeleteEnabled = false;
    }

    private async Task AddBookAsync()
    {
        if (!await ValidateFormAsync())
            return;

        try
        {
            var book = new Book
            {
                Title = Title.Trim(),
                Author = Author.Trim(),
                Isbn = Isbn.Trim(),
                Price = double.Parse(Price),
                StockQuantity = int.Parse(Stock),
            };

            await _bookService.AddBookAsync(book);
            await _dialogService.ShowInformationAsync(
                "Book added successfully!",
                "Success"
            );
            ClearForm();
            await LoadBooksAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                $"Error adding book: {ex.Message}",
                "Error"
            );
        }
    }

    private async Task UpdateBookAsync()
    {
        if (SelectedBook == null || !await ValidateFormAsync())
            return;

        try
        {
            SelectedBook.Title = Title.Trim();
            SelectedBook.Author = Author.Trim();
            SelectedBook.Isbn = Isbn.Trim();
            SelectedBook.Price = double.Parse(Price);
            SelectedBook.StockQuantity = int.Parse(Stock);

            await _bookService.UpdateBookAsync(SelectedBook);
            await _dialogService.ShowInformationAsync(
                "Book updated successfully!",
                "Success"
            );
            ClearForm();
            await LoadBooksAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                $"Error updating book: {ex.Message}",
                "Error"
            );
        }
    }

    private async Task DeleteBookAsync()
    {
        if (SelectedBook == null)
            return;

        var result = await _dialogService.ShowConfirmationAsync(
            $"Are you sure you want to delete '{SelectedBook.Title}'?",
            "Confirm Delete"
        );

        if (result)
        {
            try
            {
                await _bookService.DeleteBookAsync(SelectedBook.Id);
                await _dialogService.ShowInformationAsync(
                    "Book deleted successfully!",
                    "Success"
                );
                ClearForm();
                await LoadBooksAsync();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync(
                    $"Error deleting book: {ex.Message}",
                    "Error"
                );
            }
        }
    }

    private async Task<bool> ValidateFormAsync()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            await _dialogService.ShowWarningAsync(
                "Title is required.",
                "Validation Error"
            );
            return false;
        }

        if (string.IsNullOrWhiteSpace(Author))
        {
            await _dialogService.ShowWarningAsync(
                "Author is required.",
                "Validation Error"
            );
            return false;
        }

        if (string.IsNullOrWhiteSpace(Isbn))
        {
            await _dialogService.ShowWarningAsync(
                "ISBN is required.",
                "Validation Error"
            );
            return false;
        }

        if (!double.TryParse(Price, out double price) || price <= 0)
        {
            await _dialogService.ShowWarningAsync(
                "Please enter a valid price.",
                "Validation Error"
            );
            return false;
        }

        if (!int.TryParse(Stock, out int stock) || stock < 0)
        {
            await _dialogService.ShowWarningAsync(
                "Please enter a valid stock quantity.",
                "Validation Error"
            );
            return false;
        }

        return true;
    }

    public void RefreshView()
    {
        LoadBooksAsync();
    }
}
