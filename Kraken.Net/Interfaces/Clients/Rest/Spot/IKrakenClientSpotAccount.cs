﻿using CryptoExchange.Net.Objects;
using Kraken.Net.Enums;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Socket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kraken.Net.Interfaces.Clients.Rest.Spot
{
    public interface IKrakenClientSpotAccount
    {

        /// <summary>
        /// Get balances
        /// </summary>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with balances for assets</returns>
        Task<WebCallResult<Dictionary<string, decimal>>> GetBalancesAsync(string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get balances including quantity in holding
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <returns>Dictionary with balances for assets</returns>
        Task<WebCallResult<Dictionary<string, KrakenBalanceAvailable>>> GetAvailableBalancesAsync(string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade balance
        /// </summary>
        /// <param name="baseAsset">Base asset to get trade balance for</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade balance data</returns>
        Task<WebCallResult<KrakenTradeBalance>> GetTradeBalanceAsync(string? baseAsset = null, string? twoFactorPassword = null, CancellationToken ct = default);


        /// <summary>
        /// Get a list of open positions
        /// </summary>
        /// <param name="transactionIds">Filter by transaction ids</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with position info</returns>
        Task<WebCallResult<Dictionary<string, KrakenPosition>>> GetOpenPositionsAsync(IEnumerable<string>? transactionIds = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get ledger entries info
        /// </summary>
        /// <param name="assets">Filter list by asset names</param>
        /// <param name="entryTypes">Filter list by entry types</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Ledger entries page</returns>
        Task<WebCallResult<KrakenLedgerPage>> GetLedgerInfoAsync(IEnumerable<string>? assets = null, IEnumerable<LedgerEntryType>? entryTypes = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific ledger entries
        /// </summary>
        /// <param name="ledgerIds">The ids to get info for</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ledger entry info</returns>
        Task<WebCallResult<Dictionary<string, KrakenLedgerEntry>>> GetLedgersEntryAsync(IEnumerable<string>? ledgerIds = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade volume
        /// </summary>
        /// <param name="symbols">Symbols to get data for</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade fee info</returns>
        Task<WebCallResult<KrakenTradeVolume>> GetTradeVolumeAsync(IEnumerable<string>? symbols = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit methods
        /// </summary>
        /// <param name="asset">Asset to get methods for</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Array with methods for deposit</returns>
        Task<WebCallResult<IEnumerable<KrakenDepositMethod>>> GetDepositMethodsAsync(string asset, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit addresses for an asset
        /// </summary>
        /// <param name="asset">The asset to get the deposit address for</param>
        /// <param name="depositMethod">The method of deposit</param>
        /// <param name="generateNew">Whether to generate a new address</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenDepositAddress>>> GetDepositAddressesAsync(string asset, string depositMethod, bool generateNew = false, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get status deposits for an asset
        /// </summary>
        /// <param name="asset">Asset to get deposit info for</param>
        /// <param name="depositMethod">The deposit method</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit status list</returns>
        Task<WebCallResult<IEnumerable<KrakenDepositStatus>>> GetDepositStatusAsync(string asset, string depositMethod, string? twoFactorPassword = null, CancellationToken ct = default);


        /// <summary>
        /// Get info before a withdrawal
        /// </summary>
        /// <param name="asset">The asset</param>
        /// <param name="key">The withdrawal key name</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenWithdrawInfo>> GetWithdrawInfoAsync(string asset, string key, decimal quantity,
            string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Withdraw funds
        /// </summary>
        /// <param name="asset">The asset being withdrawn</param>
        /// <param name="key">The withdrawal key name, as set up on your account</param>
        /// <param name="quantity">The quantity to withdraw, including fees</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdraw reference id</returns>
        Task<WebCallResult<KrakenWithdraw>> WithdrawAsync(string asset, string key, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get the token to connect to the private websocket streams
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<KrakenWebSocketToken>> GetWebsocketTokenAsync(CancellationToken ct = default);
    }
}
