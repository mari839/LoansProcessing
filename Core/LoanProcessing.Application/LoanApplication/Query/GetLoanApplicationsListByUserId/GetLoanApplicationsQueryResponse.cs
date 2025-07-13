using LoanProcessing.Domain.Enums;

namespace LoanProcessing.Application.LoanApplication.Query.GetLoanApplicationsList;
public class GetLoanApplicationsQueryResponse
{
    public List<AppList> apps {  get; set; }
}

public class AppList
{
    public long Id { get; set; }
    public LoanType LoanType { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public int Period { get; set; }
    public LoanApplicaitonStatus Status { get; set; }
}
