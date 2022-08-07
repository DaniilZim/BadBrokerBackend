using BadBroker.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BadBroker.DAL.Context
{
    public interface IAppDbContext
    {
        ICollection<Rate> GetRates(DateTime from, DateTime to);

        /// <summary>
        /// Get currency
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Currency GetCurrencyByKey(string key);

        ICollection<Currency> GetCurrencies();

        void AddRate(Rate rate);

        int SaveChanges();

        CurrencyPair GetCurrencyPair(int baseCurrencyId, int counterCurrencyId);
    }

    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<Currency> Currences { get; set; }

        public DbSet<CurrencyPair> CurrencyPairs { get; set; }

        public DbSet<Rate> Rates { get; set; }


        public AppDbContext()
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Exchange;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>()
            .HasOne(p => p.CurrencyPair)
            .WithMany(t => t.Rates)
            .HasForeignKey(p => p.CurrencyPairId);

            modelBuilder.Entity<CurrencyPair>()
            .HasOne(p => p.BaseCurrency)
            .WithMany(t => t.CurrencyBasePairs)
            .HasForeignKey(p => p.BaseCurrencyId);

            modelBuilder.Entity<CurrencyPair>()
            .HasOne(p => p.CounterCurrency)
            .WithMany(t => t.CurrencyCounterPairs)
            .HasForeignKey(p => p.CounterCurrencyId);

            // Test Data
            modelBuilder.Entity<Currency>().HasData(
                new Currency[]
                {
                    new Currency { Id = 1, Name = "US Dollar", Key = "USD"},
                    new Currency { Id = 2, Name = "Russian Ruble", Key = "RUB"},
                    new Currency { Id = 3, Name = "Euro", Key = "EUR"},
                    new Currency { Id = 4, Name = "Pound Sterling", Key = "GBP"},
                    new Currency { Id = 5, Name = "Yen", Key = "JPY"}
                });

            modelBuilder.Entity<CurrencyPair>().HasData(
                new CurrencyPair[]
                {
                    new CurrencyPair { Id = 1, Name = "USD/RUB", BaseCurrencyId = 1, CounterCurrencyId = 2},
                    new CurrencyPair { Id = 2, Name = "USD/EUR", BaseCurrencyId = 1, CounterCurrencyId = 3},
                    new CurrencyPair { Id = 3, Name = "USD/GBP", BaseCurrencyId = 1, CounterCurrencyId = 4},
                    new CurrencyPair { Id = 4, Name = "USD/JPY", BaseCurrencyId = 1, CounterCurrencyId = 5}
                });

            modelBuilder.Entity<Rate>().HasData(
                new Rate[]
                {
                    new Rate { Id =  1, DateTrunc = DateTime.Now.Date.AddDays(-10), Value = 65.0M, CurrencyPairId = 1},
                    new Rate { Id =  2, DateTrunc = DateTime.Now.Date.AddDays( -9), Value = 64.4M, CurrencyPairId = 1},
                    //new Rate { Id =  3, DateTrunc = DateTime.Now.Date.AddDays( -8), Value = 62.4M, CurrencyPairId = 1},
                    //new Rate { Id =  4, DateTrunc = DateTime.Now.Date.AddDays( -7), Value = 60.4M, CurrencyPairId = 1},
                    new Rate { Id =  5, DateTrunc = DateTime.Now.Date.AddDays( -6), Value = 70.9M, CurrencyPairId = 1},
                    new Rate { Id =  6, DateTrunc = DateTime.Now.Date.AddDays( -5), Value = 80.9M, CurrencyPairId = 1},
                    new Rate { Id =  7, DateTrunc = DateTime.Now.Date.AddDays( -4), Value = 90.9M, CurrencyPairId = 1},
                    new Rate { Id =  8, DateTrunc = DateTime.Now.Date.AddDays( -3), Value = 100.0M, CurrencyPairId = 1},
                    new Rate { Id =  9, DateTrunc = DateTime.Now.Date.AddDays( -2), Value = 90.9M, CurrencyPairId = 1},
                    new Rate { Id = 10, DateTrunc = DateTime.Now.Date.AddDays( -1), Value = 50.9M, CurrencyPairId = 1},
                    new Rate { Id = 11, DateTrunc = DateTime.Now.Date             , Value = 80.9M, CurrencyPairId = 1},
                });
            
        }

        public ICollection<Rate> GetRates(DateTime from, DateTime to)
        {
            return Rates
                .Include(r => r.CurrencyPair)
                .ThenInclude(c => c.BaseCurrency)
                .Include(r => r.CurrencyPair)
                .ThenInclude(c => c.CounterCurrency)
                .Where(r => r.DateTrunc.Date >= from.Date && r.DateTrunc.Date <= to.Date)
                .ToList();
                ;

        }

        public Currency GetCurrencyByKey(string key)
        {
            return Currences
                .Where(c => c.Key == key)
                .SingleOrDefault();
        }

        public ICollection<Currency> GetCurrencies()
        {
            return Currences.ToList();
        }

        public void AddRate(Rate rate)
        {
            Rates.Add(rate);
        }

        public CurrencyPair GetCurrencyPair(int baseCurrencyId, int counterCurrencyId)
        {
            return CurrencyPairs.SingleOrDefault(x => x.BaseCurrencyId == baseCurrencyId && x.CounterCurrencyId == counterCurrencyId);
        }
    }
}
