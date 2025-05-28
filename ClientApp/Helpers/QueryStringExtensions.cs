using System.Web;
using ClientApp.Shared.DTOs.TransactionDto;

namespace ClientApp.Helpers;

public static class QueryStringExtensions
{
    public static string ToQueryString(this TransactionQueryObject query)
    {
        var parameters = HttpUtility.ParseQueryString(string.Empty);

        if (query.Type != null)
            parameters["Type"] = query.Type.ToString();
        if (query.PaymentMethod != null)
            parameters["PaymentMethod"] = query.PaymentMethod.ToString();
        if (query.TransactionStatus != null)
            parameters["TransactionStatus"] = query.TransactionStatus.ToString();
        if (query.TransactionDate != null)
            parameters["TransactionDate"] = query.TransactionDate?.ToString("o");
        if (query.SentTransactions != null)
            parameters["SentTransactions"] = query.SentTransactions.ToString();
        if (query.ReceivedTransactions != null)
            parameters["ReceivedTransactions"] = query.ReceivedTransactions.ToString();
        if (query.SortBy != null)
            parameters["SortBy"] = query.SortBy.ToString();

        parameters["IsDescending"] = query.IsDescending.ToString();
        parameters["PageNumber"] = query.PageNumber.ToString();
        parameters["PageSize"] = query.PageSize.ToString();

        return "?" + parameters.ToString();
    }
}