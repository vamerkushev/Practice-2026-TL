public class Program
{
    public static void Main()
    {
        while ( true )
        {
            var order = OrderManager.RequestOrderData();
            bool isConfirmed = OrderManager.TryConfirmOrder( order );

            if ( isConfirmed )
            {
                break;
            }
        }
    }
}