using System;

public abstract class BaseModel
{ 
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; } = DateTime.Now;
    public DateTime UpdatedAt { get; protected set; }

    public virtual bool ValidateData()
    {
        throw new NotImplementedException();
    }

    public virtual void UpdateDate()
    {
        throw new NotImplementedException();
    }

}