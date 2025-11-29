using System;
using System.Collections.Generic;
using System.Linq;
using Zwierzyniec.Common.Enums;
using Zwierzyniec.Models.Input;
using Zwierzyniec.Models.Output;

namespace Zwierzyniec.Services
{
    public class MockDataService : IMockDataService
    {
        private readonly List<AnimalResponse> _animals;
        private readonly List<ProductResponse> _products;
        private readonly List<OrderResponse> _orders;
        private readonly List<UserResponse> _users;
        private readonly object _sync = new();

        private int _nextAnimalId;
        private int _nextProductId;
        private int _nextOrderId;
        private int _nextUserId;

        public MockDataService()
        {
            _animals = SeedAnimals();
            _products = SeedProducts();
            _orders = SeedOrders();
            _users = SeedUsers();

            _nextAnimalId = _animals.Any() ? _animals.Max(a => a.Id) + 1 : 1;
            _nextProductId = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _nextOrderId = _orders.Any() ? _orders.Max(o => o.Id) + 1 : 1;
            _nextUserId = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
        }

        public IReadOnlyCollection<AnimalResponse> GetAnimals() => _animals.AsReadOnly();

        public AnimalResponse AddAnimal(AnimalAdd animal)
        {
            lock (_sync)
            {
                var response = new AnimalResponse
                {
                    Id = _nextAnimalId++,
                    Name = animal.Name,
                    Age = animal.Age,
                    Price = animal.Price,
                    Stauts = animal.Stauts,
                    Species = animal.Species,
                    Gender = animal.Gender,
                    Description = animal.Description,
                    Photo = animal.Photo,
                    IntakeDate = animal.IntakeDate
                };

                _animals.Add(response);
                return response;
            }
        }

        public AnimalResponse? UpdateAnimal(AnimalEdit animal)
        {
            lock (_sync)
            {
                var existing = _animals.FirstOrDefault(a => a.Id == animal.Id);
                if (existing == null)
                {
                    return null;
                }

                existing.Name = animal.Name;
                existing.Age = animal.Age;
                existing.Price = animal.Price;
                existing.Stauts = animal.Stauts;
                existing.Species = animal.Species;
                existing.Gender = animal.Gender;
                existing.Description = animal.Description;
                existing.Photo = animal.Photo;
                return existing;
            }
        }

        public bool DeleteAnimal(int id)
        {
            lock (_sync)
            {
                return _animals.RemoveAll(a => a.Id == id) > 0;
            }
        }

        public AnimalResponse? GetAnimalById(int id) => _animals.FirstOrDefault(a => a.Id == id);

