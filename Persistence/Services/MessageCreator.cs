using Domain.Entities;
using Domain.IServices;

namespace Persistence.Services
{
    public class MessageCreator : IMessageCreator
    {
        public string MessageCreate(Order order, Doctor doctor)
        {
            return $"You create{order.StartDate}";
        }

        public string MessageEdit(Order order, Doctor doctor)
        {
            return $"You edit{order.StartDate}";
        }

        public string MessageDelete(Order order)
        {
            return $"You delete{order.StartDate}";
        }

        public string MessageToDeleteDoctorByAdmin(Order order, Doctor doctor)
        {
            return $"You delete order{order.StartDate}";
        }
    }
}
