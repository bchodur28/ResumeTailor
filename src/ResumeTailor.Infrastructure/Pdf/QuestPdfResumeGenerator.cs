using ResumeTailor.Application.Pdf.Interfaces;
using System.Reflection.Metadata;

namespace ResumeTailor.Infrastructure.Pdf;

public class QuestPdfResumeGenerator : IResumePdfGenerator
{
    public byte[] Generate()
    {
        //return Document.Create(container => {
        //    container.Page(page =>
        //    {
        //        page.Margin(40);

        //        page.Content().Column(Column => {
        //            column.Item();
        //        });
        //    });
        //});
        throw new NotImplementedException();
    }

    public void SaveAsFile()
    {
        throw new NotImplementedException();
    }
}
