using Domain.Entities;

namespace Domain.IServices
{
    public interface IMessageCreator
    {
        public string MessageCreate(Order order, Doctor doctor);
        public string MessageEdit(Order order, Doctor doctor);
        public string MessageDelete(Order order);
        public string MessageToDeleteDoctorByAdmin(Order order, Doctor doctor);
    }
}
