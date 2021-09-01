using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace locationRecordeapi
{

    public class Emps_Locs_View
    {
        public int empId { set; get; }
        public string empCode { set; get; }

        public string empemail { set; get; }
        public  string empName { set; get; }
        public string empPhone { set; get; }
        public string? ELocaddress { set; get; }
        public double? LocLatitude { set; get; }
        public double? locLngtude { set; get; }
        public int? LOCID { set; get; }
        public bool entering { set; get; }
        public Int64 totalHours { set; get; }


    }
}
