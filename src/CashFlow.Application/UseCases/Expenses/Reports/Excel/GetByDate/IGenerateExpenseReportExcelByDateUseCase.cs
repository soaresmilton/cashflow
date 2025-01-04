namespace CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByDate;
public interface IGenerateExpenseReportExcelByDateUseCase
{
    Task<byte[]> Execute(DateOnly startDate, DateOnly endDate);
}
