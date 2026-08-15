using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Application.Pdf.Interfaces;

public interface IResumePdfGenerator
{
    byte[] Generate();
    void SaveAsFile();
}
