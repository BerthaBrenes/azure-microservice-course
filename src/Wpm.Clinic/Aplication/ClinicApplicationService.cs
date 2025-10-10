using Wpm.Clinic.Controllers;
using Wpm.Clinic.DataAccess;
using Wpm.Clinic.ExternalServices;

namespace Wpm.Clinic.Aplication;
public class ClinicApplicationService(ClinicDBContext dbContext, ManagementService managementService)
{
    public async Task<Consultation> Handle(StartConsultationCommand command)
    {
        var petInfo = await managementService.GetPetInfo(command.PatiendID);

        var newConsultation = new Consultation(Guid.NewGuid(), command.PatiendID, petInfo.Name, petInfo.Age, DateTime.UtcNow);
        await dbContext.Consultations.AddAsync(newConsultation);
        await dbContext.SaveChangesAsync();

        return newConsultation;
    }
}

