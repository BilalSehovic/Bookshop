using System.Collections.ObjectModel;
using System.Windows.Input;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using WpfApp.Commands;

namespace WpfApp.ViewModels;

public class BookManagementViewModel : ViewModelBase
{
    private readonly IBookRepository _bookRepository;
    private ObservableCollection<Book> _books = new();
    private Book? _selectedBook;
    private Book _editingBook = new();
    private bool _isEditing;

    public BookManagementViewModel(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;

        LoadBooksCommand = new RelayCommand(async () => await LoadBooks());
        AddBookCommand = new RelayCommand(AddBook, () => !IsEditing);
        EditBookCommand = new RelayCommand(EditBook, () => SelectedBook != null && !IsEditing);
        SaveBookCommand = new RelayCommand(async () => await SaveBook(), () => IsEditing);
        CancelEditCommand = new RelayCommand(CancelEdit, () => IsEditing);
        DeleteBookCommand = new RelayCommand(
            async () => await DeleteBook(),
            () => SelectedBook != null && !IsEditing
        );
    }

    public ObservableCollection<Book> Books
    {
        get => _books;
        set => SetProperty(ref _books, value);
    }

    public Book? SelectedBook
    {
        get => _selectedBook;
        set => SetProperty(ref _selectedBook, value);
    }

    public Book EditingBook
    {
        get => _editingBook;
        set => SetProperty(ref _editingBook, value);
    }

    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public ICommand LoadBooksCommand { get; }
    public ICommand AddBookCommand { get; }
    public ICommand EditBookCommand { get; }
    public ICommand SaveBookCommand { get; }
    public ICommand CancelEditCommand { get; }
    public ICommand DeleteBookCommand { get; }

    private async Task LoadBooks()
    {
        var books = await _bookRepository.GetAllAsync();
        Books = new ObservableCollection<Book>(books);
    }

    private void AddBook()
    {
        EditingBook = new Book();
        IsEditing = true;
    }

    private void EditBook()
    {
        if (SelectedBook == null)
            return;

        EditingBook = new Book
        {
            Id = SelectedBook.Id,
            Title = SelectedBook.Title,
            Author = SelectedBook.Author,
            Isbn = SelectedBook.Isbn,
            Price = SelectedBook.Price,
            StockQuantity = SelectedBook.StockQuantity,
        };
        IsEditing = true;
    }

    private async Task SaveBook()
    {
        if (EditingBook.Id == Guid.Empty)
        {
            await _bookRepository.AddAsync(EditingBook);
        }
        else
        {
            await _bookRepository.UpdateAsync(EditingBook);
        }

        await LoadBooks();
        CancelEdit();
    }

    private void CancelEdit()
    {
        EditingBook = new Book();
        IsEditing = false;
    }

    private async Task DeleteBook()
    {
        if (SelectedBook == null)
            return;

        await _bookRepository.DeleteAsync(SelectedBook);
        await LoadBooks();
    }
}
