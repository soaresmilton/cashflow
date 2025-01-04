
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;
internal class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _reposytory;
    private readonly IUnityOfWork _unityOfWork;
    public DeleteExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IUnityOfWork unityOfWork
        )
    {
        _reposytory = repository;
        _unityOfWork = unityOfWork;
    }
    public async Task Execute(long id)
    {
        var result = await _reposytory.Delete(id);

        if(!result)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        await _unityOfWork.Commit();
    }
}
