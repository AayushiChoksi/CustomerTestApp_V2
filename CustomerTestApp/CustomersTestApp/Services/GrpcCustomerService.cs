using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Core;
using CustomerTestApp;
using CustomersTestApp.Services;
using CustomersTestApp.Models;
using System.Collections.Generic;

namespace CustomerTestApp.Services
{
    public class GrpcCustomerService
    {
        private readonly CustomerService.CustomerServiceClient _client;
        private readonly Metadata _headers;

        public GrpcCustomerService()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5203");
            _client = new CustomerService.CustomerServiceClient(channel);
            _headers = new Metadata
            {
                { "app-name", "GrpcClient" }
            };
        }

        public async Task<ObservableCollection<CustomersTestApp.Models.Customer>> GetAllCustomersAsync()
        {
            var response = await _client.GetAllCustomersAsync(new Google.Protobuf.WellKnownTypes.Empty(), _headers);
            var customers = response.Customers.Select(c => CustomerMapper.ToLocalCustomer(c));

            return new ObservableCollection<CustomersTestApp.Models.Customer>(customers);
        }

        public async Task<CustomersTestApp.Models.Customer> GetCustomerAsync(string id)
        {
            var response = await _client.GetCustomerAsync(new CustomerRequest { Id = id }, _headers);
            return CustomerMapper.ToLocalCustomer(response.Customer);
        }

        public async Task<CustomersTestApp.Models.Customer> AddCustomerAsync(CustomersTestApp.Models.Customer customer)
        {
            var grpcCustomer = CustomerMapper.ToGrpcCustomer(customer);
            var response = await _client.CreateCustomerAsync(new CreateCustomerRequest
            {
                Name = grpcCustomer.Name,
                Email = grpcCustomer.Email,
                Discount = grpcCustomer.Discount,
                CanRemove = grpcCustomer.CanRemove
            }, _headers);
            return CustomerMapper.ToLocalCustomer(response.Customer);
        }

        public async Task<CustomersTestApp.Models.Customer> UpdateCustomerAsync(CustomersTestApp.Models.Customer customer)
        {
            var grpcCustomer = CustomerMapper.ToGrpcCustomer(customer);
            var response = await _client.UpdateCustomerAsync(new UpdateCustomerRequest
            {
                Id = grpcCustomer.Id,
                Name = grpcCustomer.Name,
                Email = grpcCustomer.Email,
                Discount = grpcCustomer.Discount,
                CanRemove = grpcCustomer.CanRemove
            }, _headers);
            return CustomerMapper.ToLocalCustomer(response.Customer);
        }

        public async Task DeleteCustomerAsync(string id)
        {
            await _client.DeleteCustomerAsync(new DeleteCustomerRequest { Id = id }, _headers);
        }

        public async Task<ObservableCollection<CustomersTestApp.Models.Customer>> FilterCustomersAsync(string filterText, string filterOption)
        {
            var response = await _client.FilterCustomersAsync(new FilterCustomersRequest
            {
                Name = filterText,
                Email = filterOption
            }, _headers);
            var customers = response.Customers.Select(c => CustomerMapper.ToLocalCustomer(c));

            return new ObservableCollection<CustomersTestApp.Models.Customer>(customers);
        }

        // Streaming call to send a stream of customers to the server and receive a response
        public async Task<ObservableCollection<CustomersTestApp.Models.Customer>> CreateCustomersStreamAsync(IEnumerable<CustomersTestApp.Models.Customer> customers)
        {
            var call = _client.CreateCustomersStream(_headers);
            foreach (var customer in customers)
            {
                await call.RequestStream.WriteAsync(new CreateCustomerRequest
                {
                    Name = customer.Name,
                    Email = customer.Email,
                    Discount = customer.Discount,
                    CanRemove = customer.Can_Remove
                });
            }

            await call.RequestStream.CompleteAsync();

            var response = await call.ResponseAsync;
            var grpcCustomers = response.Customers.Select(c => CustomerMapper.ToLocalCustomer(c));
            return new ObservableCollection<CustomersTestApp.Models.Customer>(grpcCustomers);
        }
    }
}
