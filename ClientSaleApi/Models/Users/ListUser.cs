﻿namespace ClientSaleApi.Models.Users
{
    public class ListUser
    {
        public int TotalCount { get; set; }
        public List<UserEx> Items { get; set; } = new List<UserEx>();
    }
}
