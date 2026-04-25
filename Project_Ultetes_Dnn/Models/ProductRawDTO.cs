using System;

namespace Ultetes.Dnn.Project_Ultetes_Dnn.Models
{
    // Ez az osztály csak az SQL lekérdezés nyers adatainak befogadására szolgál.
    // Ez garantálja, hogy ha az adatbázisban NULL van (pl. hiányzó UrlSlug), nem omlik össze a C#.
    public class ProductRawDTO
    {
        public string ProductTypeId { get; set; }
        public string ProductBvin { get; set; }
        public string UrlSlug { get; set; }
        public string ProductName { get; set; }
        public string ProductTypeName { get; set; }
        public string CategoryName { get; set; }
    }
}