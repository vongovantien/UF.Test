using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UF.AssessmentProject.Model.Transaction;

namespace UF.AssessmentProject.Model
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public long totalamount { get; set; }

        public ICollection<itemdetail> items { get; set; }

        public DateTime timestamp { get; set; }
    }
}
