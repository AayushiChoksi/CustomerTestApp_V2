using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CustomersTestApp.Commands;
using CustomersTestApp.Messaging;
using CustomersTestApp.Models;
using CustomersTestApp.Services;
using CustomerTestApp.Services;
using GrpcCustomer = CustomerTestApp.Customer; // Alias for gRPC generated Customer
using LocalCustomer = CustomersTestApp.Models.Customer;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using CsvHelper;
using ClosedXML.Excel;
using Microsoft.Win32;
using System.IO;
using System.Globalization;

namespace CustomersTestApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly GrpcCustomerService _grpcCustomerService;
        private ObservableCollection<CustomerViewModel> _allCustomers;
        private ObservableCollection<CustomerViewModel> _customers;
        private CustomerViewModel _selectedCustomer;
        private CustomerViewModel _editableCustomer;
        private string _filterText;
        private string _selectedFilterOption;

        // Properties for new customer
        private string _newCustomerName;
        private string _newCustomerEmail;
        private string _newCustomerDiscount;
        private bool _canAddCustomer;
        private string _nameError;
        private string _emailError;
        private string _discountError;

        public ObservableCollection<CustomerViewModel> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> FilterOptions { get; } = new ObservableCollection<string> { "Name", "Email" };

        public CustomerViewModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
                if (_selectedCustomer != null)
                {
                    EditableCustomer = new CustomerViewModel(new LocalCustomer(
                        _selectedCustomer.Id,
                        _selectedCustomer.Name,
                        _selectedCustomer.Email,
                        _selectedCustomer.Discount,
                        _selectedCustomer.Can_Remove));

                    EditableCustomer.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(EditableCustomer.CanSave))
                        {
                            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
                        }
                    };
                }
                else
                {
                    EditableCustomer = null;
                }
                ((RelayCommand)RemoveCustomerCommand).RaiseCanExecuteChanged();
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public CustomerViewModel EditableCustomer
        {
            get { return _editableCustomer; }
            set { _editableCustomer = value; OnPropertyChanged(); }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public string SelectedFilterOption
        {
            get => _selectedFilterOption;
            set
            {
                _selectedFilterOption = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public string NewCustomerName
        {
            get => _newCustomerName;
            set
            {
                _newCustomerName = value;
                OnPropertyChanged();
                ValidateAddCustomer();
            }
        }

        public string NewCustomerEmail
        {
            get => _newCustomerEmail;
            set
            {
                _newCustomerEmail = value;
                OnPropertyChanged();
                ValidateAddCustomer();
            }
        }

        public string NewCustomerDiscount
        {
            get => _newCustomerDiscount;
            set
            {
                _newCustomerDiscount = value;
                OnPropertyChanged();
                ValidateAddCustomer();
            }
        }

        public bool CanAddCustomer
        {
            get => _canAddCustomer;
            set
            {
                _canAddCustomer = value;
                OnPropertyChanged();
                ((RelayCommand)AddCustomerCommand).RaiseCanExecuteChanged();
            }
        }

        public string NameError
        {
            get => _nameError;
            set { _nameError = value; OnPropertyChanged(); }
        }

        public string EmailError
        {
            get => _emailError;
            set { _emailError = value; OnPropertyChanged(); }
        }

        public string DiscountError
        {
            get => _discountError;
            set { _discountError = value; OnPropertyChanged(); }
        }

        public ICommand AddCustomerCommand { get; }
        public ICommand RemoveCustomerCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand TestStreamingCommand { get; }
        public ICommand ExportCommand { get; }

        public string Description => "List of customers with filter";

        public MainViewModel()
        {
            _grpcCustomerService = new GrpcCustomerService();
            _allCustomers = new ObservableCollection<CustomerViewModel>();
            Customers = new ObservableCollection<CustomerViewModel>();
            AddCustomerCommand = new RelayCommand(async () => await AddCustomer(), () => CanAddCustomer);
            RemoveCustomerCommand = new RelayCommand(async () => await RemoveCustomer(), CanRemoveCustomer);
            SaveCommand = new RelayCommand(async () => await SaveCustomer(), CanSaveCustomer);
            TestStreamingCommand = new RelayCommand(async () => await TestStreamingMethods());
            ExportCommand = new RelayCommand(ExportData);

            LoadCustomers();
        }

        private async void LoadCustomers()
        {
            var customers = await _grpcCustomerService.GetAllCustomersAsync();
            foreach (var customer in customers)
            {
                _allCustomers.Add(new CustomerViewModel(customer));
            }
            ApplyFilter();
        }

        private async Task AddCustomer()
        {
            var newCustomer = new LocalCustomer
            {
                Name = NewCustomerName,
                Email = NewCustomerEmail,
                Discount = int.Parse(NewCustomerDiscount),
                Can_Remove = true
            };
            var addedCustomer = await _grpcCustomerService.AddCustomerAsync(newCustomer);
            var customerViewModel = new CustomerViewModel(addedCustomer);
            _allCustomers.Add(customerViewModel);
            ApplyFilter();
            Messenger.Instance.Send(new CustomerAddedMessage(customerViewModel));

            // Reset fields and errors
            NewCustomerName = string.Empty;
            NewCustomerEmail = string.Empty;
            NewCustomerDiscount = string.Empty;
            NameError = string.Empty;
            EmailError = string.Empty;
            DiscountError = string.Empty;
            CanAddCustomer = false;
        }

        private async Task RemoveCustomer()
        {
            if (SelectedCustomer != null && SelectedCustomer.Can_Remove)
            {
                var customerToRemove = SelectedCustomer;
                await _grpcCustomerService.DeleteCustomerAsync(customerToRemove.Id);
                _allCustomers.Remove(customerToRemove);
                ApplyFilter();
                Messenger.Instance.Send(new CustomerRemovedMessage(customerToRemove));
                SelectedCustomer = null;  // Clear the selection
                EditableCustomer = null;  // Clear the editable customer details
            }
        }

        private bool CanRemoveCustomer()
        {
            return SelectedCustomer != null && SelectedCustomer.Can_Remove;
        }

        private async Task SaveCustomer()
        {
            if (SelectedCustomer != null && EditableCustomer != null)
            {
                // Perform email validation
                var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (string.IsNullOrWhiteSpace(EditableCustomer.Email) || !Regex.IsMatch(EditableCustomer.Email, emailPattern))
                {
                    MessageBox.Show("Invalid email format. Please enter a valid email address.", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var updatedCustomer = new LocalCustomer
                {
                    Id = EditableCustomer.Id,
                    Name = EditableCustomer.Name,
                    Email = EditableCustomer.Email,
                    Discount = EditableCustomer.Discount,
                    Can_Remove = EditableCustomer.Can_Remove
                };

                try
                {
                    var updatedCustomerResult = await _grpcCustomerService.UpdateCustomerAsync(updatedCustomer);

                    SelectedCustomer.Name = updatedCustomerResult.Name;
                    SelectedCustomer.Email = updatedCustomerResult.Email;
                    SelectedCustomer.Discount = updatedCustomerResult.Discount;
                    ApplyFilter();
                    Messenger.Instance.Send(new CustomerUpdatedMessage(SelectedCustomer));

                    // Show success message
                    MessageBox.Show("Customer details saved successfully!", "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Show error message if something goes wrong
                    MessageBox.Show($"An error occurred while saving customer details: {ex.Message}", "Save Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private bool CanSaveCustomer()
        {
            return EditableCustomer != null && EditableCustomer.CanSave;
        }

        private void ValidateAddCustomer()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(NewCustomerName) || !NewCustomerName.All(char.IsLetter))
            {
                NameError = "Name must contain only letters and cannot be empty.";
                isValid = false;
            }
            else
            {
                NameError = string.Empty;
            }

            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (string.IsNullOrWhiteSpace(NewCustomerEmail) || !Regex.IsMatch(NewCustomerEmail, emailPattern))
            {
                EmailError = "Invalid email format.";
                isValid = false;
            }
            else
            {
                EmailError = string.Empty;
            }

            if (!int.TryParse(NewCustomerDiscount, out int discount) || discount < 0 || discount > 30)
            {
                DiscountError = "Discount must be between 0 and 30.";
                isValid = false;
            }
            else
            {
                DiscountError = string.Empty;
            }

            CanAddCustomer = isValid;
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                Customers = new ObservableCollection<CustomerViewModel>(_allCustomers);
            }
            else
            {
                switch (SelectedFilterOption)
                {
                    case "Name":
                        Customers = new ObservableCollection<CustomerViewModel>(_allCustomers.Where(c => c.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase)));
                        break;
                    case "Email":
                        Customers = new ObservableCollection<CustomerViewModel>(_allCustomers.Where(c => c.Email.Contains(FilterText, StringComparison.OrdinalIgnoreCase)));
                        break;
                    default:
                        Customers = new ObservableCollection<CustomerViewModel>(_allCustomers);
                        break;
                }
            }
        }

        private async Task TestStreamingMethods()
        {
            // Test server-side streaming
            var customersStream = await _grpcCustomerService.GetCustomersStreamAsync();
            _allCustomers.Clear();
            foreach (var customer in customersStream)
            {
                _allCustomers.Add(new CustomerViewModel(customer));
            }
            ApplyFilter();

            // Test client-side streaming
            var newCustomerRequests = new List<LocalCustomer>
            {
                new LocalCustomer { Name = "Test Customer 1", Email = "test1@example.com", Discount = 10, Can_Remove = true },
                new LocalCustomer { Name = "Test Customer 2", Email = "test2@example.com", Discount = 20, Can_Remove = true },
                // Add more test customers as needed
            };
            var newCustomersStream = await _grpcCustomerService.CreateCustomersStreamAsync(newCustomerRequests);
            foreach (var customer in newCustomersStream)
            {
                _allCustomers.Add(new CustomerViewModel(customer));
            }
            ApplyFilter();
        }

        private void OnCustomerRemoved(CustomerRemovedMessage message)
        {
            // Handle customer removal logic if necessary
        }

        private void OnCustomerAdded(CustomerAddedMessage message)
        {
            // Handle customer addition logic if necessary
        }

        private void ExportData()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx",
                Title = "Export Data"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                try
                {
                    if (filePath.EndsWith(".csv"))
                    {
                        using (var writer = new StreamWriter(filePath))
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csv.WriteRecords(_allCustomers.Select(c => new { c.Name, c.Email, c.Discount }));
                        }
                    }
                    else if (filePath.EndsWith(".xlsx"))
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Customers");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Name";
                            worksheet.Cell(currentRow, 2).Value = "Email";
                            worksheet.Cell(currentRow, 3).Value = "Discount";
                            foreach (var customer in _allCustomers)
                            {
                                currentRow++;
                                worksheet.Cell(currentRow, 1).Value = customer.Name;
                                worksheet.Cell(currentRow, 2).Value = customer.Email;
                                worksheet.Cell(currentRow, 3).Value = customer.Discount;
                            }
                            workbook.SaveAs(filePath);
                        }
                    }

                    // Show success message
                    MessageBox.Show("Data exported successfully!", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Show error message if something goes wrong
                    MessageBox.Show($"An error occurred while exporting data: {ex.Message}", "Export Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
