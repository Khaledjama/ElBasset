using System.ComponentModel.DataAnnotations;


namespace ElBasset.DataBase
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Insert Department Name")]
        public string Name { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }
    }
}
