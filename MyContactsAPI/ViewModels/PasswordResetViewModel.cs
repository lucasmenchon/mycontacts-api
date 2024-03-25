namespace MyContactsAPI.ViewModels
{
    public class PasswordResetViewModel
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
