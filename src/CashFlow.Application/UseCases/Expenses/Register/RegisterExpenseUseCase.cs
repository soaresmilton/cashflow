﻿using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;
public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;   
    public RegisterExpenseUseCase(
        IExpensesWriteOnlyRepository repository, 
        IUnityOfWork unityOfWork,
        IMapper mapper
        )
    {
        _repository = repository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }
    public async Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request)
    {
        Validate(request);

        var entity = _mapper.Map<Expense>(request);

        await  _repository.Add(entity);

        await _unityOfWork.Commit();

        return _mapper.Map<ResponseRegisteredExpenseJson>(entity);
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
