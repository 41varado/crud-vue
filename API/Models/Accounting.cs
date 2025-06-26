using MongoDB.Bson;

namespace API.Models
{
    public class Accounting
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
