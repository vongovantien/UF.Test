using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UF.AssessmentProject.Model.Transaction
{
    public class RequestMessage : Model.RequestMessage
    {
        public string partnerkey { get; set; }
        public string partnerrefno { get; set; }
        public long totalamount { get; set; }
        public string partnerpassword { get; set; }
        public ICollection<itemdetail> items { get; set; } 
    }

    public class itemdetail { 
        [Key]
        public string partneritemref { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public long unitprice { get; set; }

    }

    public class ResponseMessage: Model.ResponseMessage
    {
    }
}