        public IReadOnlyCollection<AnimalResponse> FilterAnimals(IEnumerable<string>? tags)
        {
            var normalizedTags = NormalizeTags(tags);
            if (normalizedTags.Count == 0)
            {
                return _animals.AsReadOnly();
            }

            return _animals
                .Where(animal => normalizedTags.Any(tag =>
                    animal.Name.Contains(tag, StringComparison.OrdinalIgnoreCase) ||
                    animal.Description.Contains(tag, StringComparison.OrdinalIgnoreCase) ||
                    animal.Species.ToString().Equals(tag, StringComparison.OrdinalIgnoreCase) ||
                    animal.Stauts.ToString().Equals(tag, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public IReadOnlyCollection<ProductResponse> GetProducts() => _products.AsReadOnly();

        public ProductResponse AddProduct(ProductAdd product)
        {
            lock (_sync)
            {
                var response = new ProductResponse
                {
                    Id = _nextProductId++,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Discount = product.Discount,
                    Volume = 10,
                    SoldVolume = 0
                };

                _products.Add(response);
                return response;
            }
        }

        public ProductResponse? UpdateProduct(ProductEdit product)
        {
            lock (_sync)
            {
                var existing = _products.FirstOrDefault(p => p.Id == product.Id);
                if (existing == null)
                {
                    return null;
                }

                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.Discount = product.Discount;
                return existing;
            }
        }

        public bool DeleteProduct(int id)
        {
            lock (_sync)
            {
                return _products.RemoveAll(p => p.Id == id) > 0;
            }
        }

        public IReadOnlyCollection<ProductResponse> FilterProducts(IEnumerable<string>? tags)
        {
            var normalizedTags = NormalizeTags(tags);
            if (normalizedTags.Count == 0)
            {
                return _products.AsReadOnly();
            }

            return _products
                .Where(product => normalizedTags.Any(tag =>
                    product.Name.Contains(tag, StringComparison.OrdinalIgnoreCase) ||
                    product.Description.Contains(tag, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public ProductResponse? IncreaseProductVolume(int id, int count)
        {
            if (count <= 0)
            {
                return null;
            }

            lock (_sync)
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    return null;
                }

                product.Volume += count;
                return product;
            }
        }

        public ProductResponse? ReduceProductVolume(int id, int count, out bool insufficientStock)
        {
            insufficientStock = false;

            if (count <= 0)
            {
                return null;
            }

            lock (_sync)
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    return null;
                }

                if (product.Volume < count)
                {
                    insufficientStock = true;
                    return product;
                }

                product.Volume -= count;
                product.SoldVolume += count;
                return product;
            }
        }

        public OrderResponse CreateOrder(OrderAdd order)
        {
            lock (_sync)
            {
                var response = new OrderResponse
                {
                    Id = _nextOrderId++,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    Paymethod = order.Paymethod,
                    ShippingMethodEnum = order.ShippingMethodEnum,
                    ProductIds = new List<int>(order.ProductIds)
                };

                _orders.Add(response);
                return response;
            }
        }

        public OrderResponse? GetOrderById(int orderId) => _orders.FirstOrDefault(o => o.Id == orderId);

        public bool CancelOrder(int orderId)
        {
            lock (_sync)
            {
                var order = _orders.FirstOrDefault(o => o.Id == orderId);
                if (order == null)
                {
                    return false;
                }

                order.Status = "cancelled";
                return true;
            }
        }

        public IReadOnlyCollection<OrderResponse> GetOrdersByUserId(int userId) =>
            _orders.Where(o => o.UserId == userId).ToList();

        public IReadOnlyCollection<OrderResponse> GetOrdersByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return Array.Empty<OrderResponse>();
            }

            return _orders
                .Where(o => string.Equals(o.Status, status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public UserResponse? GetUserById(int userId) => _users.FirstOrDefault(u => u.Id == userId);

        public UserResponse CreateUser(UserAdd userAdd)
        {
            lock (_sync)
            {
                var response = new UserResponse
                {
                    Id = _nextUserId++,
                    Username = userAdd.Username,
                    Email = userAdd.Email,
                    FirstName = userAdd.FirstName,
                    LastName = userAdd.LastName,
                    UserRole = UserRoleEnum.user
                };

                _users.Add(response);
                return response;
            }
        }

        public UserResponse? UpdateUser(UserEdit userEdit)
        {
            lock (_sync)
            {
                var existing = _users.FirstOrDefault(u => u.Id == userEdit.Id);
                if (existing == null)
                {
                    return null;
                }

                existing.Username = userEdit.Username;
                existing.Email = userEdit.Email;
                existing.FirstName = userEdit.FirstName;
                existing.LastName = userEdit.LastName;
                existing.UserRole = userEdit.UserRole;
                return existing;
            }
        }

        public bool DeleteUser(int userId)
        {
            lock (_sync)
            {
                return _users.RemoveAll(u => u.Id == userId) > 0;
            }
        }

        private static List<AnimalResponse> SeedAnimals() =>
            new()
            {
                new AnimalResponse
                {
                    Id = 1,
                    Name = "Luna",
                    Age = 2,
                    Price = 120.5f,
                    Stauts = StatusEnum.available,
                    Species = SpeciesEnum.Cat,
                    Gender = GenderEnum.Female,
                    Description = "Nieśmiała kotka szuka spokojnego domu.",
                    IntakeDate = DateTime.UtcNow.AddDays(-10)
                },
                new AnimalResponse
                {
                    Id = 2,
                    Name = "Rocky",
                    Age = 4,
                    Price = 200,
                    Stauts = StatusEnum.available,
                    Species = SpeciesEnum.Dog,
                    Gender = GenderEnum.Male,
                    Description = "Energiczny psiak idealny dla aktywnej rodziny.",
                    IntakeDate = DateTime.UtcNow.AddDays(-30)
                },
                new AnimalResponse
                {
                    Id = 3,
                    Name = "Kiwi",
                    Age = 1,
                    Price = 60,
                    Stauts = StatusEnum.pending,
                    Species = SpeciesEnum.Bird,
                    Gender = GenderEnum.Female,
                    Description = "Papużka uwielbiająca towarzystwo.",
                    IntakeDate = DateTime.UtcNow.AddDays(-5)
                }
            };

        private static List<ProductResponse> SeedProducts() =>
            new()
            {
                new ProductResponse
                {
                    Id = 1,
                    Name = "Karma premium",
                    Description = "Bezzbożowa karma dla psów średnich ras.",
                    Price = 89.99f,
                    Discount = 5,
                    Volume = 25,
                    SoldVolume = 60
                },
                new ProductResponse
                {
                    Id = 2,
                    Name = "Drapak XXL",
                    Description = "Stabilny drapak z legowiskiem.",
                    Price = 199.99f,
                    Discount = null,
                    Volume = 5,
                    SoldVolume = 12
                }
            };

        private static List<OrderResponse> SeedOrders() =>
            new()
            {
                new OrderResponse
                {
                    Id = 1,
                    UserId = 1,
                    OrderDate = DateTime.UtcNow.AddDays(-7),
                    Status = "processing",
                    Paymethod = PayMethodEnum.Card,
                    ShippingMethodEnum = ShippingMethodEnum.Courier,
                    ProductIds = new List<int> { 1, 2 }
                },
                new OrderResponse
                {
                    Id = 2,
                    UserId = 2,
                    OrderDate = DateTime.UtcNow.AddDays(-2),
                    Status = "completed",
                    Paymethod = PayMethodEnum.Blik,
                    ShippingMethodEnum = ShippingMethodEnum.Pickup,
                    ProductIds = new List<int> { 2 }
                }
            };

        private static List<UserResponse> SeedUsers() =>
            new()
            {
                new UserResponse
                {
                    Id = 1,
                    Username = "pawel",
                    Email = "pawel@example.com",
                    FirstName = "Paweł",
                    LastName = "Lis",
                    UserRole = UserRoleEnum.worker
                },
                new UserResponse
                {
                    Id = 2,
                    Username = "asia",
                    Email = "asia@example.com",
                    FirstName = "Joanna",
                    LastName = "Mróz",
                    UserRole = UserRoleEnum.user
                }
            };

        private static List<string> NormalizeTags(IEnumerable<string>? tags)
        {
            return tags?
                       .Where(t => !string.IsNullOrWhiteSpace(t))
                       .Select(t => t.Trim())
                       .Where(t => t.Length > 0)
                       .ToList()
                   ?? new List<string>();
        }
    }
}
