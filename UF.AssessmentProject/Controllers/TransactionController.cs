
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UF.AssessmentProject.Model;
using UF.AssessmentProject.Services;

namespace UF.AssessmentProject.Controllers
{
    [Produces("application/json"),
        Route("api/submittrxmessage"),
        ApiController]
    [SwaggerTag("Transaction Middleware Controller to keep transactional data in Log Files")]
    public class TransactionController : ControllerBase
    {
        private readonly IMainRepository _repository;
        private readonly ILogger _logger;

        public TransactionController(IMainRepository repository, ILogger<TransactionController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Submit Transaction data
        /// </summary>
        /// <remarks>
        /// Ensure all parameter needed and responded as per IDD
        /// Ensure all posible validation is done
        /// API purpose: To ensure all data is validated and only valid partner with valid signature are able to access to this API
        /// </remarks>
        /// <param name="req">language:en-US(English), ms-MY(BM)</param>  
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "Submit Transaction Message resultfully", typeof(Model.Transaction.ResponseMessage))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized, Request")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Oops! Can't get your Post right now")]
        public async Task<ResponseMessage> SubmitTRansaction(Model.Transaction.RequestMessage req)
        {
            try
            {
                // Check null filed
                var errMess = "";
                if (req.partnerkey == null)
                {
                    errMess += "partnerkey is required, ";
                }
                if (req.partnerrefno == null)
                {
                    errMess += "partnerrefno is required, ";
                }
                DateTime defaultTime = new DateTime();
                if (req.timestamp == null || req.timestamp == defaultTime.ToUniversalTime().ToString("o"))
                {
                    errMess += "timestamp is required, ";
                }
                if (req.sig == null)
                {
                    errMess += "sig is required.";
                }
                if (errMess.Length > 0)
                {
                    return new Model.Transaction.ResponseMessage
                    {
                        result = Helper.DataDictionary.responseResult.failed,
                        resultmessage = errMess.Substring(0),
                    };

                }
                if (req.totalamount < 0)
                {
                    return new Model.Transaction.ResponseMessage
                    {
                        result = Helper.DataDictionary.responseResult.failed,
                        resultmessage = "totalamount only allow positive value",
                    };
                }
                if (req.items != null && req.items.Count > 0)
                {
                    //Check list item detail
                    foreach (var item in req.items)
                    {
                        if (item.name.Length < 0)
                        {
                            return new Model.Transaction.ResponseMessage
                            {
                                result = Helper.DataDictionary.responseResult.failed,
                                resultmessage = "item name cannot be null!",
                            };
                        }


                        if (item.partneritemref.Length < 0)
                        {
                            return new Model.Transaction.ResponseMessage
                            {
                                result = Helper.DataDictionary.responseResult.failed,
                                resultmessage = "partneritemref cannot be null!",
                            };
                        }


                        if (item.unitprice < 0)
                        {
                            return new Model.Transaction.ResponseMessage
                            {
                                result = Helper.DataDictionary.responseResult.failed,
                                resultmessage = "unitprice only allow positive value!",
                            };
                        }

                        //Check quantity
                        if (item.qty > 1 && item.qty <= 5)
                        {
                            return new Model.Transaction.ResponseMessage
                            {
                                result = Helper.DataDictionary.responseResult.failed,
                                resultmessage = "Only allow value to be > 1 and quantity must not exceed 5",
                            };
                        }

                        //check totel item to amountitem
                        double total = 0;
                        total = req.items.Select(s => (s.qty * s.unitprice)).Sum();
                        if (total != req.totalamount)
                        {
                            return new Model.Transaction.ResponseMessage { result = Helper.DataDictionary.responseResult.failed, resultmessage = "Invalid Total Amount" };
                        }

                        //check qty
                        if (item.qty > 0 && item.qty <= 5)
                        {
                            return new Model.Transaction.ResponseMessage
                            {
                                result = Helper.DataDictionary.responseResult.failed,
                                resultmessage = "Only allow value quantity to be > 1 and quantity must not exceed 5",
                            };
                        }
                        Order newOrder = new Order();
                        newOrder.totalamount = req.totalamount;
                        newOrder.timestamp = DateTime.Parse(req.timestamp);

                        foreach (var newItem in req.items)
                        {
                            await _repository.PostItemDetail(newItem);
                        }

                        await _repository.PostOrder(newOrder);
                    }
                    //Check partner in system
                    var dawSig = DateTime.Parse(req.timestamp).ToString("yyyyMMddHHmmss") + req.partnerkey + req.partnerrefno + req.totalamount + req.partnerpassword;
                    var sig = Helper.ShareHepper.ComputeSha256Hash(Helper.ShareHepper.Base64Encode(dawSig));
                    var decodePassword = Helper.ShareHepper.Base64Decode(req.partnerpassword);
                    var existPartner = await _repository.checkPartner(req.partnerkey, decodePassword);
                    if (existPartner == null || (existPartner != null && existPartner.partnerpassword != decodePassword) || sig != req.sig)
                    {
                        return new ResponseMessage { result = Helper.DataDictionary.responseResult.failed, resultmessage = "Access Denied!" };
                    }

                    //Check expire for api
                    if (DateTime.Now <= DateTime.Parse(req.timestamp).AddMinutes(5))
                        return new Model.Transaction.ResponseMessage
                        {
                            result = Helper.DataDictionary.responseResult.failed,
                            resultmessage = "Expired!"
                        };
                }
                _logger.LogInformation("Info");
                return new ResponseMessage { result = Helper.DataDictionary.responseResult.success, resultmessage = "Request data is valid." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ResponseMessage { result = Helper.DataDictionary.responseResult.failed, resultmessage = "Unexpected Error!" };
            }
        }

        /// <summary>
        /// Test this controller is active
        /// </summary>
        /// <remarks>
        /// Test API to check API is Alive or not
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> TestAPI()
        {
            string result = "Hello World!";
            return Ok(result);
        }
    }
}
