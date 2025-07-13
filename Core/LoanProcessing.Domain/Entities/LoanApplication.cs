using LoanProcessing.Domain.Enums;

namespace LoanProcessing.Domain.Entities
{
    public class LoanApplication : Entity<long>
    {
        public User User { get; set; } = default!;
        public LoanType LoanType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int Period { get; set; }
        public LoanApplicaitonStatus Status { get; set; } = LoanApplicaitonStatus.InProcess;
        public long UserId { get; set; }
        public LoanApplication()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public void Delete()
        {
            IsDeleted = true;
            IsActive = false;
        }
        public void Approve()
        {
            if (Status != LoanApplicaitonStatus.Submitted)
                throw new InvalidOperationException("Only applications Submitted can be approved.");

            Status = LoanApplicaitonStatus.Approved;
        }

        public void Reject()
        {
            if (Status != LoanApplicaitonStatus.Submitted)
                throw new InvalidOperationException("Only applications Submitted can be rejected.");

            Status = LoanApplicaitonStatus.Rejected;
        }

        public void Edit(decimal amount, string currency, int period, LoanType loanType)
        {
            if (Status != LoanApplicaitonStatus.InProcess)
                throw new InvalidOperationException("Only in-process applications can be edited.");

            Amount = amount;
            Currency = currency;
            Period = period;
            LoanType = loanType;
        }
        public void Submit()
        {
            if (Status != LoanApplicaitonStatus.InProcess)
                throw new InvalidOperationException("Only in-process applications can be submitted.");

            Status = LoanApplicaitonStatus.Submitted;
        }

    }
}
