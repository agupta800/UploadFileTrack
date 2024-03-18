using System.ComponentModel.DataAnnotations;

public class FileModel
{
    [Key]
    public int Id { get; set; }

    public string? Image { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [MaxLength(10, ErrorMessage = "First Name cannot exceed 10 characters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [MaxLength(10, ErrorMessage = "Last Name cannot exceed 10 characters")]
    public string LastName { get; set; }


    

    public string? TrackingId { get; set; } // New property for tracking ID

    public bool IsApproved { get; set; } // New property for approval status
}
