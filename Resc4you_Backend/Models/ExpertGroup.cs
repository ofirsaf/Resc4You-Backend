namespace Resc4you_Backend.Models
{
    public class ExpertGroup
    {
        int expertGroupId;
        string expertGroupName;

        public int ExpertGroupId { get => expertGroupId; set => expertGroupId = value; }
        public string ExpertGroupName { get => expertGroupName; set => expertGroupName = value; }

        public List<ExpertGroup> Read()
        {
            DBservicesExpertGroup dbs = new DBservicesExpertGroup();
            return dbs.ReadExpertGroup();
        }
    }
}
