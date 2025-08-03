namespace SubaruBOT.Models
{
    public class UserCharacter
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int CharacterId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}