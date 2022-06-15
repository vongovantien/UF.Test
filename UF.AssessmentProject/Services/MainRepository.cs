using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UF.AssessmentProject.Model;
using UF.AssessmentProject.Model.Transaction;

namespace UF.AssessmentProject.Services
{
    public class MainRepository : IMainRepository
    {

        private readonly MyDBContext _context;
        public MainRepository(MyDBContext context)
        {
            _context = context;
        }

        public async Task<Partner> checkPartner(string partnerkey, string partnerpassword)
        {
            return await _context.partners.Where(s => s.partnerkey == partnerkey.Trim() && s.partnerpassword == partnerpassword.Trim()).FirstOrDefaultAsync();
        }
        public async Task<itemdetail> PostItemDetail(itemdetail newItem)
        {
            try
            {
                _context.itemdetails.Add(newItem);
                await _context.SaveChangesAsync();
                return newItem;
            }
            catch (Exception ex)
            {
                return null;
            }
          }
        public async Task<Order> PostOrder(Order newItem)
        {
            try
            {
                _context.orders.Add(newItem);
                await _context.SaveChangesAsync();
                return newItem;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}