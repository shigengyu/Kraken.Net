﻿using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.Rest.Spot;
using Kraken.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kraken.Net.Objects.Models;

namespace Kraken.Net.Clients.Rest.Spot
{
    public class KrakenClientSpotExchangeData: IKrakenClientSpotExchangeData
    {
        private KrakenClientSpot _baseClient;

        internal KrakenClientSpotExchangeData(KrakenClientSpot baseClient)
        {
            _baseClient = baseClient;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.Execute<KrakenServerTime>(_baseClient.GetUri("0/public/Time"), HttpMethod.Get, ct).ConfigureAwait(false);
            if (!result)
                return WebCallResult<DateTime>.CreateErrorResult(result.Error!);
            return new WebCallResult<DateTime>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.UnixTime, null);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenSystemStatus>(_baseClient.GetUri("0/public/SystemStatus"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenAssetInfo>>> GetAssetsAsync(IEnumerable<string>? assets = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (assets?.Any() == true)
                parameters.AddOptionalParameter("asset", string.Join(",", assets));

            return await _baseClient.Execute<Dictionary<string, KrakenAssetInfo>>(_baseClient.GetUri("0/public/Assets"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenSymbol>>> GetSymbolsAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (symbols?.Any() == true)
                parameters.AddOptionalParameter("pair", string.Join(",", symbols));

            return await _baseClient.Execute<Dictionary<string, KrakenSymbol>>(_baseClient.GetUri("0/public/AssetPairs"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickerAsync(string symbol, CancellationToken ct = default)
            => GetTickersAsync(new string[] { symbol }, ct);

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
        {
            if (!symbols.Any())
                throw new ArgumentException("No symbols defined to get ticker data for");

            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("pair", string.Join(",", symbols));

            var result = await _baseClient.Execute<Dictionary<string, KrakenRestTick>>(_baseClient.GetUri("0/public/Ticker"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result)
                return result;

            foreach (var item in result.Data)
                item.Value.Symbol = item.Key;

            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenKlinesResult>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? since = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
                {"interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}
            };
            parameters.AddOptionalParameter("since", since.HasValue ? JsonConvert.SerializeObject(since, new TimestampSecondsConverter()) : null);
            return await _baseClient.Execute<KrakenKlinesResult>(_baseClient.GetUri("0/public/OHLC"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("count", limit);
            var result = await _baseClient.Execute<Dictionary<string, KrakenOrderBook>>(_baseClient.GetUri("0/public/Depth"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result)
                return new WebCallResult<KrakenOrderBook>(result.ResponseStatusCode, result.ResponseHeaders, null, result.Error);
            return result.As(result.Data.First().Value);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenTradesResult>> GetTradeHistoryAsync(string symbol, DateTime? since = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("since", since.HasValue ? JsonConvert.SerializeObject(since, new TimestampNanoSecondsConverter()) : null);
            return await _baseClient.Execute<KrakenTradesResult>(_baseClient.GetUri("0/public/Trades"), HttpMethod.Get, ct, parameters: parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenSpreadsResult>> GetRecentSpreadAsync(string symbol, DateTime? since = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("since", since.HasValue ? JsonConvert.SerializeObject(since, new TimestampSecondsConverter()) : null);
            return await _baseClient.Execute<KrakenSpreadsResult>(_baseClient.GetUri("0/public/Spread"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }
    }
}
