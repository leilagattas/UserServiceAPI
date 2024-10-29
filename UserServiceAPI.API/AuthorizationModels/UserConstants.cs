namespace UserServiceAPI.API.AuthorizationModels
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { Username = "admin", Password = "admin_pass", Role = "Administrator"},
            new UserModel() { Username = "manager", Password = "manager_pass", Role = "Manager"}
        };
    }
}
