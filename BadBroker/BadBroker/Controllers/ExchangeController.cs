using AutoMapper;
using BadBroker.Extension;
using BadBroker.Logic.Service;
using BadBroker.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadBroker.Controllers
{
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly ILogger<ExchangeController> _logger;
        private readonly IExchangeService _exchangeService;
        private readonly IMapper _mapper;

        public ExchangeController(
            ILogger<ExchangeController> logger,
            IExchangeService exchangeService,
            IMapper mapper
            )
        {
            _logger = logger;
            _exchangeService = exchangeService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public ActionResult Best(DateTime startDate, DateTime endDate, decimal moneyUsd)
        {
            var tmp = _exchangeService.GetBestStrategy(startDate, endDate, moneyUsd, "USD");

            var tmp1 = _mapper.Map<List<BestStrategyViewModel>>(tmp);

            return Ok(tmp1.DistinctBestStrategy());
        }
    }
}
