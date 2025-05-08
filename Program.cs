using System;

namespace Publisher_Subscriber_Pattern
{
    public class OrderEventArgs : EventArgs
    {
       public string OrderName { get; }
        public string OrderType { get; }
        public DateTime OrderDate { get; }
        public OrderEventArgs(string orderName, string orderType, DateTime orderDate)
        {
            OrderName = orderName;
            OrderType = orderType;
            OrderDate = orderDate;
        }
    }
    
    public class  Order
    {
        public event EventHandler<OrderEventArgs> OrderPlaced;

        public void PlaceOrder(string orderName, string orderType)
        {
            OrderEventArgs args = new OrderEventArgs(orderName, orderType, DateTime.Now);
            OnOrderPlaced(args);
        }

        protected virtual void OnOrderPlaced(OrderEventArgs e)
        {
            OrderPlaced?.Invoke(this, e);
        }
    }

    public class EmailService
    {
        public void Subscribe(Order order)
        {
            order.OrderPlaced += OnOrderPlaced;
        }

        private void SendSms(OrderEventArgs e)
        {
            Console.WriteLine($"Email sent for order: {e.OrderName}, Type: {e.OrderType}, Date: {e.OrderDate}");
        }

        public void OnOrderPlaced(object sender, OrderEventArgs e)
        {
            SendSms(e);
        }

        public void Unsubscribe(Order order)
        {
            order.OrderPlaced -= OnOrderPlaced;
        }
    }

    public class SmsService
    {
        public void Subscribe(Order order)
        {
            order.OrderPlaced += OnOrderPlaced;
        }
        private void SendEmail(OrderEventArgs e)
        {
            Console.WriteLine($"SMS sent for order: {e.OrderName}, Type: {e.OrderType}, Date: {e.OrderDate}");
        }

        public void OnOrderPlaced(object sender, OrderEventArgs e)
        {
            SendEmail(e);
        }

        public void Unsubscribe(Order order)
        {
            order.OrderPlaced -= OnOrderPlaced;
        }
    }


    public class Program
    {
        static void Main(string[] args)
        {

            EmailService emailService = new EmailService();
            SmsService smsService = new SmsService();

            Order order = new Order();
            emailService.Subscribe(order);
            smsService.Subscribe(order);

            order.PlaceOrder("Order1", "TypeA");
            order.PlaceOrder("Order2", "TypeB");
            order.PlaceOrder("Order3", "TypeC");

            emailService.Unsubscribe(order);
            smsService.Unsubscribe(order);

        }

      

    }
}
