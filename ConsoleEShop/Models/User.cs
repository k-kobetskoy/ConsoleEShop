using System;
using System.Collections.Generic;
using System.Text;
using ConsoleEShop.Models;

namespace ConsoleEShop
{
    public class User
    {
        public int Id { get; set; }
        public Roles  Role { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

    }

    public enum Roles
    {
        Guest=1,
        RegisteredUser=2,
        Administrator=3
    }
}
