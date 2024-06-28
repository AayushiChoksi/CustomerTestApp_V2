using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CustomersTestApp.Models;
using CustomersTestApp;

namespace CustomersTestApp.Services
{
    public interface ICustomerService
    {
        Task<ObservableCollection<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerAsync(string id);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string id);
        Task<ObservableCollection<Customer>> FilterCustomersAsync(string name, string email);
    }
}
