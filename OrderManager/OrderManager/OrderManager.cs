public class OrderManager
{
    static string RequestAndGetProduct()
    {
        Console.WriteLine( "Введите название товара:" );
        string product = Console.ReadLine() ?? string.Empty;
        return product;
    }

    static string RequestAndGetProductQuantity()
    {
        Console.WriteLine( "Введите количество товара:" );
        string productQuantity = Console.ReadLine() ?? string.Empty;
        return productQuantity;
    }

    static string RequestAndGetName()
    {
        Console.WriteLine( "Введите Ваше имя:" );
        string name = Console.ReadLine() ?? string.Empty;
        return name;
    }

    static string RequestAndGetAddress()
    {
        Console.WriteLine( "Введите адрес доставки:" );
        string address = Console.ReadLine() ?? string.Empty;
        return address;
    }

    public static void RequestingAndGetData( out string product, out string productQuantity, out string name, out string address )
    {
        product = RequestAndGetProduct();
        productQuantity = RequestAndGetProductQuantity();
        name = RequestAndGetName();
        address = RequestAndGetAddress();
    }

    public static bool OrderConfirmation( string product, string productQuantity, string name, string address, DateTime now )
    {
        Console.WriteLine( $"Здравствуйте, {name}! Вы заказали {productQuantity} {product} на адрес {address}, все верно?" );
        Console.WriteLine( "Для подтвеждения заказа введите 'Да'. Для изменения и оформления нового заказа введите 'Нет':" );

        while ( true )
        {
            string confirmation = Console.ReadLine() ?? string.Empty;

            if ( confirmation == "Да" )
            {
                Console.WriteLine( $"{name}! Ваш заказ {product} в количестве {productQuantity} оформлен! Ожидайте доставку по адресу {address} к {now.AddDays( 3 ).ToString( "dd.MM.yyyy" )}!" );
                return true;
            }
            else if ( confirmation == "Нет" )
            {
                Console.WriteLine( $"{name}! Заказ не оформлен! Вы можете оформить новый заказ, повторно заполнив форму:" );
                return false;
            }
            else
            {
                Console.WriteLine( "Введите 'Да' или 'Нет':" );
            }
        }
    }
}

public class MainProgram
{
    public static void Main()
    {
        while ( true )
        {
            string product, productQuantity, name, address;
            DateTime now = DateTime.Now;
            OrderManager.RequestingAndGetData( out product, out productQuantity, out name, out address );
            bool getConfirmation = OrderManager.OrderConfirmation( product, productQuantity, name, address, now );

            if ( getConfirmation )
            {
                break;
            }
        }
    }
}