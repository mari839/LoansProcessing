using LoanProcessing.Domain.Enums;

namespace LoanProcessing.Domain.Entities;

public class User : Entity<long>
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PersonalId { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Role Role { get; set; } = Role.User;
    public ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();

    public User()
    {
        CreatedDate = DateTime.Now;
        IsDeleted = false;
        IsActive = true;
    }
    public User(string username, string passwordHash, string firstName, string lastName, string personalId, DateTime birthDate, Role role) : this()
    {
        Username = username;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        PersonalId = personalId;
        BirthDate = birthDate;
        Role = role;
    }

    public static User Create(string username, string passwordHash, string firstName, string lastName, string personalId, DateTime birthDate, Role role)
    {
        return new User(username, passwordHash, firstName, lastName, personalId, birthDate, role);
    }
    public void CreateLoanApplication(LoanApplication loanApplication)
    {
        LoanApplications.Add(loanApplication);
    }
}

