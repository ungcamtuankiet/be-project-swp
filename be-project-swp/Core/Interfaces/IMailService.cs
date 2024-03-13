using System.Threading.Tasks;
namespace be_project_swp.Core.Interfaces
{
    
    public interface IMailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
