using System;

public class User : BaseModel
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    public User()
	{
	}

    public User(string username, string email, string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }

    internal void SetId(int id)
    {
        Id = id;
    }

    public void UpdateProfile(string username, string email)
    {
        Username = username;
        Email = email;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }

    public override bool ValidateData()
    {
        return
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(PasswordHash);
    }

    public override void UpdateDate()
    {
        UpdatedAt = DateTime.Now;
    }
}
