using Grpc.Net.Client;
using Pcf.GivingToCustomer.WebHost.Protos;

namespace GrpcClient
{
    class Program
    {
        const string ServerAddress = "http://localhost:5000";
        const string TestCustomerId = "a6c8c6b1-4349-45b0-ab31-244740aaf0f0";
        static async Task Main(string[] args)
        {
            Console.WriteLine("GRPC запуск");
            await TestGrpcConnection();
            Console.ReadKey();
        }

        static async Task TestGrpcConnection()
        {
            using var channel = GrpcChannel.ForAddress(ServerAddress);
            Console.WriteLine("Канал создан");

            var client = new CustomerGrpcService.CustomerGrpcServiceClient(channel);
            Console.WriteLine("Клиент создан");

            // Все customers
            Console.WriteLine("Все GetCustomers");
            try
            {
                var customersResponse = await client.GetCustomersAsync(new GetAllRequest());
                Console.WriteLine($"Получено клиентов: {customersResponse.Customers.Count}");

                if (customersResponse.Customers.Count > 0)
                {
                    Console.WriteLine("Клиенты:");
                    foreach (var customer in customersResponse.Customers)
                    {
                        Console.WriteLine($"  - {customer.FirstName} {customer.LastName} ({customer.Email})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в GetCustomers: {ex.Message}");
            }

            // Получение одного клиента
            Console.WriteLine("Один GetCustomer");
            try
            {

                Console.WriteLine($"Запрашиваем клиента с Id: {TestCustomerId}");

                var customerResponse = await client.GetCustomerAsync(new GetCustomerRequest
                {
                    Id = TestCustomerId
                });

                Console.WriteLine($"Клиент найден: {customerResponse.FirstName} {customerResponse.LastName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в GetCustomer: {ex.Message}");
            }
        }
    }
}