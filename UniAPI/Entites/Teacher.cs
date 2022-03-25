namespace UniAPI.Entites
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PESEL { get; set; }
        public string Specialization { get; set; }
        public virtual Address Address { get; set; }
    }
}
