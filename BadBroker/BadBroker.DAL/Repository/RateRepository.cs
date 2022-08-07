using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadBroker.DAL.Context;
using BadBroker.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace BadBroker.DAL.Repository
{
    public interface IRateRepository
    {
        List<Rate> GetRates(int currencyPairId, DateTime from, DateTime to);

        Currency GetCurrencyByKey(string key);

        IEnumerable<Currency> GetCurrencies();

        void AddRate(Rate rate);

        CurrencyPair GetCurrencyPair(int baseCurrencyId, int counterCurrencyId);

        int Commit();
    }

    public class RateRepository : IRateRepository
    {
        readonly IAppDbContext _context;

        public RateRepository(IAppDbContext context)
        {
            _context = context;
        }

        public List<Rate> GetRates(int currencyPairId, DateTime from, DateTime to)
        {
            var result = _context.GetRates(from, to)
                .Where(r => r.CurrencyPairId == currencyPairId)
                .ToList();

            return result;
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public Currency GetCurrencyByKey(string key)
        {
            var result = _context.GetCurrencyByKey(key);

            return result;
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            var result = _context.GetCurrencies();

            return result;
        }

        public void AddRate(Rate rate)
        {
            _context.AddRate(rate);

            Commit();
        }

        public CurrencyPair GetCurrencyPair(int baseCurrencyId, int counterCurrencyId)
        {
            return _context.GetCurrencyPair(baseCurrencyId, counterCurrencyId);
        }

    }
}
