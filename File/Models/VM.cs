//using System.ComponentModel.DataAnnotations;

//namespace File.Models
//{
//    public class FileModel
//    {
//        [Key]
//        public int Id { get; set; }
//        public string FileName { get; set; }
//        public byte[]? FileContent { get; set; }



//        [Required(ErrorMessage = "First Name is required")]
//        [MaxLength(10, ErrorMessage = "First Name cannot exceed 10 characters")]
//        public string FirstName { get; set; }

//        [Required(ErrorMessage = "Last Name is required")]
//        [MaxLength(10, ErrorMessage = "Last Name cannot exceed 10 characters")]
//        public string LastName { get; set; }

//        [Required(ErrorMessage = "Email is required")]
//        [EmailAddress(ErrorMessage = "Invalid Email Address")]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "Date of Birth is required")]
//        public DateTime DateOfBirth { get; set; }

//        public int Mobile { get; set; }

//        [Required(ErrorMessage = "Phone Number is required")]
//        public int Phone { get; set; }



//        [Required(ErrorMessage = "Address is required")]
//        public string? Address { get; set; }

//        public string City { get; set; }

//        public string Qualification { get; set; }

//        [Required(ErrorMessage = "Job Category is required")]
//        public string JobCategory { get; set; }

//        public string Position { get; set; }

//        public string KeySkills { get; set; }




//        public bool HasExperience { get; set; }



//    }

//}
