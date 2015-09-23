using System;
using System.Collections.Generic;
using XYS.Model;
using XYS;
using XYS.Common;

namespace XYS.His
{
    public delegate void PatientInfoQueryHandler(object sender,PatientInfoQueryEventArgs e);
    public class PatientInfoQueryEventArgs:EventArgs
    {

    }
    public interface IPatientSearcher : ISearch
    {
        List<IResultModel> getPatientInfo(SearchArgument argument);
        event PatientInfoQueryHandler PatientInfoQueryEvent;
    }
}
