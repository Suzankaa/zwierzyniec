using System.Collections.Generic;
using Zwierzyniec.Models.Input;
using Zwierzyniec.Models.Output;

namespace Zwierzyniec.Services
{
    public interface IMockDataService
    {
        IReadOnlyCollection<AnimalResponse> GetAnimals();
        AnimalResponse AddAnimal(AnimalAdd animal);
        AnimalResponse? UpdateAnimal(AnimalEdit animal);
        bool DeleteAnimal(int id);
        AnimalResponse? GetAnimalById(int id);
        IReadOnlyCollection<AnimalResponse> FilterAnimals(IEnumerable<string>? tags);

        IReadOnlyCollection<ProductResponse> GetProducts();
        ProductResponse AddProduct(ProductAdd product);
        ProductResponse? UpdateProduct(ProductEdit product);
        bool DeleteProduct(int id);
        IReadOnlyCollection<ProductResponse> FilterProducts(IEnumerable<string>? tags);
        ProductResponse? IncreaseProductVolume(int id, int count);
        ProductResponse? ReduceProductVolume(int id, int count, out bool insufficientStock);

        OrderResponse CreateOrder(OrderAdd order);
        OrderResponse? GetOrderById(int orderId);
        bool CancelOrder(int orderId);
        IReadOnlyCollection<OrderResponse> GetOrdersByUserId(int userId);
        IReadOnlyCollection<OrderResponse> GetOrdersByStatus(string status);

        UserResponse? GetUserById(int userId);
        UserResponse CreateUser(UserAdd userAdd);
        UserResponse? UpdateUser(UserEdit userEdit);
        bool DeleteUser(int userId);
    }
}
