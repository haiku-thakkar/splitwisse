using SecondSplitWise.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.DBContext
{
    public class SecondSplitWiseContext : DbContext
    {
        public SecondSplitWiseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupMember> GroupMember {get; set;}
        public DbSet<Expense> Expense { get; set; }
        public DbSet<ExpenseMember> ExpenseMember { get; set; }
        public DbSet<Payer> Payer { get; set; }
        public DbSet<Friend> Friend { get; set; }
        public DbSet<Settlement> Settlement { get; set; }
        public DbSet<Transactions> Transactions { get; set; }

    }
}
