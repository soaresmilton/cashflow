using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
public class ExpenseReportFontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        // Face Name ==> Nome do arquivo da fonte
        
        var stream = ReadFontFile(faceName) ?? ReadFontFile(FontHelper.DEFAULT_FONT);
        var length = stream!.Length;
        
        var data = new byte[length];

        stream.Read(buffer: data, offset: 0, count: (int)length);

        return data;
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    private Stream? ReadFontFile(string faceName)
    {
        // Pega a referencia do arquivo dll (build do .NET) do projeto de Application e dentro desse projeto queremos ler o arquivo de fonte
        var assembly = Assembly.GetExecutingAssembly();

        // Caminho completo do arquivo de fonte
        return assembly.GetManifestResourceStream($"CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts.{faceName}.ttf");
    }
}
