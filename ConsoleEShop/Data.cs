using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleEShop.Models;

namespace ConsoleEShop
{
    public static class Data
    {
        public static List<Order> Orders { get; set; } = new List<Order>();
        

        public static List<User> Users { get; set; } = new List<User>
        {
            new User
            {
                Id=12,
                Role = Roles.RegisteredUser,
                Name = "User",
                Password = "User"
            },
            new User
            {
                Id=1,
                Role = Roles.Administrator,
                Name = "Administrator",
                Password = "Administrator"
            }
        };

        public static readonly Category[] Categories =
        {
            new Category {Id = 1, Name = "Подарки"},
            new Category {Id = 2, Name = "Щенки"},
            new Category {Id = 3, Name = "Игры"},
            new Category {Id = 4, Name = "Еда и напитки"},
            new Category {Id = 5, Name = "Средства против змей"},
        };

        public static List<Product> Products { get; set; } = new List<Product>
        {
            new Product
            {
                Id=1, Name = "Мазь против змей", Price = 350M, CategoryId = 5,
                Description = "К счастью, мое потребительское любопытство не зашло так далеко чтобы купить эту поделку."

            },
            new Product
            {
                Id=2, Name = "Табличка 'Не беспокоить'", Price = 85M, CategoryId = 1,
                Description = "Стоит у меня такая вот в шкафу. Выкинуть жалко, подарить некому. Продать тоже смысла ноль… Технически-то сделано неплохо, не подвальщина."
            },
            new Product
            {
                Id=3, Name = "Щенок лошади", Price = 12000M, CategoryId = 2,
                Description = "Зубы ровный, без пробег. Хороший. Зовут Олег"
            },
            new Product
            {
                Id=4, Name = "Тенисный мяч", Price = 12M, CategoryId = 3,
                Description = "Фитнес-тренажёр для разминки перед партией в карманный бильярд."
            },
            new Product
            {
                Id=5, Name = "Терка", Price = 3M, CategoryId = 4,
                Description = "Вещь, которая удачно комбинирует в себе бессмысленность и опасность."

            },
            new Product
            {
                Id=6, Name = "Пиво Жигулевское 1л", Price = 18M, CategoryId = 4,
                Description = "Я люблю пельмени и экономить деньги. Поэтому это мой выбор"
            },
            new Product
            {
                Id=7, Name = "Шашки", Price = 65M, CategoryId = 3,
                Description = "Шашки трехцветные"
            },
        };
    }
}
