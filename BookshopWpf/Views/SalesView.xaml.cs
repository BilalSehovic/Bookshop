using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BookshopWpf.Models;
using BookshopWpf.Services;

namespace BookshopWpf.Views
{
    public partial class SalesView : UserControl
    {
        private readonly IBookService _bookService;
        private Book? _selectedBook;

        public SalesView(IBookService bookService)
        {
            InitializeComponent();
            _bookService = bookService;
            LoadAvailableBooks();
        }

        private async void LoadAvailableBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                var availableBooks = books.Where(b => b.StockQuantity > 0).ToList();
                AvailableBooksDataGrid.ItemsSource = availableBooks;

                if (availableBooks.Count == 0)
                {
                    StatusTextBlock.Text = "No books available for sale.";
                }
                else
                {
                    StatusTextBlock.Text = $"{availableBooks.Count} books available for sale.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading books: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                StatusTextBlock.Text = "Error loading books.";
            }
        }

        private void AvailableBooksDataGrid_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e
        )
        {
            if (AvailableBooksDataGrid.SelectedItem is Book selectedBook)
            {
                _selectedBook = selectedBook;
                SelectedBookTextBlock.Text = $"{selectedBook.Title} by {selectedBook.Author}";
                SalePriceTextBox.Text = selectedBook.Price.ToString("F2");
                QuantityTextBox.Text = "1";
                SellBookButton.IsEnabled = true;
                StatusTextBlock.Text =
                    $"Ready to sell: {selectedBook.Title} (Stock: {selectedBook.StockQuantity})";
            }
            else
            {
                _selectedBook = null;
                SelectedBookTextBlock.Text = "No book selected";
                SalePriceTextBox.Clear();
                QuantityTextBox.Text = "1";
                SellBookButton.IsEnabled = false;
                StatusTextBlock.Text = "Select a book to sell.";
            }
        }

        private async void SellBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBook == null)
            {
                MessageBox.Show(
                    "Please select a book to sell.",
                    "No Selection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (!double.TryParse(SalePriceTextBox.Text, out double salePrice) || salePrice <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid sale price.",
                    "Invalid Price",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid quantity (must be 1 or greater).",
                    "Invalid Quantity",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (quantity > _selectedBook.StockQuantity)
            {
                MessageBox.Show(
                    $"Cannot sell {quantity} books. Only {_selectedBook.StockQuantity} in stock.",
                    "Insufficient Stock",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            var totalPrice = salePrice * quantity;
            var result = MessageBox.Show(
                $"Confirm sale of:\n\nBook: {_selectedBook.Title}\nAuthor: {_selectedBook.Author}\nQuantity: {quantity}\nPrice per book: {salePrice:C}\nTotal Price: {totalPrice:C}\n\nThis will reduce stock by {quantity}.",
                "Confirm Sale",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    SellBookButton.IsEnabled = false;
                    StatusTextBlock.Text = "Processing sale...";

                    bool success = await _bookService.SellBookAsync(
                        _selectedBook.Id,
                        salePrice,
                        quantity
                    );

                    if (success)
                    {
                        var remainingStock = _selectedBook.StockQuantity - quantity;
                        MessageBox.Show(
                            $"Book sold successfully!\n\nTitle: {_selectedBook.Title}\nQuantity Sold: {quantity}\nPrice per book: {salePrice:C}\nTotal Sale: {totalPrice:C}\nRemaining Stock: {remainingStock}",
                            "Sale Complete",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );

                        ClearSelection();
                        LoadAvailableBooks();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Sale failed. The book may be out of stock or no longer available.",
                            "Sale Failed",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                        LoadAvailableBooks();
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
                    SellBookButton.IsEnabled = true;
                    StatusTextBlock.Text = "Sale failed.";
                }
            }
        }

        private void ClearSelection()
        {
            _selectedBook = null;
            SelectedBookTextBlock.Text = "No book selected";
            SalePriceTextBox.Clear();
            QuantityTextBox.Text = "1";
            SellBookButton.IsEnabled = false;
            AvailableBooksDataGrid.SelectedItem = null;
        }

        public void RefreshView()
        {
            ClearSelection();
            LoadAvailableBooks();
        }
    }
}
