using System.Globalization;

namespace CashFlow.API.Middleware;

public class CultureMiddleware
{   
    private readonly RequestDelegate _next;
    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        // Pega uma lista de todas as culturas/linguas suportada pelo .NET  
        var supportedCutures = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

        // Extrai do header da requisição qual é a cultura do cliente
        var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();



        // Define uma cultura padrão
        var cultureInfo = new CultureInfo("en");

        // Se a cultura foi informada pelo cliente, sobrescreve a cultureInfo com o que o cliente passou
        if(
            !string.IsNullOrWhiteSpace(requestedCulture) 
            && supportedCutures.Exists(culture => culture.Name.Equals(requestedCulture))
        )
        {
            cultureInfo = new CultureInfo(requestedCulture);
        }

        // Altera a API para devolver a cultura definida anteriormente (default ou a informada)
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        // Permite o fluxo continuar => "valida" o middleware / libera
        await _next(context);
    }
}
