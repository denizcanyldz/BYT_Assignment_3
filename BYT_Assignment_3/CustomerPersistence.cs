public class CustomerPersistence : Persistence<Customer>
{
    public static void SaveCustomers(string filePath){
        Save(filePath, Customer.customers);
    
    }
    public static void LoadCustomers(string filePath){
        Customer.customers = Load(filePath);
    }
}
