using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Excel.GetByDate;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetExcel(
        [FromHeader] DateOnly month,
        [FromServices] IGenerateExpenseReportExcelUseCase useCase
        )
    {
        byte[] file = await useCase.Execute(month);

        if(file.Length > 0)
        {
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"{timeStamp} - report.xlsx";
            return File(file, MediaTypeNames.Application.Octet, fileName);
        }

        return NoContent();
    }

    [HttpGet("getexcelbydate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> GetExcelByDate(
        [FromHeader] DateOnly startDate, 
        [FromHeader] DateOnly endDate, 
        [FromServices] IGenerateExpenseReportExcelByDateUseCase useCase)
    {
        byte[] file = await useCase.Execute(startDate, endDate);
        if(file.Length > 0)
        {
            var startDateTimeStamp = startDate.ToString("yyyyMMdd");
            var endDateTimeStamp = endDate.ToString("yyyyMMdd");
            var timeStamp = $"{startDateTimeStamp} - {endDateTimeStamp}";
            var fileName = $"{timeStamp} - report.xlsx";
            return File(file, MediaTypeNames.Application.Octet, fileName);
        }

        return NoContent();
    }

    [HttpGet("pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf(
        [FromQuery] DateOnly month,
        [FromServices] IGenerateExpenseReportPdfUseCase useCase
        )
    {
        byte[] file = await useCase.Execute(month);
        if(file.Length > 0)
        {
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"{timeStamp} - report.pdf";

            return File(file, MediaTypeNames.Application.Pdf, fileName);
        }
        return NoContent();
    }
}
