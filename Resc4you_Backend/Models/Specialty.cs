namespace Resc4you_Backend.Models
{
    public class Specialty
    {
        int specialtyId;
        string specialtyName;
        string specialtyIcon;
        bool isActive;

        public int SpecialtyId { get => specialtyId; set => specialtyId = value; }
        public string SpecialtyName { get => specialtyName; set => specialtyName = value; }
        public string SpecialtyIcon { get => specialtyIcon; set => specialtyIcon = value; }
        public bool IsActive { get => isActive; set => isActive = value; }

        public List<Specialty> Read()
        {
            DBservicesSpecialty dbs=new DBservicesSpecialty();
            return dbs.ReadSpecialty();
        }

        public List<Specialty> ReadVolunteerSpecialty(string volunteerPhone)
        {
            DBservicesSpecialty dbs = new DBservicesSpecialty();
            return dbs.ReadVolunteerSpecialty(volunteerPhone);
        }

        public int DeletePersonSpecialty(string volunteerPhone)
        {
            DBservicesSpecialty dbs = new DBservicesSpecialty();
            return dbs.DeletePersonSpecialty(volunteerPhone);
        }
        public int Insert()
        {
            DBservicesSpecialty dbs = new DBservicesSpecialty();
            return dbs.insertNewSpecialty(this);
        }
    }
}
