using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CustomersTestApp.Commands;
using CustomersTestApp.Messaging;
using CustomersTestApp.Models;
using CustomersTestApp.Services;
using CustomerTestApp.Services;
using GrpcCustomer = CustomerTestApp.Customer; // Alias for gRPC generated Customer
using LocalCustomer = CustomersTestApp.Models.Customer; // Alias for Local Customer

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

        public ICommand AddCustomerCommand { get; }
        public ICommand RemoveCustomerCommand { get; }
        public ICommand SaveCommand { get; }

        public string Description => "List of customers with filter";

        public MainViewModel()
        {
            _grpcCustomerService = new GrpcCustomerService();
            _allCustomers = new ObservableCollection<CustomerViewModel>();
            Customers = new ObservableCollection<CustomerViewModel>();
            AddCustomerCommand = new RelayCommand(async () => await AddCustomer(), () => CanAddCustomer);
            RemoveCustomerCommand = new RelayCommand(async () => await RemoveCustomer(), CanRemoveCustomer);
            SaveCommand = new RelayCommand(async () => await SaveCustomer(), CanSaveCustomer);

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

            // Reset fields
            NewCustomerName = string.Empty;
            NewCustomerEmail = string.Empty;
            NewCustomerDiscount = string.Empty;
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
                var updatedCustomer = new LocalCustomer
                {
                    Id = EditableCustomer.Id,
                    Name = EditableCustomer.Name,
                    Email = EditableCustomer.Email,
                    Discount = EditableCustomer.Discount,
                    Can_Remove = EditableCustomer.Can_Remove
                };

                var updatedCustomerResult = await _grpcCustomerService.UpdateCustomerAsync(updatedCustomer);

                SelectedCustomer.Name = updatedCustomerResult.Name;
                SelectedCustomer.Email = updatedCustomerResult.Email;
                SelectedCustomer.Discount = updatedCustomerResult.Discount;
                ApplyFilter();
                Messenger.Instance.Send(new CustomerUpdatedMessage(SelectedCustomer));
            }
        }

        private bool CanSaveCustomer()
        {
            return EditableCustomer != null && EditableCustomer.CanSave;
        }

        private void ValidateAddCustomer()
        {
            if (string.IsNullOrWhiteSpace(NewCustomerName) ||
                string.IsNullOrWhiteSpace(NewCustomerEmail) ||
                !new EmailAddressAttribute().IsValid(NewCustomerEmail) ||
                !int.TryParse(NewCustomerDiscount, out int discount) ||
                discount < 0 || discount > 30)
            {
                CanAddCustomer = false;
            }
            else
            {
                CanAddCustomer = true;
            }
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

        private void OnCustomerRemoved(CustomerRemovedMessage message)
        {
            // Handle customer removal logic if necessary
        }

        private void OnCustomerAdded(CustomerAddedMessage message)
        {
            // Handle customer addition logic if necessary
        }
    }
}
