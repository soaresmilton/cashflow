﻿namespace CashFlow.Communication.Responses.Expenses;
public class ResponseShortExpenseJson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
