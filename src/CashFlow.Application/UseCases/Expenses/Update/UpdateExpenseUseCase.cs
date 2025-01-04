using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;
public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly IExpensesUpdateOnlyRepository _repository;
    public UpdateExpenseUseCase(
        IUnityOfWork unityOfWork,
        IMapper mapper,
        IExpensesUpdateOnlyRepository repository
        )
    {
        _unityOfWork = unityOfWork;
        _mapper = mapper;
        _repository = repository;
    }
    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);

        var expense = await _repository.GetById( id );
        
        if (expense is null) 
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        };

        _mapper.Map(request, expense);

        _repository.Update(expense);

        await _unityOfWork.Commit();
    }

    private void Validate(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(f => f.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
