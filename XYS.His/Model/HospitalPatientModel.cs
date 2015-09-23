using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYS.Model;

namespace XYS.His.Model
{
    public class HospitalPatientModel:PatientModel
    {
        private string _visitID;
        private string _inpNo;
        public string VisitID 
        {
            get { return this._visitID; }
        }
        public string InpNo
        {
            get { return this._inpNo; }
        }
    }
}
