using LoanProcessing.Domain.Enums;

public class GetLoanApplicationsListQueryResponse
{
    public long Id { get; set; }
    public LoanType LoanType { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public int Period { get; set; }
    public LoanApplicaitonStatus Status { get; set; }
}
