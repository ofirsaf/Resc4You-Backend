namespace Resc4you_Backend.Models
{
    public class Language
    {
        int languageId;
        string languageName;

        public int LanguageId { get => languageId; set => languageId = value; }
        public string LanguageName { get => languageName; set => languageName = value; }


        public List<Language> Read()
        {
            DBservicesLanguage dbs = new DBservicesLanguage();
            return dbs.ReadLanguage();
        }

        public List<Language> ReadPersonLanguge(string personPhone)
        {
            DBservicesLanguage dbs = new DBservicesLanguage();
            return dbs.ReadPersonLanguage(personPhone);
        }

        public int DeletePersonLanguge(string personPhone)
        {
            DBservicesLanguage dbs = new DBservicesLanguage();
            return dbs.DeletePersonLanguge(personPhone);
        }
    }
}
