using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UF.AssessmentProject.Controllers;
using UF.AssessmentProject.logs;
using UF.AssessmentProject.Model;
using UF.AssessmentProject.Services;
using Xunit;

namespace TestAPI
{
    public class TestController
    {
        private readonly IMainRepository _repository;
        private readonly TransactionController _controller;
        private readonly ILoggerManager _logger;
        public TestController()
        {
            _controller = new TransactionController(_repository, _logger);
        }


        [Fact]
        public void Test_Access()
        {
        var item = new Mock<TransactionController>();
            //Arrage
            var rq = new UF.AssessmentProject.Model.Transaction.RequestMessage()
            {
                partnerkey = "aaa",
                partnerrefno = "FG-00001",
                partnerpassword = "RkFLRVBBU1NXT1JEMTIzNA==",
                totalamount = 110,
                items = new List<UF.AssessmentProject.Model.Transaction.itemdetail>()
                {
                    new UF.AssessmentProject.Model.Transaction.itemdetail()
                    {
                        name = "i-00001",
                        partneritemref = "Pen",
                        qty = 5,
                        unitprice = 2
                    },
                    new UF.AssessmentProject.Model.Transaction.itemdetail()
                    {
                        name = "i-00002",
                        partneritemref = "Ruler",
                        qty = 1,
                        unitprice = 100
                    }
                }
            };
            rq.timestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            var rawsign = DateTime.Parse(rq.timestamp).ToString("yyyyMMddHHmmss") + rq.partnerkey + rq.partnerrefno + rq.totalamount + rq.partnerpassword;
            string sig = Helpper.Share.ComputeSha256Hash((Helpper.Share.Base64Encode(rawsign)));
            rq.sig = sig;

            //Assert
            Task<ResponseMessage> rs = _controller.SubmitTRansaction(rq);
            Assert.True(rs.IsCompleted);
        }

        [Fact]
        public void Expired()
        {
            var rq = new UF.AssessmentProject.Model.Transaction.RequestMessage()
            {
                partnerkey = "FAKEGOOGLE",
                partnerrefno = "FG-00001",
                partnerpassword = "RkFLRVBBU1NXT1JEMTIzNA==",
                sig = "24XYSmvKGH9I9Y5FLvSsId2MPtjkvog7U5JLhE3m30A=",
                timestamp = "2013-11-22T02:11:22.0000000Z",
                totalamount = 110,
                items = new List<UF.AssessmentProject.Model.Transaction.itemdetail>()
                {
                    new UF.AssessmentProject.Model.Transaction.itemdetail()
                    {
                        name = "i-00001",
                        partneritemref = "Pen",
                        qty = 5,
                        unitprice = 2
                    },
                    new UF.AssessmentProject.Model.Transaction.itemdetail()
                    {
                        name = "i-00002",
                        partneritemref = "Ruler",
                        qty = 1,
                        unitprice = 100
                    }
                }
            };
            Task<ResponseMessage> rs = _controller.SubmitTRansaction(rq);
            Assert.True(rs.IsCompleted);
        }

        [Fact]
        public void Partnerkey_isRequired()
        {

            var rq = new UF.AssessmentProject.Model.Transaction.RequestMessage()
            {
                partnerrefno = "FG-00001",
                partnerpassword = "RkFLRVBBU1NXT1JEMTIzNA==",
                totalamount = 110,
                items = new List<UF.AssessmentProject.Model.Transaction.itemdetail>()
                {
                    new UF.AssessmentProject.Model.Transaction.itemdetail()
                    {
                        name = "i-00001",
                        partneritemref = "Pen",
                        qty = 5,
                        unitprice = 2
                    },
                    new UF.AssessmentProject.Model.Transaction.itemdetail()
                    {
                        name = "i-00002",
                        partneritemref = "Ruler",
                        qty = 1,
                        unitprice = 100
                    }
                }
            };
            rq.timestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            var rawsign = DateTime.Parse(rq.timestamp).ToString("yyyyMMddHHmmss") + rq.partnerkey + rq.partnerrefno + rq.totalamount + rq.partnerpassword;
            string sig = Helpper.Share.ComputeSha256Hash((Helpper.Share.Base64Encode(rawsign)));
            rq.sig = sig;

            Task<ResponseMessage> rs = _controller.SubmitTRansaction(rq);
            Assert.True(rs.IsCompleted);

        }

        [Fact]
        public void Invalid_total()
        {

            var rq = new UF.AssessmentProject.Model.Transaction.RequestMessage()
            {
                partnerkey = "FAKEGOOGLE",
                partnerrefno = "FG-00001",
                partnerpassword = "RkFLRVBBU1NXT1JEMTIzNA==",
                totalamount = 9999,
                items = new List<UF.AssessmentProject.Model.Transaction.itemdetail>()
                {
                    new UF.AssessmentProject.Model.Transaction.itemdetail()
                    {
                        name = "i-00001",
                        partneritemref = "Pen",
                        qty = 5,
                        unitprice = 2
                    },
                    new UF.AssessmentProject.Model.Transaction.itemdetail()
                    {
                        name = "i-00002",
                        partneritemref = "Ruler",
                        qty = 1,
                        unitprice = 100
                    }
                }
            };
            rq.timestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            var rawsign = DateTime.Parse(rq.timestamp).ToString("yyyyMMddHHmmss") + rq.partnerkey + rq.partnerrefno + rq.totalamount + rq.partnerpassword;
            string sig = Helpper.Share.ComputeSha256Hash((Helpper.Share.Base64Encode(rawsign)));
            rq.sig = sig;

            Task<ResponseMessage> rs = _controller.SubmitTRansaction(rq);


            Assert.True(rs.IsCompleted);
        }

        [Fact]
        public void Unitprice_positive()
        {

            var rq = new UF.AssessmentProject.Model.Transaction.RequestMessage()
            {
                partnerkey = "FAKEGOOGLE",
                partnerrefno = "FG-00001",
                partnerpassword = "RkFLRVBBU1NXT1JEMTIzNA==",
                totalamount = 110,
                items = new List<UF.AssessmentProject.Model.Transaction.itemdetail>()
                {
                    new UF.AssessmentProject.Model.Transaction.itemdetail()
                    {
                        name = "i-00001",
                        partneritemref = "Pen",
                        qty = 5,
                        unitprice = -10
                    },
                    new UF.AssessmentProject.Model.Transaction.itemdetail()
                    {
                        name = "i-00002",
                        partneritemref = "Ruler",
                        qty = 1,
                        unitprice = -100
                    }
                }
            };
            rq.timestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            var rawsign = DateTime.Parse(rq.timestamp).ToString("yyyyMMddHHmmss") + rq.partnerkey + rq.partnerrefno + rq.totalamount + rq.partnerpassword;
            string sig = Helpper.Share.ComputeSha256Hash((Helpper.Share.Base64Encode(rawsign)));
            rq.sig = sig;

            Task<ResponseMessage> rs = _controller.SubmitTRansaction(rq);
            Assert.True(rs.IsCompleted);
        }
    }
}
