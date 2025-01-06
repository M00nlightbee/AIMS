using System.Collections.Generic;

namespace AIMS.Utilities
{
    public static class RoleHelper
    {
        private static readonly Dictionary<string, List<string>> RolePermissions = new()
        {
            { "Admin", new List<string> { "Products", "Order", "Inventory", "History", "User Control", "Analytics" } },
            { "Branch Manager", new List<string> { "Products", "Inventory", "History", "Analytics" } },
            { "Sales Associate", new List<string> { "Products", "Order" } }
        };

        public static bool CanAccess(string role, string linkName)
        {
            if (string.IsNullOrEmpty(role)) return false;

            return RolePermissions.ContainsKey(role) && RolePermissions[role].Contains(linkName);
        }
    }
}
