using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace PagoPark.Tools;

public class ThemeReport
{
    public ThemeReport()
    {
        // code in your main method
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                // page content
            });
        });

        // instead of the standard way of generating a PDF file
        document.GeneratePdf("hello.pdf");

        // use the following invocation
        document.ShowInPreviewer();

        // optionally, you can specify an HTTP port to communicate with the previewer host (default is 12500)
        document.ShowInPreviewer(12345);
    }
}
