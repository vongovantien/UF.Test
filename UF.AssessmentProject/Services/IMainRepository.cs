using System.Threading.Tasks;
using UF.AssessmentProject.Model;
using UF.AssessmentProject.Model.Transaction;

namespace UF.AssessmentProject.Services
{
    public interface IMainRepository
    {
        Task<Partner> checkPartner(string partnerkey,string partnerpassword);
        Task<itemdetail> PostItemDetail(itemdetail newItem);
        Task<Order> PostOrder(Order newItem);
    }
}
