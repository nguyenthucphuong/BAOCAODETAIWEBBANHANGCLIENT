﻿namespace ClientSaleApi.Models.Roles
{
    public class ListRole
    {
        public int TotalCount { get; set; }
        public List<RoleEx> Items { get; set; } = new List<RoleEx>();
    }
}
