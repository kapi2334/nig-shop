using clientApp.Models;

namespace clientApp.Services
{
    public class OrderService
    {
        private List<OrderItem> currentOrder = new();

        public List<OrderItem> GetCurrentOrder()
        {
            return currentOrder;
        }

        public void SetCurrentOrder(List<OrderItem> order)
        {
            currentOrder = order;
        }

        public void ClearOrder()
        {
            currentOrder.Clear();
        }
    }
} 