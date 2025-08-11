using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BookshopWpf.Data;
using BookshopWpf.Models;
using BookshopWpf.Services;
using BookshopWpf.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BookshopWpf
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private BookManagementView? _bookManagementView;
        private SalesView? _salesView;
        private SalesReportView? _salesReportView;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            LoadBookManagementView();
        }

        private void BookManagementMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadBookManagementView();
        }

        private void SalesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadSalesView();
        }

        private void SalesReportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadSalesReportView();
        }

        private void LoadBookManagementView()
        {
            if (_bookManagementView == null)
            {
                var bookService = _serviceProvider.GetRequiredService<IBookService>();
                _bookManagementView = new BookManagementView(bookService);
            }

            ContentArea.Content = _bookManagementView;
            Title = "Bookshop Management System - Book Management";
        }

        private void LoadSalesView()
        {
            if (_salesView == null)
            {
                var bookService = _serviceProvider.GetRequiredService<IBookService>();
                _salesView = new SalesView(bookService);
            }
            else
            {
                _salesView.RefreshView();
            }

            ContentArea.Content = _salesView;
            Title = "Bookshop Management System - Sales";
        }

        private void LoadSalesReportView()
        {
            if (_salesReportView == null)
            {
                var bookService = _serviceProvider.GetRequiredService<IBookService>();
                _salesReportView = new SalesReportView(bookService);
            }
            else
            {
                _salesReportView.RefreshReport();
            }

            ContentArea.Content = _salesReportView;
            Title = "Bookshop Management System - Sales Report";
        }

        private async void InitDbMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "This will initialize the database and create sample data. Continue?",
                "Initialize Database",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var dbContext = _serviceProvider.GetRequiredService<BookshopDbContext>();
                    await dbContext.Database.EnsureCreatedAsync();

                    if (!dbContext.Books.Any())
                    {
                        await AddSampleData(dbContext);
                    }

                    MessageBox.Show(
                        "Database initialized successfully!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    _bookManagementView = null;
                    _salesView = null;
                    _salesReportView = null;
                    LoadBookManagementView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error initializing database: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private async Task AddSampleData(BookshopDbContext dbContext)
        {
            var sampleBooks = new[]
            {
                new Book
                {
                    Id = Guid.Parse("9e5d8d6b-3e1a-4d82-84f1-f5b86e7a95c4"),
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0547928227",
                    Price = 9.99,
                    StockQuantity = 1,
                },
                new Book
                {
                    Id = Guid.Parse("5a3df3e4-bcc4-4b17-a0b6-4c1c98c3f4f7"),
                    Title = "1984",
                    Author = "George Orwell",
                    Isbn = "978-0451524935",
                    Price = 7.99,
                    StockQuantity = 2,
                },
                new Book
                {
                    Id = Guid.Parse("b31f1f26-1420-4c54-a25f-d344bb8a7e23"),
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Isbn = "978-0061120084",
                    Price = 8.49,
                    StockQuantity = 15,
                },
                new Book
                {
                    Id = Guid.Parse("73ff9fd9-4f2e-4d7c-a67e-56a13f3b5876"),
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    Isbn = "978-1503290563",
                    Price = 6.99,
                    StockQuantity = 14,
                },
                new Book
                {
                    Id = Guid.Parse("f0cb6c6d-bb65-4a88-9d3c-54f3a58dd8bb"),
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Isbn = "978-0743273565",
                    Price = 7.49,
                    StockQuantity = 13,
                },
                new Book
                {
                    Id = Guid.Parse("e23e9eaf-4cb5-43a6-a7a4-08ffb6432781"),
                    Title = "Moby-Dick",
                    Author = "Herman Melville",
                    Isbn = "978-1503280786",
                    Price = 10.99,
                    StockQuantity = 12,
                },
                new Book
                {
                    Id = Guid.Parse("c7d0f9c6-9c71-4370-bf54-93b47c987edf"),
                    Title = "War and Peace",
                    Author = "Leo Tolstoy",
                    Isbn = "978-1400079988",
                    Price = 12.99,
                    StockQuantity = 20,
                },
                new Book
                {
                    Id = Guid.Parse("6b1894a2-71cb-4a85-815b-c245b53c7923"),
                    Title = "Crime and Punishment",
                    Author = "Fyodor Dostoevsky",
                    Isbn = "978-0486415871",
                    Price = 11.49,
                    StockQuantity = 50,
                },
                new Book
                {
                    Id = Guid.Parse("d8b42b6a-5bc5-4e9b-ae0c-bdcd02c6c8b2"),
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    Isbn = "978-0316769488",
                    Price = 8.99,
                    StockQuantity = 5,
                },
                new Book
                {
                    Id = Guid.Parse("4453c2c5-f2df-46c0-b3d4-482f7b37602e"),
                    Title = "Brave New World",
                    Author = "Aldous Huxley",
                    Isbn = "978-0060850524",
                    Price = 7.89,
                    StockQuantity = 100,
                },
            };

            dbContext.Books.AddRange(sampleBooks);
            await dbContext.SaveChangesAsync();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Bookshop Management System\n\nVersion 1.0\n\nA simple WPF application for managing book inventory and sales.\n\nFeatures:\n• Book Management (Add, Edit, Delete)\n• Sales Processing\n• Sales Reporting\n\nBuilt with C# WPF, Entity Framework Core, and PostgreSQL.",
                "About Bookshop Management System",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
}
