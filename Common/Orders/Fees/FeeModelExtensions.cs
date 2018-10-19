/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using QuantConnect.Securities;

namespace QuantConnect.Orders.Fees
{
    /// <summary>
    /// Provides extension methods as backwards compatibility shims
    /// </summary>
    public static class FeeModelExtensions
    {
        /// <summary>
        /// Gets the order fee associated with the specified order. This returns the cost
        /// of the transaction in the account currency
        /// </summary>
        /// <param name="model">The fee model</param>
        /// <param name="security">The security matching the order</param>
        /// <param name="order">The order to compute fees for</param>
        /// <returns>The cost of the order in units of the account currency</returns>
        public static decimal GetOrderFee(this IFeeModel model, Security security, Order order)
        {
            // invoking this extension assumes the fee is in the account currency
            var converter = new IdentityCurrencyConverter(CashBook.AccountCurrency);
            var context = new OrderFeeContext(security, order, converter);
            var feeCashAmount = model.GetOrderFee(context).Value;

            // this will throw if it's not already in the account currency
            // as a means of confirming pre-conditions of this method
            var feeInAccountCurrency = feeCashAmount.ValueInAccountCurrency;

            return feeInAccountCurrency.Amount;
        }
    }
}