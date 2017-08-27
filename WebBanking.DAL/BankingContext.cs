using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public partial class BankingContext : DbContext
    {
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountOrder> AccountOrder { get; set; }
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<CreditCard> CreditCard { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DebitCard> DebitCard { get; set; }
        public virtual DbSet<LinkedProducts> LinkedProducts { get; set; }
        public virtual DbSet<Loan> Loan { get; set; }
        public virtual DbSet<OrganizationOrder> OrganizationOrder { get; set; }
        public virtual DbSet<PrepaidCard> PrepaidCard { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
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
                entity.HasKey(e => e.Iban)
                    .HasName("PK_Accounts");

                entity.Property(e => e.Iban)
                    .HasColumnName("IBAN")
                    .HasColumnType("varchar(27)");

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

            modelBuilder.Entity<AccountOrder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(15)");

                entity.Property(e => e.Amount).HasColumnType("decimal");

                entity.Property(e => e.Comments).HasColumnType("text");

                entity.Property(e => e.CreditAccount)
                    .IsRequired()
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.DebitAccount)
                    .IsRequired()
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.ExecutionFrequency)
                    .IsRequired()
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.ExecutionsLeft).HasColumnType("decimal");

                entity.Property(e => e.NextExecutionDate).HasColumnType("date");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(3)");

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

                entity.Property(e => e.AvailableLimit).HasColumnType("decimal");

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

                entity.Property(e => e.AvailableLimit).HasColumnType("decimal");

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

            modelBuilder.Entity<OrganizationOrder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(15)");

                entity.Property(e => e.CreditAccount)
                    .IsRequired()
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.MaxCreditAmount).HasColumnType("decimal");

                entity.Property(e => e.Organization)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PrepaidCard>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("varchar(16)");

                entity.Property(e => e.AvailableLimit).HasColumnType("decimal");

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
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal");

                entity.Property(e => e.Beneficiary)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Details)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LedgerBalance).HasColumnType("decimal");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasColumnType("varchar(27)");

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnType("varchar(6)");
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