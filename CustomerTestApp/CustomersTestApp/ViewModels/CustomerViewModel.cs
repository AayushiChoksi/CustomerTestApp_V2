using CustomersTestApp.Commands;
using CustomersTestApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace CustomersTestApp.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        private Customer _customer;

        public CustomerViewModel(Customer customer)
        {
            _customer = customer;
            RemoveCommand = new RelayCommand(RemoveCustomer, () => Can_Remove);
            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Can_Remove))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            };
        }

        public string Id => _customer.Id;
        public string Name
        {
            get => _customer.Name;
            set
            {
                if (_customer.Name != value)
                {
                    _customer.Name = value;
                    OnPropertyChanged();
                    ValidateProperty(nameof(Name));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }
        public string Email
        {
            get => _customer.Email;
            set
            {
                if (_customer.Email != value)
                {
                    _customer.Email = value;
                    OnPropertyChanged();
                    ValidateProperty(nameof(Email));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }
        public int Discount
        {
            get => _customer.Discount;
            set
            {
                if (_customer.Discount != value)
                {
                    _customer.Discount = value;
                    OnPropertyChanged();
                    ValidateProperty(nameof(Discount));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }
        public bool Can_Remove
        {
            get => _customer.Can_Remove;
            set
            {
                if (_customer.Can_Remove != value)
                {
                    _customer.Can_Remove = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanSave => string.IsNullOrWhiteSpace(NameError) && string.IsNullOrWhiteSpace(EmailError) && string.IsNullOrWhiteSpace(DiscountError);

        public ICommand RemoveCommand { get; }

        private void RemoveCustomer()
        {
            // Implement the logic to remove the customer
        }

        private string _nameError;
        public string NameError
        {
            get => _nameError;
            set { _nameError = value; OnPropertyChanged(); }
        }

        private string _emailError;
        public string EmailError
        {
            get => _emailError;
            set { _emailError = value; OnPropertyChanged(); }
        }

        private string _discountError;
        public string DiscountError
        {
            get => _discountError;
            set { _discountError = value; OnPropertyChanged(); }
        }

        private void ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        NameError = "Name cannot be empty.";
                    }
                    else
                    {
                        NameError = string.Empty;
                    }
                    break;
                case nameof(Email):
                    if (string.IsNullOrWhiteSpace(Email) || !new EmailAddressAttribute().IsValid(Email))
                    {
                        EmailError = "Invalid email format.";
                    }
                    else
                    {
                        EmailError = string.Empty;
                    }
                    break;
                case nameof(Discount):
                    if (Discount < 0 || Discount > 30)
                    {
                        DiscountError = "Discount must be between 0 and 30.";
                    }
                    else
                    {
                        DiscountError = string.Empty;
                    }
                    break;
            }
        }
    }
}
