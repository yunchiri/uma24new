using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMA24.Model
{
    class CalcModel
    {
        int employeeID;

        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }
        DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public void Init(int cID, DateTime sDate, DateTime eDate)
        {
            this.employeeID = cID;
            this.startDate = sDate;
            this.endDate = eDate;

        }
    }
}
