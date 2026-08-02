public class OrderData
{
    public string Product { get; }
    public int ProductQuantity { get; }
    public string Name { get; }
    public string Address { get; }

    public OrderData( string product, int productQuantity, string name, string address )
    {
        Product = product;
        ProductQuantity = productQuantity;
        Name = name;
        Address = address;
    }
}
