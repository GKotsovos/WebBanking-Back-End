using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public partial class BankingContext : DbContext
    {
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<CreditCard> CreditCard { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DebitCard> DebitCard { get; set; }
        public virtual DbSet<LinkedProducts> LinkedProducts { get; set; }
        public virtual DbSet<Loan> Loan { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }
        public virtual DbSet<PaymentOrder> PaymentOrder { get; set; }
        public virtual DbSet<PrepaidCard> PrepaidCard { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransferOrder> TransferOrder { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-JQNLA1D\SQLEXPRESS;Database=Banking;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(27)");

                entity.Property(e => e.AvailableBalance).HasColumnType("decimal");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.LastMovementDate).HasColumnType("date");

                entity.Property(e => e.LedgerBalance).HasColumnType("decimal");

                entity.Property(e => e.Overdraft).HasColumnType("decimal");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(3)");

                entity.Property(e => e.Bic)
                    .IsRequired()
                    .HasColumnName("BIC")
                    .HasColumnType("varchar(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CreditCard>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(16)");

                entity.Property(e => e.AvailableBalance).HasColumnType("decimal");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Debt).HasColumnType("decimal");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.IssueDate).HasColumnType("date");

                entity.Property(e => e.LedgerBalance).HasColumnType("decimal");

                entity.Property(e => e.Limit).HasColumnType("decimal");

                entity.Property(e => e.NextInstallmentAmount).HasColumnType("decimal");

                entity.Property(e => e.NextInstallmentDate).HasColumnType("date");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(10)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DebitCard>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(16)");

                entity.Property(e => e.AvailableBalance).HasColumnType("decimal");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.DailyLimit).HasColumnType("decimal");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.IssueDate).HasColumnType("date");

                entity.Property(e => e.LedgerBalance).HasColumnType("decimal");
            });

            modelBuilder.Entity<LinkedProducts>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.CardId)
                    .IsRequired()
                    .HasColumnType("varchar(16)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasColumnType("varchar(27)");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(15)");

                entity.Property(e => e.AvailableBalance).HasColumnType("decimal");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomTitle).HasMaxLength(300);

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(11)");

                entity.Property(e => e.Debt).HasColumnType("decimal");

                entity.Property(e => e.IssuedDate).HasColumnType("date");

                entity.Property(e => e.LedgerBalance)
                    .HasColumnType("decimal")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LoanedAmount).HasColumnType("decimal");

                entity.Property(e => e.NextInstallmentAmount).HasColumnType("decimal");

                entity.Property(e => e.NextInstallmentDate).HasColumnType("date");

                entity.Property(e => e.RepaymentBalance)
                    .HasColumnType("decimal")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RepaymentDate).HasColumnType("date");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SubCategory).HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentOrder>(entity =>
            {
                entity.Property(e => e.Charges)
                    .HasColumnType("decimal")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.DebitProductId)
                    .IsRequired()
                    .HasColumnType("varchar(27)");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.MaxPaymentAmount).HasColumnType("decimal");

                entity.Property(e => e.PaymentCode)
                    .IsRequired()
                    .HasMaxLength(27);

                entity.Property(e => e.PaymentMethod)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PreviousExecutionDate).HasColumnType("date");
            });

            modelBuilder.Entity<PrepaidCard>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(16)");

                entity.Property(e => e.AvailableBalance).HasColumnType("decimal");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.IssueDate).HasColumnType("date");

                entity.Property(e => e.LedgerBalance).HasColumnType("decimal");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal");

                entity.Property(e => e.Bank)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Beneficiary)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Comments).HasColumnType("text");

                entity.Property(e => e.CreditProductId)
                    .IsRequired()
                    .HasColumnType("varchar(27)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DebitProductId)
                    .IsRequired()
                    .HasColumnType("varchar(27)");

                entity.Property(e => e.NewBalance).HasColumnType("decimal");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnType("varchar(6)");
            });

            modelBuilder.Entity<TransferOrder>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal");

                entity.Property(e => e.Charges).HasColumnType("decimal");

                entity.Property(e => e.Comments).HasColumnType("text");

                entity.Property(e => e.CreditProductId)
                    .IsRequired()
                    .HasColumnType("varchar(27)");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomTitle)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.DebitProductId)
                    .IsRequired()
                    .HasColumnType("varchar(27)");

                entity.Property(e => e.NextExecutionDate).HasColumnType("date");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(6)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnName("CustomerID")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(60)");
            });
        }
    }
}