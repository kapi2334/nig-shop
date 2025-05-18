using clientApp.Models;

namespace clientApp.Services
{
    public class OrderService
    {
        private List<OrderItem> currentOrder = new();
        private bool includeDelivery = false;

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
            includeDelivery = false;
        }

        public bool GetDeliveryOption()
        {
            return includeDelivery;
        }

        public void SetDeliveryOption(bool delivery)
        {
            includeDelivery = delivery;
        }
    }
} 