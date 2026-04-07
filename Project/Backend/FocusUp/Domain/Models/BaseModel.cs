using System;

public abstract class BaseModel
{ 
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }

    public void SetId(int id) => Id = id;

    public void SetCreatedAt(DateTime createdAt) =>  CreatedAt = createdAt;

    public void SetUpdatedAt(DateTime updatedAt) => UpdatedAt = updatedAt;

    public virtual bool ValidateData()
    {
        throw new NotImplementedException();
    }

    public virtual void UpdateDate()
    {
        throw new NotImplementedException();
    }

}