using System.Text.Json.Serialization;

namespace FinanceManger.Contracts.Transactions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    Income,
    Expense
}
