using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DataStoreWithToken.Models;

namespace DataStoreWithToken.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DataStoreWithToken.Models.DataStore> DataStore { get; set; }
        public DbSet<DataStoreWithToken.Models.DataStoreItem> DataStoreItem { get; set; }

        public DbSet<DataStoreWithToken.Models.ItemDetail> ItemDetail { get; set; }

        public DbSet<DataStoreWithToken.Models.Token> Token { get; set; }

        public DbSet<DataStoreWithToken.Models.ActiveToken> ActiveToken { get; set; }
    }
}
