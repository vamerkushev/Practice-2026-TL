public class Program
{
    public static void Main()
    {
        bool orderConfirmed = false;
        while ( !orderConfirmed )
        {
            var order = OrderManager.RequestOrderData();
            orderConfirmed = OrderManager.TryConfirmOrder( order );
        }
    }
}
