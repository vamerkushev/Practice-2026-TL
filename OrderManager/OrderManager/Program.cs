public class Program
{
    public static void Main()
    {
        bool orderConfirmed = false;
        while ( !orderConfirmed )
        {
            OrderData order = OrderManager.RequestOrderData();
            orderConfirmed = OrderManager.TryConfirmOrder( order );
        }
    }
}
