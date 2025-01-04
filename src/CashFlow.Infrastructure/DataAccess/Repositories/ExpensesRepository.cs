using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

//Aqui devemos garantir que essa classe não será utilizada dentro do projeto da API
internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;
    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<bool> Delete(long id)
    {
        var result = await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
        if(result is null)
        {
            return false;
        }

       _dbContext.Expenses.Remove(result);

        return true;
    }

    public async Task<List<Expense>> GetAll()
    {
        // To List Async => Retorna todos registros do banco de dados (SELECT)
        // Usar AsNoTracking para melhorar a performance para implementações que não modifica o conteúdo dos dados
        return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id)
    {
        return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long id)
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month:date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext
            .Expenses
            .AsNoTracking()
            .Where(expense => expense.CreatedDate >= startDate && expense.CreatedDate <= endDate)
            .OrderBy(expense => expense.CreatedDate)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }

    public async Task<List<Expense>> FilterByDate(DateOnly startDate, DateOnly endDate)
    {
        var initialDate = new DateTime(year: startDate.Year, month: startDate.Month, day: startDate.Day).Date;

        var finalDate = new DateTime(year: endDate.Year, month: endDate.Month, day: endDate.Day, hour: 23, minute: 59, second: 59);

        return await _dbContext
            .Expenses
            .AsNoTracking()
            .Where(expense => expense.CreatedDate >= initialDate && expense.CreatedDate <= finalDate)
            .OrderBy (expense => expense.CreatedDate)
            .ThenBy (expense => expense.Title)
            .ToListAsync();
    }
}
