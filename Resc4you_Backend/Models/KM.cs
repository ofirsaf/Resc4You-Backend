namespace Resc4you_Backend.Models
{
    public class KM
    {
        int distance;

        public int Distance { get => distance; set => distance = value; }


        public int Update()
        {
            DBservicesKM dbs = new DBservicesKM();
            return dbs.Update(this);
        }

        public int GetKM()
        {
            DBservicesKM dbs = new DBservicesKM();
            return dbs.GetKM();
        }
    }
}
