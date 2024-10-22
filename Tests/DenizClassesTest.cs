using BYT_Assignment_3;

[TestFixture]
public class DenizClassesTests
{
    private const string TestFilePath = "testData.json";

    [SetUp]
    public void SetUp()
    {
        Order.Orders.Clear();
        OrderItem.OrderItems.Clear();
        MenuItem.MenuItems.Clear();
        Ingredient.Ingredients.Clear();
        Inventory.Inventories.Clear();
        Table.Tables.Clear();
    }

    [Test]
    public void CreateMenuItem_WithValidData_ShouldAddToMenuItemList()
    {
        var menuItem = new MenuItem(1, "Pizza", "Delicious cheese pizza", 12.99, true, "Main Course", 15, 800, 0);

        Assert.That(MenuItem.MenuItems.Count, Is.EqualTo(1));
        Assert.That(menuItem.Name, Is.EqualTo("Pizza"));
        Assert.That(menuItem.Price, Is.EqualTo(12.99));
    }

    [Test]
    public void CreateMenuItem_WithInvalidPrice_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new MenuItem(1, "Pizza", "Delicious cheese pizza", -5.0, true, "Main Course", 15, 800, 0));
    }

    [Test]
    public void UpdateMenuItemPrice_WithValidPrice_ShouldUpdatePrice()
    {
        var menuItem = new MenuItem(1, "Pizza", "Delicious cheese pizza", 12.99, true, "Main Course", 15, 800, 0);
        menuItem.UpdatePrice(14.99);

        Assert.That(menuItem.Price, Is.EqualTo(14.99));
    }

    [Test]
    public void UpdateMenuItemPrice_WithInvalidPrice_ShouldThrowArgumentException()
    {
        var menuItem = new MenuItem(1, "Pizza", "Delicious cheese pizza", 12.99, true, "Main Course", 15, 800, 0);

        Assert.Throws<ArgumentException>(() => menuItem.UpdatePrice(-10.0));
    }

    [Test]
    public void CreateIngredient_WithValidData_ShouldAddToIngredientList()
    {
        var ingredient = new Ingredient(1, "Cheese", 100, "kg", true);

        Assert.That(Ingredient.Ingredients.Count, Is.EqualTo(1));
        Assert.That(ingredient.Name, Is.EqualTo("Cheese"));
        Assert.That(ingredient.QuantityInStock, Is.EqualTo(100));
    }

    [Test]
    public void AddStock_WithValidAmount_ShouldIncreaseQuantity()
    {
        var ingredient = new Ingredient(1, "Cheese", 100, "kg", true);
        ingredient.AddStock(50);

        Assert.That(ingredient.QuantityInStock, Is.EqualTo(150));
    }

    [Test]
    public void AddStock_WithInvalidAmount_ShouldThrowArgumentException()
    {
        var ingredient = new Ingredient(1, "Cheese", 100, "kg", true);

        Assert.Throws<ArgumentException>(() => ingredient.AddStock(-20));
    }

    [Test]
    public void RemoveStock_WithValidAmount_ShouldDecreaseQuantity()
    {
        var ingredient = new Ingredient(1, "Cheese", 100, "kg", true);
        ingredient.RemoveStock(30);

        Assert.That(ingredient.QuantityInStock, Is.EqualTo(70));
    }

    [Test]
    public void RemoveStock_WithInvalidAmount_ShouldThrowException()
    {
        var ingredient = new Ingredient(1, "Cheese", 100, "kg", true);

        Assert.Throws<InvalidOperationException>(() => ingredient.RemoveStock(150));
    }

    [Test]
    public void CreateOrderItem_WithValidData_ShouldAddToOrderItemList()
    {
        var menuItem = new MenuItem(1, "Pizza", "Delicious cheese pizza", 12.99, true, "Main Course", 15, 800, 0);
        var orderItem = new OrderItem(1, menuItem, 2, "Extra cheese");

        Assert.That(OrderItem.OrderItems.Count, Is.EqualTo(1));
        Assert.That(orderItem.Quantity, Is.EqualTo(2));
        Assert.That(orderItem.TotalPrice, Is.EqualTo(25.98));
    }

    [Test]
    public void CreateOrderItem_WithInvalidQuantity_ShouldThrowArgumentException()
    {
        var menuItem = new MenuItem(1, "Pizza", "Delicious cheese pizza", 12.99, true, "Main Course", 15, 800, 0);

        Assert.Throws<ArgumentException>(() => new OrderItem(1, menuItem, 0, "Extra cheese"));
    }

    [Test]
    public void CreateOrder_WithValidData_ShouldAddToOrderList()
    {
        var table = new Table(1, 4);
        var order = new Order(1, table, "Pending");

        Assert.That(Order.Orders.Count, Is.EqualTo(1));
        Assert.That(order.Status, Is.EqualTo("Pending"));
    }

    [Test]
    public void AddOrderItem_ToOrder_ShouldIncreaseOrderItemsCount()
    {
        var table = new Table(1, 4);
        var order = new Order(1, table, "Pending");

        var menuItem = new MenuItem(1, "Pizza", "Delicious cheese pizza", 10.0, true, "Main Course", 15, 800, 0);
        var orderItem = new OrderItem(1, menuItem, 2, "");

        order.AddOrderItem(orderItem);

        Assert.That(order.OrderItems.Count, Is.EqualTo(1));
        Assert.That(order.TotalAmount, Is.EqualTo(20.0));
    }

    [Test]
    public void TotalAmount_WithMultipleOrderItems_ShouldReturnCorrectSum()
    {
        var table = new Table(1, 4);
        var order = new Order(1, table, "Pending");

        var menuItem1 = new MenuItem(1, "Pizza", "Delicious cheese pizza", 10.0, true, "Main Course", 15, 800, 0);
        var menuItem2 = new MenuItem(2, "Pasta", "Creamy pasta", 8.0, true, "Main Course", 10, 600, 0);

        var orderItem1 = new OrderItem(1, menuItem1, 2, "");
        var orderItem2 = new OrderItem(2, menuItem2, 1, "");

        order.AddOrderItem(orderItem1);
        order.AddOrderItem(orderItem2);

        Assert.That(order.TotalAmount, Is.EqualTo(28.0));
    }

    [Test]
    public void CreateTable_WithValidData_ShouldAddToTableList()
    {
        var table = new Table(1, 4);

        Assert.That(Table.Tables.Count, Is.EqualTo(1));
        Assert.That(table.TableNumber, Is.EqualTo(1));
        Assert.That(table.MaxSeats, Is.EqualTo(4));
    }

    [Test]
    public void CreateTable_WithInvalidMaxSeats_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Table(1, 0));
    }
    

    [Test]
    public void SaveAndLoadMenuItems_ShouldPreserveData()
    {
        var menuItem1 = new MenuItem(1, "Pizza", "Delicious cheese pizza", 10.0, true, "Main Course", 15, 800, 0);
        var menuItem2 = new MenuItem(2, "Pasta", "Creamy pasta", 8.0, true, "Main Course", 10, 600, 0);

        MenuItemPersistence.SaveMenuItems(TestFilePath);

        MenuItem.MenuItems.Clear();
        MenuItemPersistence.LoadMenuItems(TestFilePath);

        Assert.That(MenuItem.MenuItems.Count, Is.EqualTo(2));
        Assert.That(MenuItem.MenuItems[0].Name, Is.EqualTo("Pizza"));
        Assert.That(MenuItem.MenuItems[1].Name, Is.EqualTo("Pasta"));
    }

    [Test]
    public void SaveAndLoadIngredients_ShouldPreserveData()
    {
        var ingredient1 = new Ingredient(1, "Cheese", 100, "kg", true);
        var ingredient2 = new Ingredient(2, "Tomato", 200, "kg", true);

        IngredientPersistence.SaveIngredients(TestFilePath);

        Ingredient.Ingredients.Clear();
        IngredientPersistence.LoadIngredients(TestFilePath);

        Assert.That(Ingredient.Ingredients.Count, Is.EqualTo(2));
        Assert.That(Ingredient.Ingredients[0].Name, Is.EqualTo("Cheese"));
        Assert.That(Ingredient.Ingredients[1].Name, Is.EqualTo("Tomato"));
    }

    [Test]
    public void SaveAndLoadOrders_ShouldPreserveData()
    {
        var table = new Table(1, 4);
        var order = new Order(1, table, "Pending");
        var menuItem = new MenuItem(1, "Pizza", "Delicious cheese pizza", 10.0, true, "Main Course", 15, 800, 0);
        var orderItem = new OrderItem(1, menuItem, 2, "");

        order.AddOrderItem(orderItem);

        OrderPersistence.SaveOrders(TestFilePath);

        Order.Orders.Clear();
        OrderPersistence.LoadOrders(TestFilePath);

        Assert.That(Order.Orders.Count, Is.EqualTo(1));
        Assert.That(Order.Orders[0].OrderItems.Count, Is.EqualTo(1));
        Assert.That(Order.Orders[0].TotalAmount, Is.EqualTo(20.0));
    }

    
    [TearDown]
    public void Cleanup()
    {
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
    }
}
