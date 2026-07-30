namespace EnterprisePOS.Core
{
    public enum RoomStatus
    {
        Available,
        Occupied,
        Cleaning,
        Maintenance
    }

    public enum TableStatus
    {
        Available,
        Occupied,
        Reserved,
        Cleaning
    }

    public enum OrderType
    {
        DineIn,
        Takeaway,
        Delivery,
        RoomService
    }

    public enum PaymentMethod
    {
        Cash,
        Card,
        BankTransfer,
        SplitPayment,
        Credit,
        RoomCharge
    }

    public enum ItemType
    {
        SalableProduct,
        ConsumableStock,
        RawIngredient
    }

    public enum OrderStatus
    {
        Pending,
        InKitchen,
        Preparing,
        Ready,
        Served,
        Cancelled
    }

    public enum UserRole
    {
        Admin,
        Manager,
        Cashier,
        Waiter,
        KitchenStaff
    }
}

