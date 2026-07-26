using System.Security.Cryptography.X509Certificates;

public class OrderManager
{
    private const int deliveryInDays = 3;

    public static OrderData RequestOrderData()
    {
        var order = new OrderData();
        order.Product = RequestProductTitle();
        order.ProductQuantity = RequestProductQuantity();
        order.Name = RequestClientName();
        order.Address = RequestClientAddress();
        return order;
    }

    public static bool TryConfirmOrder( OrderData order )
    {
        Console.WriteLine( $"Здравствуйте, {order.Name}! Вы заказали {order.ProductQuantity} {order.Product} на адрес {order.Address}, все верно?" );
        Console.WriteLine( "Для подтвеждения заказа введите 'Да'. Для изменения и оформления нового заказа введите 'Нет':" );

        while ( true )
        {
            string confirmation = Console.ReadLine() ?? string.Empty;

            if ( confirmation == "Да" )
            {
                MakeOrder( order );
                return true;
            }
            else if ( confirmation == "Нет" )
            {
                Console.WriteLine( $"{order.Name}! Заказ не оформлен! Вы можете оформить новый заказ, повторно заполнив форму:" );
                return false;
            }
            else
            {
                Console.WriteLine( "Введите 'Да' или 'Нет':" );
            }
        }
    }

    private static void MakeOrder( OrderData order )
    {
        Console.WriteLine( $"{order.Name}! Ваш заказ {order.Product} в количестве {order.ProductQuantity} оформлен! Ожидайте доставку по адресу {order.Address} к {DateTime.Now.AddDays( deliveryInDays ).ToString( "dd.MM.yyyy" )}!" );
    }

    private static string CheckEmptyInput()
    {
        while ( true )
        {
            string input = Console.ReadLine() ?? string.Empty;
            if ( !string.IsNullOrWhiteSpace( input ) )
            {
                return input;
            }
            Console.WriteLine( "Поле не должно быть пустым!" );
        }
    }

    private static string CheckValidQuantity()
    {
        while ( true )
        {
            string input = Console.ReadLine() ?? string.Empty;
            if ( int.TryParse( input, out int q ) && q > 0 )
            {
                return input;
            }
            Console.WriteLine( "Введите целое положительное число!" );
        }
    }

    private static string RequestProductTitle()
    {
        Console.WriteLine( "Введите название товара:" );
        var Product = CheckEmptyInput();
        return Product;
    }

    private static string RequestProductQuantity()
    {
        Console.WriteLine( "Введите количество товара:" );
        var ProductQuantity = CheckValidQuantity();
        return ProductQuantity;
    }

    private static string RequestClientName()
    {
        Console.WriteLine( "Введите Ваше имя:" );
        var Name = CheckEmptyInput();
        return Name;
    }

    private static string RequestClientAddress()
    {
        Console.WriteLine( "Введите адрес доставки:" );
        var Address = CheckEmptyInput();
        return Address;
    }
}