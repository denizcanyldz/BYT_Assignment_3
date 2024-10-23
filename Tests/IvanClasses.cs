using BYT_Assignment_3.Models;
using BYT_Assignment_3;

[TestFixture]
public class StaffTests
{
    private const string TestFilePath = "staffTestData.json";

    [SetUp]
    public void SetUp()
    {
        Bartender.Bartenders.Clear();
        Chef.Chefs.Clear();
        Waiter.Waiters.Clear();
        Manager.Managers.Clear();
    }

    [Test]
    public void Create_Bartender_With_Valid_Data_Should_Add_To_BartenderList()
    {
        var bartender = new Bartender(1, "Jake", 987654321);
        Assert.That(Bartender.Bartenders.Count, Is.EqualTo(1));
        Assert.That(bartender.name, Is.EqualTo("Jake"));
    }

    [Test]
    public void Create_Bartender_With_Empty_Name_Should_Throw_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Bartender(2, "", 987654321));
    }

    [Test]
    public void Save_And_Load_Bartenders_Should_Preserve_Data()
    {
        var Bartender1 = new Bartender(1, "Jake", 987654321);
        BartenderPersistence.SaveBartenders(TestFilePath);

        Bartender.Bartenders.Clear();
        BartenderPersistence.LoadBartenders(TestFilePath);

        Assert.That(Bartender.Bartenders.Count, Is.EqualTo(1));
        Assert.That(Bartender.Bartenders[0].name, Is.EqualTo("Jake"));
    }

    [Test]
    public void Create_Chef_With_Valid_Data_Should_Add_To_ChefList()
    {
        var chef = new Chef(1, "Gordon", 123456789, "Baker");
        Assert.That(Chef.Chefs.Count, Is.EqualTo(1));
        Assert.That(chef.name, Is.EqualTo("Gordon"));
    }

    [Test]
    public void Create_Chef_With_Empty_Specialty_Should_Throw_exception()
    {
        Assert.Throws<ArgumentException>(() => new Chef(2, "Gordon", 123456789, ""));
    }

    [Test]
    public void Chef_Should_Be_Able_To_Prepare_Menu_Item()
    {
        var chef = new Chef(1, "Gordon", 123456789, "Italian");
        var menuItem = new MenuItem(1, "Pizza", "Delicious cheese pizza", 12.99, true, "Main Course", 15, 800, 0);

        chef.PrepareMenuItem(menuItem);
        Assert.That(chef.MenuItemsPrepared.Count, Is.EqualTo(1));
        Assert.That(menuItem.PreparedBy, Is.EqualTo(chef));
    }

    [Test]
    public void Save_And_Load_Chefs_Should_Preserve_Data()
    {
        var chef1 = new Chef(1, "Gordon", 123456789, "Baker");
        ChefPersistence.SaveChefs(TestFilePath);

        Chef.Chefs.Clear();
        ChefPersistence.LoadChefs(TestFilePath);

        Assert.That(Chef.Chefs.Count, Is.EqualTo(1));
        Assert.That(Chef.Chefs[0].name, Is.EqualTo("Gordon"));
    }

    [Test]
    public void Create_Waiter_With_Valid_Data_Should_Add_To_WaiterList()
    {
        var table = new Table(1, 4);
        var order = new Order(1, table, "Confirmed");
        var waiter = new Waiter(1, "Sarah", 123456789, order);

        Assert.That(Waiter.Waiters.Count, Is.EqualTo(1));
        Assert.That(waiter.name, Is.EqualTo("Sarah"));
    }

    [Test]
    public void Create_Waiter_With_Empty_Name_Should_Throw_Exception()
    {
        var table = new Table(1, 4);
        var order = new Order(1, table, "Confirmed");
        Assert.Throws<ArgumentException>(() => new Waiter(2, "", 123456789, order));
    }

    [Test]
    public void Save_And_Load_Waiters_Should_Preserve_Data()
    {
        var table = new Table(1, 4);
        var order = new Order(1, table, "Confirmed");
        var waiter1 = new Waiter(1, "Sarah", 123456789, order);
        WaiterPersistence.SaveWaiters(TestFilePath);

        Waiter.Waiters.Clear();
        WaiterPersistence.LoadCustomers(TestFilePath);

        Assert.That(Waiter.Waiters.Count, Is.EqualTo(1));
        Assert.That(Waiter.Waiters[0].name, Is.EqualTo("Sarah"));
    }

    [Test]
    public void Create_Manager_With_Valid_Data_Should_Add_To_ManagerList()
    {
        var manager = new Manager(1, "Michael", 987654321);

        Assert.That(Manager.Managers.Count, Is.EqualTo(1));
        Assert.That(manager.name, Is.EqualTo("Michael"));
    }

    [Test]
    public void Create_Manager_With_Empty_Name_Should_Throw_Exception()
    {
        Assert.Throws<ArgumentException>(() => new Manager(2, "", 987654321));
    }

    [Test]
    public void SaveAndLoad_Managers_Should_Preserve_Data()
    {
        var manager1 = new Manager(1, "Michael", 987654321);
        ManagerPersistence.SaveCustomers(TestFilePath);

        Manager.Managers.Clear();
        ManagerPersistence.LoadCustomers(TestFilePath);

        Assert.That(Manager.Managers.Count, Is.EqualTo(1));
        Assert.That(Manager.Managers[0].name, Is.EqualTo("Michael"));
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
