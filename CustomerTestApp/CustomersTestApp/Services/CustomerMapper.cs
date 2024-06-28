namespace CustomersTestApp.Services
{
    public static class CustomerMapper
    {
        public static CustomersTestApp.Models.Customer ToLocalCustomer(CustomerTestApp.Customer grpcCustomer)
        {
            if (grpcCustomer == null) return null;

            return new CustomersTestApp.Models.Customer
            {
                Id = grpcCustomer.Id,
                Name = grpcCustomer.Name,
                Email = grpcCustomer.Email,
                Discount = grpcCustomer.Discount,
                Can_Remove = grpcCustomer.CanRemove
            };
        }

        public static CustomerTestApp.Customer ToGrpcCustomer(CustomersTestApp.Models.Customer localCustomer)
        {
            if (localCustomer == null) return null;

            return new CustomerTestApp.Customer
            {
                Id = localCustomer.Id,
                Name = localCustomer.Name,
                Email = localCustomer.Email,
                Discount = localCustomer.Discount,
                CanRemove = localCustomer.Can_Remove
            };
        }
    }
}
