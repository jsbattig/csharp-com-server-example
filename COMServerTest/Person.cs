using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace TestComObject
{
  [ClassInterface(ClassInterfaceType.None)]
  public class Person : System.EnterpriseServices.ServicedComponent, IPerson
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsMale { get; set; }
    public void Persist(string FilePath)
    {
      StreamWriter oFile = new StreamWriter(FilePath);
      XmlSerializer oXmlSerializer = new XmlSerializer(typeof(Person));
      oXmlSerializer.Serialize(oFile, this);
      oFile.Flush();
      oFile.Close();
    }
  }
}
