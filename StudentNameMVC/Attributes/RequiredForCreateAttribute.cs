using System.ComponentModel.DataAnnotations;

namespace StudentNameMVC.Attributes
{
    public class RequiredForCreateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            // This will be handled in the controller logic
            // For create operations, password is required
            // For update operations, password is optional
            return true;
        }
    }
}
