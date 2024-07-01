using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CustomersTestApp.Models
{
    public class Customer : IDataErrorInfo
    {
        public Customer()
        {
            Id = Guid.NewGuid().ToString(); // Generates a unique ID
        }

        public Customer(string id, string name, string email, int discount, bool canRemove)
        {
            Id = id;
            Name = name;
            Email = email;
            Discount = discount;
            Can_Remove = canRemove;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Discount { get; set; } // min = 0, max = 30
        public bool Can_Remove { get; set; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(Name))
                        {
                            result = "Name cannot be empty.";
                        }
                        break;
                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                        {
                            result = "Invalid email format.";
                        }
                        break;
                    case nameof(Discount):
                        if (Discount < 0 || Discount > 30)
                        {
                            result = "Discount must be between 0 and 30.";
                        }
                        break;
                }
                return result;
            }
        }
    }
}
