using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UF.AssessmentProject.Helper;

namespace UF.AssessmentProject.Model
{
    public class RequestMessage
    {
        public string sig { get; set; } 

        /// <summary>
        /// Time in the ISO format. ie. 2014-04-14T12:34:23.00+0800 Preferdably set using RealTimeStamp instead.
        /// </summary>
        /// <example>2020-07-28T12:34:23.00+0800</example>
        /// <returns></returns> 
        [Required]
        public string timestamp
        {
            get { return _realTimeStamp.ToUniversalTime().ToString("o"); }
            set { _realTimeStamp = DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind); }
        }

        private DateTime _realTimeStamp;
        /// <summary>
        /// When this field is set, automatically converts to the string 
        /// for the timestamp property
        /// </summary> 
        /// <remarks>Internal use</remarks>
        [Newtonsoft.Json.JsonIgnore()]
        [System.Text.Json.Serialization.JsonIgnore()]
        public DateTime RealTimeStamp
        {
            get { return _realTimeStamp; }
        }

    }
     
}
