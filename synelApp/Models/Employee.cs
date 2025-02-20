using System.ComponentModel.DataAnnotations;

namespace synelApp.Models
{
    public class Employee // the table model
    {
        [Key] // setting primary key Id
        public int Id { get; set; }

        [Required]
        [StringLength(50)] // char length max
        public string Payroll_Number { get; set; }

        [Required]
        [StringLength(100)]
        public string Forenames { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Required]
        public DateTime Date_of_Birth { get; set; }

        [StringLength(20)]
        public string Telephone { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string Address_2 { get; set; }

        [StringLength(20)]
        public string Postcode { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string EMail_Home { get; set; }

        [Required]
        public DateTime Start_Date { get; set; }
    }
}
