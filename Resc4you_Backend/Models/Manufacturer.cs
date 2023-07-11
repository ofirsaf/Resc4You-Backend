namespace Resc4you_Backend.Models
{
    public class Manufacturer
    {
        int manufacturerId;
        string manufacturerName;

        public int ManufacturerId { get => manufacturerId; set => manufacturerId = value; }
        public string ManufacturerName { get => manufacturerName; set => manufacturerName = value; }

        public List<Manufacturer> Read()
        {
            DBservicesManufacture dbs = new DBservicesManufacture();
            return dbs.ReadManufacturer();
        }

        public int Insert()
        {
            DBservicesManufacture dbs = new DBservicesManufacture();
            return dbs.insertNewManufacturer(this);
        }

    }
}
