namespace TestComObject
{
  public interface IPerson
  {
    string FirstName { get; set; }
    bool IsMale { get; set; }
    string LastName { get; set; }

    void Persist(string FilePath);
  }
}