using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BookshopWpf.Models;
using BookshopWpf.Services;

namespace BookshopWpf.Views
{
    public partial class SalesReportView : UserControl
    {
        private readonly IBookService _bookService;

        public SalesReportView(IBookService bookService)
        {
            InitializeComponent();
            _bookService = bookService;

            ReportDatePicker.SelectedDate = DateTime.Today;
            SelectedDateTextBlock.Text = DateTime.Today.ToString("yyyy-MM-dd");
            LoadReport();
        }

        private async void LoadReport()
        {
            if (ReportDatePicker.SelectedDate == null)
                return;

            try
            {
                var selectedDate = ReportDatePicker.SelectedDate.Value;
                var sales = await _bookService.GetSalesByDateAsync(selectedDate);

                SalesReportDataGrid.ItemsSource = sales;
                UpdateSummary(sales, selectedDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading sales report: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                ClearReport();
            }
        }

        private void UpdateSummary(List<Sale> sales, DateTime selectedDate)
        {
            var totalSales = sales.Count;
            var booksSold = sales.Sum(s => s.Quantity);
            var totalRevenue = sales.Sum(s => s.UnitPrice * s.Quantity);

            TotalSalesTextBlock.Text = totalSales.ToString();
            BooksSoldTextBlock.Text = booksSold.ToString();
            TotalRevenueTextBlock.Text = totalRevenue.ToString("C");
            SelectedDateTextBlock.Text = selectedDate.ToString("yyyy-MM-dd");
        }

        private void ClearReport()
        {
            SalesReportDataGrid.ItemsSource = null;
            TotalSalesTextBlock.Text = "0";
            BooksSoldTextBlock.Text = "0";
            TotalRevenueTextBlock.Text = "$0.00";
        }

        private void ReportDatePicker_SelectedDateChanged(
            object sender,
            SelectionChangedEventArgs e
        )
        {
            if (ReportDatePicker.SelectedDate.HasValue)
            {
                SelectedDateTextBlock.Text = ReportDatePicker.SelectedDate.Value.ToString(
                    "yyyy-MM-dd"
                );
            }
        }

        private void LoadReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReportDatePicker.SelectedDate == null)
            {
                MessageBox.Show(
                    "Please select a date first.",
                    "No Date Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            LoadReport();
        }

        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            ReportDatePicker.SelectedDate = DateTime.Today;
            LoadReport();
        }

        public void RefreshReport()
        {
            LoadReport();
        }
    }
}
