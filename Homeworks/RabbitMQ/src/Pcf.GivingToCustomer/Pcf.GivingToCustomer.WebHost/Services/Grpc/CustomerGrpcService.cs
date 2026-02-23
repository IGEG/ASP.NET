using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Mappers;
using Pcf.GivingToCustomer.WebHost.Models;
using Pcf.GivingToCustomer.WebHost.Protos;
using static Pcf.GivingToCustomer.WebHost.Protos.CustomerGrpcService;
using CustomerResponse = Pcf.GivingToCustomer.WebHost.Protos.CustomerResponse;
using PreferenceResponse = Pcf.GivingToCustomer.WebHost.Protos.PreferenceResponse;
using PromoCodeShortResponse = Pcf.GivingToCustomer.WebHost.Protos.PromoCodeShortResponse;

namespace Pcf.GivingToCustomer.WebHost.Services.Grpc
{
    /// <summary>
    /// Сервис для работы с GRPC Customer
    /// </summary>
    public class CustomerGrpcService : CustomerGrpcServiceBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly ILogger<CustomerGrpcService> _logger;

        public CustomerGrpcService( IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository, ILogger<CustomerGrpcService> logger)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
            _logger = logger;
        }

        /// <summary>
        /// Получаем один Customer по Id
        /// </summary>
        public override async Task<CustomerResponse> GetCustomer(GetCustomerRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Получение customer по id: {request.Id}");

            if (!Guid.TryParse(request.Id, out var customerId))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Не верный формат ID."));

            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Customer c id {request.Id} не найден."));

            return MapToProtoCustomerResponse(customer);
        }

        /// <summary>
        /// Получем все Customers
        /// </summary>
        public override async Task<CustomersListResponse> GetCustomers(GetAllRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Получение всех customers");

            var customers = await _customerRepository.GetAllAsync();

            var response = new CustomersListResponse();
            response.Customers.AddRange(customers.Select(MapToProtoCustomerResponse));

            return response;
        }

        /// <summary>
        /// Добавляем нового Customer
        /// </summary>
        public override async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Создание нового customer: {request.FirstName} {request.LastName}");

            var preferenceIds = request.PreferenceIds
                .Select(id => Guid.TryParse(id, out var guid) ? guid : Guid.Empty)
                .Where(id => id != Guid.Empty)
                .ToList();

            if (!preferenceIds.Any())
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Не найдены валидные ID предпочтений."));

            // Получаем предпочтения из БД
            var preferences = await _preferenceRepository.GetRangeByIdsAsync(preferenceIds);

            // Создаем модель запроса для маппера
            var createRequest = new CreateOrEditCustomerRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PreferenceIds = preferenceIds
            };

            var customer = CustomerMapper.MapFromModel(createRequest, preferences);

            await _customerRepository.AddAsync(customer);

            return MapToProtoCustomerResponse(customer);
        }

        /// <summary>
        /// Обновление Customer
        /// </summary>
        public override async Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Обновление customer: {request.Id}");

            if (!Guid.TryParse(request.Id, out var customerId))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Не верный формат customer ID "));

            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Customer c id {request.Id} не найден."));

            var preferenceIds = request.PreferenceIds
                .Select(id => Guid.TryParse(id, out var guid) ? guid : Guid.Empty)
                .Where(id => id != Guid.Empty)
                .ToList();

            var preferences = await _preferenceRepository.GetRangeByIdsAsync(preferenceIds);

            // Создаем модель запроса для маппера
            var updateRequest = new CreateOrEditCustomerRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PreferenceIds = preferenceIds
            };

            CustomerMapper.MapFromModel(updateRequest, preferences, customer);

            await _customerRepository.UpdateAsync(customer);

            return MapToProtoCustomerResponse(customer);
        }

        /// <summary>
        /// Удаление Customer
        /// </summary>
        public override async Task<DeleteCustomerResponse> DeleteCustomer(DeleteCustomerRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Удаление customer с id {request.Id}");

            var response = new DeleteCustomerResponse();

            if (!Guid.TryParse(request.Id, out var customerId))
            {
                response.Success = false;
                response.Message = "Не верный формат ID ";
                return response;
            }

            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                response.Success = false;
                response.Message = $"Customer с id {request.Id} не найден";
                return response;
            }

            await _customerRepository.DeleteAsync(customer);

            response.Success = true;
            response.Message = "Customer успешно удален";

            return response;
        }

        // Маппер из доменной модели в protobuf ответ
        private CustomerResponse MapToProtoCustomerResponse(Customer customer)
        {
            var response = new CustomerResponse
            {
                Id = customer.Id.ToString(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            };

            // Добавляем предпочтения
            if (customer.Preferences != null)
            {
                foreach (var pref in customer.Preferences)
                {
                    response.Preferences.Add(new PreferenceResponse
                    {
                        Id = pref.PreferenceId.ToString(),
                        Name = pref.Preference?.Name ?? string.Empty
                    });
                }
            }

            // Добавляем промокоды
            if (customer.PromoCodes != null)
            {
                foreach (var pc in customer.PromoCodes)
                {
                    response.PromoCodes.Add(new PromoCodeShortResponse
                    {
                        Id = pc.PromoCodeId.ToString(),
                        Code = pc.PromoCode?.Code ?? string.Empty,
                        ServiceInfo = pc.PromoCode?.ServiceInfo ?? string.Empty,
                        BeginDate = pc.PromoCode?.BeginDate.ToString("yyyy-MM-dd") ?? string.Empty,
                        EndDate = pc.PromoCode?.EndDate.ToString("yyyy-MM-dd") ?? string.Empty,
                        PartnerId = pc.PromoCode?.PartnerId.ToString() ?? string.Empty
                    });
                }
            }

            return response;
        }
    }
}