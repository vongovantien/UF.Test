
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using UF.AssessmentProject.Helper;

namespace UF.AssessmentProject.Model
{
    public class ResponseMessage
    {
        /// <summary>
        ///  Response Result
        /// </summary>
        /// <example>1</example>
        [Required]
        public DataDictionary.responseResult result { get; set; }

        /// <summary>
        /// Error Messages Description
        /// </summary>
        /// <example>Success</example>
        [Required]
        public string resultmessage { get; set; }
    }
}
