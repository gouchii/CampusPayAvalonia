using System.Collections.Generic;
using ClientApp.Models;

namespace ClientApp.Messages
{
    public class TransactionsLoadedMessage
    {
        public List<TransactionModel> Transactions { get; }

        public TransactionsLoadedMessage(List<TransactionModel> transactions)
        {
            Transactions = transactions;
        }
    }
}