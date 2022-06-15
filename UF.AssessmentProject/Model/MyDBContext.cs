using Microsoft.EntityFrameworkCore;
using UF.AssessmentProject.Model.Transaction;

namespace UF.AssessmentProject.Model
{
    public class MyDBContext: DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options)
    : base(options)
        { }
        #region DbSet
        public DbSet<Partner> partners  { get; set; }
        public DbSet<itemdetail> itemdetails { get; set; }

        public DbSet<Order> orders { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
