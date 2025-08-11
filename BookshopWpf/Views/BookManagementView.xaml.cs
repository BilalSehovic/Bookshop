using System;
using System.Windows;
using System.Windows.Controls;
using BookshopWpf.Models;
using BookshopWpf.Services;

namespace BookshopWpf.Views
{
    public partial class BookManagementView : UserControl
    {
        private readonly IBookService _bookService;
        private Book? _selectedBook;

        public BookManagementView(IBookService bookService)
        {
            InitializeComponent();
            _bookService = bookService;
            LoadBooks();
        }

        private async void LoadBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                BooksDataGrid.ItemsSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading books: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                _selectedBook = selectedBook;
                PopulateForm(selectedBook);
                UpdateButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
            }
            else
            {
                _selectedBook = null;
                UpdateButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
        }

        private void PopulateForm(Book book)
        {
            TitleTextBox.Text = book.Title;
            AuthorTextBox.Text = book.Author;
            ISBNTextBox.Text = book.Isbn;
            PriceTextBox.Text = book.Price.ToString("F2");
            StockTextBox.Text = book.StockQuantity.ToString();
        }

        private void ClearForm()
        {
            TitleTextBox.Clear();
            AuthorTextBox.Clear();
            ISBNTextBox.Clear();
            PriceTextBox.Clear();
            StockTextBox.Clear();
            _selectedBook = null;
            UpdateButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            BooksDataGrid.SelectedItem = null;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var book = new Book
                {
                    Title = TitleTextBox.Text.Trim(),
                    Author = AuthorTextBox.Text.Trim(),
                    Isbn = ISBNTextBox.Text.Trim(),
                    Price = double.Parse(PriceTextBox.Text),
                    StockQuantity = int.Parse(StockTextBox.Text),
                };

                await _bookService.AddBookAsync(book);
                MessageBox.Show(
                    "Book added successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                ClearForm();
                LoadBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error adding book: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBook == null || !ValidateForm())
                return;

            try
            {
                _selectedBook.Title = TitleTextBox.Text.Trim();
                _selectedBook.Author = AuthorTextBox.Text.Trim();
                _selectedBook.Isbn = ISBNTextBox.Text.Trim();
                _selectedBook.Price = double.Parse(PriceTextBox.Text);
                _selectedBook.StockQuantity = int.Parse(StockTextBox.Text);

                await _bookService.UpdateBookAsync(_selectedBook);
                MessageBox.Show(
                    "Book updated successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                ClearForm();
                LoadBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error updating book: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBook == null)
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete '{_selectedBook.Title}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _bookService.DeleteBookAsync(_selectedBook.Id);
                    MessageBox.Show(
                        "Book deleted successfully!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    ClearForm();
                    LoadBooks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error deleting book: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show(
                    "Title is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            if (string.IsNullOrWhiteSpace(AuthorTextBox.Text))
            {
                MessageBox.Show(
                    "Author is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            if (string.IsNullOrWhiteSpace(ISBNTextBox.Text))
            {
                MessageBox.Show(
                    "ISBN is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid price.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            if (!int.TryParse(StockTextBox.Text, out int stock) || stock < 0)
            {
                MessageBox.Show(
                    "Please enter a valid stock quantity.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            return true;
        }
    }
}
