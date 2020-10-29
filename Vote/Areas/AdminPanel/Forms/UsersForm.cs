using System;
using System.Collections.Generic;

namespace Vote.Areas.AdminPanel.Forms
{
    public class UsersForm
    {
        public List<AdminPanelUser> Users { get; set; }
    }
    public class AdminPanelUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Phone { get; set; }
        public int AccessFailedCount { get; set; }
        public List<string> Roles { get; set; }
    }
}
